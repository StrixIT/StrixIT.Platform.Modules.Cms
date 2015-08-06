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

    angular.module('strixAdmin').controller('entitycontroller', ['$rootScope', '$scope', '$location', '$route', '$filter', 'dataService', 'entityService', function ($rootScope, $scope, $location, $route, $filter, dataService, entityService) {
        var restoreVersionConfirmTitle = null;
        var restoreVersionConfirmBody = null;

        $scope.storeData('restoreVersion', restoreVersion);
        $scope.storeData('backToVersionList', backToVersionList);

        $scope.hasTags = false;
        $scope.filterText = '';
        $scope.showTags = showTags;
        $scope.filterTags = filterTags;
        $scope.getTags = getTags;
        $scope.getEntity = entityService.getEntity();
        $scope.getUpdateText = getUpdateText;
        $scope.showIcon = showIcon;
        $scope.iconToShow = entityService.getImageIcon;
        $scope.loadVersions = loadVersions;
        $scope.showVersion = showVersion;
        $scope.confirmRestoreVersion = confirmRestoreVersion;
        $scope.loadComments = loadComments;
        $scope.submit = submit;
        $scope.delete = $scope.deleteEntity;
        $scope.confirmDelete = $scope.confirmDeleteEntity;

        $scope.$on("$locationChangeStart", changeLocation);

        function showTags() {
            var entity = entityService.getEntity();

            if (entity && entity.tags) {
                for (var n in entity.tags) {
                    if (entity.tags[n].selected) {
                        return true;
                    }
                }
            }

            return false;
        }

        function filterTags() {
            var tags = $scope.getTags();

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

        function getTags() {
            var entity = entityService.getEntity();

            if (entity && entity.tags) {
                $scope.hasTags = entity.tags.length > 0;

                if ($scope.hasTags) {
                    for (var n in entity.tags) {
                        var tag = entity.tags[n];
                        tag.name = strixIT.htmlDecode(tag.name);
                    }
                }

                return $filter('orderBy')(entity.tags, ['-selected', 'name']);
            }

            return [];
        }

        function getUpdateText(text) {
            if (text) {
                var entity = entityService.getEntity();

                if (entity)
                {
                    var anonymous = $scope.getResource('cms', 'interface', 'anonymous');
                    var updatedBy = entity.updatedBy ? text.replace('{0}', entity.updatedBy) : text.replace('{0}', anonymous);
                    return updatedBy.replace('{1}', $filter('kendoDate')(entity.updatedOn))
                }
            }
        }

        function showIcon(entity) {
            return entity.filePath == undefined || entity.filePath == null;
        }

        function loadVersions() {
            var scopeVersions = entityService.getOrCreateList({ url: '/GetVersionList', data: { entityId: entityService.getEntity().entityId }, key: 'Versions' });

            if (!$scope[type + 'Versions']) {
                var type = $route.current.data.type;
                $scope[type + 'versionstemplate'] = $('#' + type + 'versionstemplate').html();
                $scope[type + 'Versions'] = scopeVersions;
            }
        }

        function showVersion(item) {
            var type = $route.current.data.type;
            var entity = entityService.getEntity();
            var promise = dataService.getFromServer($location.path().toLowerCase(),
                {
                    entityId: entity.entityId,
                    culture: entity.culture,
                    versionNumber: item.versionNumber
                }, 'getentity');

            promise.then(function (data) {
                setActiveTab(0);
                entityService.storeEntity(data);
                $scope.$emit('versionLoaded', data);
            });
        }

        function backToVersionList() {
            var entity = entityService.getEntity();
            var promise = dataService.getFromServer($location.path().toLowerCase(),
                {
                    entityId: entity.entityId,
                    culture: entity.culture
                }, 'getentity');

            promise.then(function (data) {
                setActiveTab(2);
                entityService.storeEntity(data);
                $scope.$emit('versionLoaded', data);
            });
        }

        function restoreVersion(version) {
            var type = $route.current.data.type;
            var entity = entityService.getEntity();

            if (!version) {
                version = {
                    entityId: entity.entityId,
                    versionNumber: entity.versionNumber,
                    stayOnDetails: true
                };
            }

            $scope.versionToRestore = version;
            var win = $('#confirmrestoreversion').data('kendoWindow');
            var body = win.element.find('.confirmbody');

            if (!restoreVersionConfirmTitle) {
                restoreVersionConfirmTitle = win.title();
            }

            if (!restoreVersionConfirmBody) {
                restoreVersionConfirmBody = body.html();
            }

            win.title(restoreVersionConfirmTitle.replace('{0}', version.versionNumber));
            body.html(restoreVersionConfirmBody.replace('{0}', version.versionNumber).replace('{1}', $scope.toTitleCase(type)).replace('{2}', entity.name));
            win.center().open();
        }

        function confirmRestoreVersion() {
            var promise = dataService.getFromServer($location.path().toLowerCase(),
                {
                    id: $scope.versionToRestore.entityId,
                    versionNumber: $scope.versionToRestore.versionNumber
                }, 'restoreversion');

            promise.then(function (data) {
                var type = $route.current.data.type;
                entityService.storeEntity(data);
                $scope[type + 'Versions'].read();
                $scope.$emit('versionLoaded', data);

                if ($scope.versionToRestore.stayOnDetails) {
                    // Workaround to show tabs when hiding them manually.
                    var tabStrip = $("#tabstrip").data("kendoTabStrip");

                    var hiddenTab = tabStrip.contentElements.filter(function () { return $(this).css('display').toLowerCase() == 'none'; });
                    hiddenTab.css('display', 'block');
                }

                setActiveTab(0);
                var restoreMessage = $scope.getResource('cms', 'interface', 'versionRestoredSuccessfully');
                restoreMessage = restoreMessage.replace('{0}', $scope.versionToRestore.versionNumber);
                $scope.notify.show(restoreMessage, "success");
                var win = $('#confirmrestoreversion').data('kendoWindow');
                win.close();
            });
        }

        function loadComments(event) {
            var type = $route.current.data.type;
            $rootScope.$broadcast('$commentsRequested', { entity: $scope[type], element: $(event).closest('#tabstrip')[0] });
        }

        function setActiveTab(index) {
            var tabStrip = $("#tabstrip").data("kendoTabStrip");
            tabStrip.select(tabStrip.tabGroup.children("li").eq(index));

            // Workaround to show tabs when hiding them manually.
            tabStrip.contentElements.eq(index).css('display', 'block');
        }

        function submit(data) {
            if (!this.validator.validate()) {
                return;
            }

            var type = $route.current.data.type;

            if (data.body && $scope[type + 'form'].body) {
                if (data.body != $scope[type + 'form'].body.$modelValue) {
                    data.body = $scope[type + 'form'].body.$modelValue;
                }
            }

            var args = { type: type, entity: data, isValid: true };
            $rootScope.$broadcast('validatingEntity', args);

            if (!args.isValid) {
                return;
            }

            $scope.saveEntity(data);
        }

        function changeLocation(event) {
            var isTabChange = $location.$$path.indexOf('#') == -1 && $location.$$url.indexOf('#') > 0;

            if (isTabChange) {
                event.preventDefault();
            }
        }
    }])
})()