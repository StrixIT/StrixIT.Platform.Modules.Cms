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

    angular.module('strixAdmin').controller('createdocumentcontroller', ['$scope', '$timeout', 'dataService', 'entityService', function ($scope, $timeout, dataService, entityService) {
        $scope.documents = [];
        $scope.tags = [];
        $scope.tagsLoaded = false;
        $scope.editMode = false;
        $scope.valid = true;
        $scope.filterText = '';
        $scope.saving = false;

        $scope.init = init;
        $scope.editDocuments = editDocuments;
        $scope.iconToShow = function (document) { if (document) { return entityService.getImageIcon(document); } };
        $scope.showIcon = function (document) { if (document) { return !document.image && $scope.iconToShow(document) != null; } }

        // Todo: use entity controller filter tags.
        $scope.filterTags = filterTags;

        $scope.documentSelected = function (document) { document.selected = !document.selected; }
        $scope.addToDocuments = addToDocuments;
        $scope.removeTag = removeTag;
        $scope.saveDocuments = saveDocuments;

        function init() {
            dataService.callServer('/Document/GetAllTags').then(function (results) {
                $scope.tags = results;
                $scope.tagsLoaded = true;
            });
        }

        function showIcon(document) {
            return document.image == undefined || document.image == null;
        }

        function editDocuments(data) {
            $scope.documents = [];

            for (var n in data) {
                var item = data[n];

                $scope.documents.push({
                    fileId: item.fileId,
                    image: item.image,
                    name: '',
                    date: '',
                    documentType: item.documentType,
                    extension: item.extension
                });
            }

            $scope.editMode = true;

            var sortable = $('#documentlist').data('kendoSortable');

            // Todo: KendoBug, report to Kendo, must have another way to do this.
            sortable._draggable.options.ignore = 'input';

            sortable.bind('end', function (e) {
                var temp = [];
                var children = e.sender.element.children('li');

                for (n = 0; n < children.length; n++) {
                    var currentElement = children[n];

                    if (e.item[0] == currentElement) {
                        continue;
                    }

                    var current = $.grep($scope.documents, function (x) { return x.fileId == $(currentElement).attr('data-id'); })[0];
                    temp.push(current);
                }

                $scope.documents = temp;
                $scope.$apply();
            });
        }

        // Todo: use filter and get tags from entities controller.
        function filterTags() {
            var tags = $scope.tags;

            for (var n in tags) {
                var tag = tags[n];

                if (!$scope.filterText || tag.name.toLowerCase().indexOf($scope.filterText.toLowerCase()) > -1) {
                    tag.show = true;
                }
                else {
                    tag.show = false;
                }
            }
        }

        function addToDocuments(tag) {
            for (var n in $scope.documents) {
                var document = $scope.documents[n];

                if (document.selected) {
                    var hasTag = false;

                    if (!document.tags) {
                        document.tags = [];
                    }
                    else {
                        hasTag = $.grep(document.tags, function (x) { return x.name.toLowerCase() == tag.name.toLowerCase(); }).length > 0;
                    }

                    if (!hasTag) {
                        var copy = angular.copy(tag);
                        copy.Selected = true;
                        document.tags.push(copy);
                    }
                }
            }
        }

        function removeTag(document, tag) {
            if (document.tags) {
                var existing = $.grep(document.tags, function (x) { return x.id == tag.id; })[0];

                if (existing) {
                    var index = document.tags.indexOf(existing);
                    document.tags.splice(index, 1);
                }
            }
        }

        function saveDocuments() {
            $scope.valid = true;
            $scope.saving = true;

            for (var n in $scope.documents) {
                if ($scope.documents[n].name == '') {
                    $scope.valid = false;
                    break;
                }
            }

            if ($scope.valid) {
                var models = [];

                for (var n in $scope.documents) {
                    var model = angular.copy($scope.documents[n]);
                    model.Image = null;
                    models.push(model);
                }

                dataService.callServer('/Document/CreateMany', { models: models }).then(function (results) {
                    $scope.saving = false;
                    $scope.editMode = false;
                    $scope.documents = null;

                    var uploadScope = angular.element($('.cms-uploadform')[0]).scope();
                    uploadScope.selectedFile = null;
                    uploadScope.fileSelected = false;

                    $timeout(function () {
                        $('.fileuploader', '.cms-uploadform').val('');
                    }, 0);
                });
            }
        }
    }]);

    strixIT.documentsUploaded = function (data) {
        var scope = angular.element($('#documentcontainer')[0]).scope();
        scope.editDocuments(data);
    }
})()