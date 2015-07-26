//#region Apache License
/**
 * Copyright 2015 StrixIT. Author R.G. Schurgers MA MSc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
//#endregion

(function () {
    'use strict';

    // Todo: create a cms widgets module?
    angular.module('strixAdmin').controller('commentcontroller', ['$scope', '$timeout', 'dataService', 'entityService', function ($scope, $timeout, dataService, entityService) {
        return commentController($scope, $timeout, dataService, entityService);
    }]);

    angular.module('strixSite').controller('commentcontroller', ['$scope', '$timeout', 'dataService', 'entityService', function ($scope, $timeout, dataService, entityService) {
        return commentController($scope, $timeout, dataService, entityService);
    }]);

    function commentController($scope, $timeout, dataService, entityService) {
        var type = null;
        var entityEntityTypeId = null;
        var entityId = null;
        var entityCulture = null;
        var entityVersionNumber = 0;
        var newResponseHeader = null;
        var editResponseHeader = null;
        var respondToResponseHeader = null;

        $scope.loading = false;
        $scope.isEditing = false;
        $scope.currentComment = {};
        $scope.formHeader = null;
        $scope.supportGravatar = false;
        $scope.isSaving = false;

        $scope.getLoadingImage = strixIT.getLoadingImage;
        $scope.init = init;
        $scope.getScopeComments = getScopeComments;
        $scope.save = save;
        $scope.delete = deleteComment;
        $scope.confirmDelete = confirmDeleteComment;
        $scope.respond = respond;
        $scope.edit = edit;
        $scope.cancelEdit = cancelEdit;
        $scope.refresh = refreshComments;

        $scope.$on('entityLoaded', entityLoaded);
        $scope.$on('$commentsRequested', commentsRequested);

        function init(type, entityTypeId, newHeader, editHeader, respondHeader, loadForEntityId) {
            type = type;
            newResponseHeader = newHeader;
            editResponseHeader = editHeader;
            respondToResponseHeader = respondHeader;
            $scope.formHeader = newResponseHeader;
            entityEntityTypeId = entityTypeId;
            $scope.supportGravatar = window.md5 != undefined;

            if (loadForEntityId && loadForEntityId != strixIT.config.emptyGuid) {
                setValues(loadForEntityId);
                getComments({ entityId: loadForEntityId });
            }
        }

        function getScopeComments() {
            if (type) {
                return $scope[type + 'Comments'];
            }

            return $scope.comments;
        }

        function entityLoaded(event, data) {
            setValues(data.entityId, data.culture, data.versionNumber);
        }

        function commentsRequested(event, data) {
            var comments = $scope.getScopeComments();

            if (!comments || !comments.length) {
                getComments(data.entity, data.element);
            }
            else {
                $timeout(function () {
                    resetEditor(data.element);
                }, 300);
            }
        }

        function save(event) {
            event.preventDefault();

            if ($scope.validator.validate()) {
                $scope.isSaving = true;

                // A workaround is needed to get the comment text when two comment forms are on the same page.
                var area = $(event.target).closest('form').find('textarea');
                var editor = tinymce.get(area[0].id);
                $scope.currentComment.text = editor.getContent() || $scope.currentComment.text;

                var isNew = !$scope.currentComment.id;

                dataService.callServer('/Comment/SaveComment', { model: $scope.currentComment }).then(function (data) {
                    resetForm(event.currentTarget);

                    if ($scope.supportGravatar) {
                        data.gravatarLink = getGravatarLink(data.createdByEmail);
                    }

                    data.entityTypeId = entityEntityTypeId;

                    if (isNew && !data.parentId) {
                        $scope.getScopeComments().push(data);
                    }
                    else if (isNew && data.parentId) {
                        var parent = findComment(data.parentId);

                        if (!parent.childComments) {
                            parent.childComments = [];
                        }

                        parent.childComments.push(data);
                    }
                    else {
                        var existing = findComment(data.id);
                        angular.copy(data, existing);
                    }

                    $scope.isSaving = false;
                    $scope.isEditing = false;
                });
            }
        }

        function deleteComment(comment, event) {
            $scope.isEditing = false;
            comment.entityTypeId = entityEntityTypeId;
            entityService.storeData('commentToDelete', comment);
            entityService.storeData('elementTriggeringDelete', event.currentTarget);
            var win = $('#commentconfirm').data('kendoWindow');
            win.center().open();
        }

        function confirmDeleteComment() {
            var commentToDelete = entityService.getData('commentToDelete');

            dataService.callServer('/Comment/DeleteComment', { model: commentToDelete }).then(function (data) {
                resetForm(entityService.getData('elementTriggeringDelete'));
                removeComment($scope, commentToDelete.id);
                entityService.storeData('commentToDelete', null);
                entityService.storeData('elementTriggeringDelete', null);
                var win = $('#commentconfirm').data('kendoWindow');
                win.close();
            });
        }

        function respond(comment, event) {
            $scope.isEditing = true;
            $scope.formHeader = respondToResponseHeader;
            refreshCurrentComment();
            $scope.currentComment.parentId = comment.id;
            placeForm(event.currentTarget);
        }

        function edit(comment, event) {
            $scope.isEditing = true;
            $scope.formHeader = editResponseHeader;
            $scope.currentComment = angular.copy(comment);
            placeForm(event.currentTarget);
        }

        function cancelEdit(event) {
            $scope.isEditing = false;
            resetForm(event.currentTarget);
        }

        // Needed by RutgerEnSanne, do not delete
        function refreshComments(entity, element) {
            setValues(entity.entityId, entity.culture, entity.versionNumber);
            getComments(entity, element);
        }

        function getComments(entity, element) {
            $scope.loading = true;

            dataService.callServer(strixIT.config.rootUrl + 'Comment/GetComments', { entityId: entity.entityId, culture: entity.culture, versionNumber: entity.versionNumber }).then(function (data) {
                if ($scope.supportGravatar) {
                    for (var n in data.childComments) {
                        data.childComments[n].gravatarLink = getGravatarLink(data.childComments[n].createdByEmail);
                    }
                }

                $scope.loading = false;

                if (type) {
                    $scope[type + 'Comments'] = data.childComments;
                }
                else {
                    $scope.comments = data.childComments;
                }

                if (!element) {
                    element = angular.element('.currentComment')[0];
                }

                $timeout(function () {
                    resetEditor(element);
                }, 50);
            });
        }

        function getGravatarLink(email) {
            if (email) {
                return 'http://www.gravatar.com/avatar/' + md5(email.trim().toLowerCase()) + '?s=100';
            }
        }

        function setValues(id, culture, versionNumber) {
            entityId = id;
            entityCulture = culture;
            entityVersionNumber = versionNumber;
            refreshCurrentComment();
        }

        function findComment(id, comments) {
            var commentsToSearch = comments ? comments : $scope.getScopeComments();

            for (var n in commentsToSearch) {
                var currentComment = commentsToSearch[n];

                if (currentComment.id == id) {
                    return currentComment;
                }

                if (currentComment.childComments) {
                    var foundComment = findComment(id, currentComment.childComments);
                    if (foundComment) {
                        return foundComment;
                    }
                }
            }
        }

        function removeComment(scope, id, parent, comments) {
            var scopeComments = scope.getScopeComments();
            var commentsToSearch = comments ? comments : scopeComments;

            for (var n in commentsToSearch) {
                var currentComment = commentsToSearch[n];

                if (currentComment.id == id) {
                    if (parent) {
                        var index = parent.childComments.indexOf(currentComment);

                        if (index > -1) {
                            parent.childComments.splice(index, 1);
                        }
                    }
                    else {
                        var index = scopeComments.indexOf(currentComment);

                        if (index > -1) {
                            scopeComments.splice(index, 1);
                        }
                    }
                    return true;
                }

                if (currentComment.childComments) {
                    var removed = removeComment(scope, id, currentComment, currentComment.childComments);
                    if (removed) {
                        return;
                    }
                }
            }
        }

        function refreshCurrentComment() {
            $scope.currentComment = {};
            $scope.currentComment.text = '';
            $scope.currentComment.entityTypeId = entityEntityTypeId;
            $scope.currentComment.entityId = entityId;
            $scope.currentComment.entityCulture = entityCulture;
            $scope.currentComment.entityVersion = entityVersionNumber == 0 ? 1 : entityVersionNumber;
        }

        function resetForm(element) {
            refreshCurrentComment();
            $scope.formHeader = newResponseHeader;
            // $scope.comment is the form.
            $scope.comment.text.$dirty = false;

            var form = $(element).closest('.cms-comment-data').find('.commentform');
            var list = $(element).closest('.cms-comment-list');

            if (list.length == 0) {
                list = form.siblings('.cms-comment-list');
            }

            moveForm(form, list, '');
        }

        function placeForm(element) {
            var li = $(element).closest('li');
            var commentForm = $(element).closest('.cms-comment-data').find('.commentform');
            moveForm(commentForm, li, $scope.currentComment.text);
        }

        function moveForm(form, targetElement, value) {
            var editorElement = $(form).find('.currentComment')[0];
            tinyMCE.execCommand('mceRemoveEditor', false, editorElement.id);
            form.detach().appendTo(targetElement);
            tinyMCE.execCommand("mceAddEditor", false, editorElement.id);
            var editor = tinymce.get(editorElement.id);
            editor.setContent(value);
            editor.focus();
            editor.selection.select(editor.getBody(), true);
            editor.selection.collapse(false);
            form.find('span.k-tooltip-validation').hide();

            $('html, body').animate({
                scrollTop: form.offset().top
            }, 500);

            $timeout(function () {
                $scope.validator.hideMessages();
            }, 0);
        }

        function resetEditor(element) {
            var editorElement = $(element).hasClass('currentComment') ? element : $(element).find('.currentComment')[0];

            if (editorElement) {
                tinyMCE.execCommand('mceRemoveEditor', false, editorElement.id);
                tinyMCE.execCommand("mceAddEditor", false, editorElement.id);
            }
        }
    }
})();