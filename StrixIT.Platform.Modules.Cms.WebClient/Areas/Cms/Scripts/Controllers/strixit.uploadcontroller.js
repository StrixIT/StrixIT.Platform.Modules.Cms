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

    angular.module('strixAdmin').controller('uploadcontroller', ['$scope', '$timeout', '$http', '$q', '$route', 'entityService', function ($scope, $timeout, $http, $q, $route, entityService) {
        $scope.settings = {};
        $scope.defaultPath = '';
        $scope.fileSelected = false;
        $scope.showFile = false;
        $scope.showUploader = true;
        $scope.savingFile = false;
        $scope.selectedFile = null;
        $scope.files = null;
        $scope.uploadedFile = null;
        $scope.input = null;
        $scope.showDeleteLink = true;
        $scope.showIcon = false
        $scope.iconToShow = null;
        $scope.entityImage = null;
        $scope.entityName = null;

        $scope.getLoadingImage = strixIT.getLoadingImage;
        $scope.initUpload = init;
        $scope.selectFile = selectFile;
        $scope.deleteFile = deleteFile;
        $scope.confirmDeleteFile = confirmDeleteFile;
        $scope.saveFile = saveFile;
        $scope.$on('entityLoaded', entityLoaded);
        $scope.$on('noFileSelected', noFileSelected);

        function init(settingsName) {
            $scope.settings = strixIT.fileUpload[settingsName];

            if ($scope.settings.fileTypes.indexOf('zip') == -1 && $scope.settings.unzip) {
                $scope.settings.fileTypes += ',zip';
            }
        }

        function entityLoaded(event, entity) {
            //Set either the image for the entity, the default image, or no image.
            entity.image = entity[$scope.settings.filePath];
            $scope.uploadedFile = entity[$scope.settings.fileIdPropertyName.substring(0, $scope.settings.fileIdPropertyName.length - 2)];
            $scope.defaultPath = $scope.settings.defaultImage != null ? $scope.settings.defaultImage : '';

            if ($scope.settings.mode == "UploadAndShow") {
                if (!entity.image && $scope.defaultPath) {
                    entity.image = $scope.defaultPath;
                    $scope.showDeleteLink = false;
                }
            }

            setEntityData(entity);
            $scope.savingFile = false;
        }

        function noFileSelected() {
            $scope.selectFileMessageVisible = true;
        }

        function selectFile(element) {
            $scope.selectFileMessageVisible = false;
            $scope.input = $(element);
            var val = $scope.input.val();
            $scope.$apply(function () {
                for (var n = 0; n < $scope.input[0].files.length; n++) {
                    var file = $scope.input[0].files[n];

                    if (file.size / 1024 > $scope.settings.maxSize) {
                        $scope.notify.show($scope.getResource('cms', 'interface', 'fileTooLarge').replace('{0}', $scope.settings.maxSize / 1024), "error");
                        return;
                    }

                    var nameParts = file.name.split('.');
                    var extension = nameParts[nameParts.length - 1].toLowerCase();

                    if ($scope.settings.fileTypes.indexOf(extension) == -1) {
                        $scope.notify.show($scope.getResource('cms', 'interface', 'fileTypeNotAllowed').replace('{0}', extension), "error");
                        return;
                    }
                }

                $scope.fileSelected = val && val != "";
                var fileName = '';

                if (val) {
                    var parts = val.split('\\');
                    fileName = parts[parts.length - 1];
                }

                $scope.selectedFile = fileName;
                $scope.files = $scope.input[0].files;
            });
        }

        function deleteFile() {
            var win = $('#confirmdeletefile').data('kendoWindow');
            win.center().open();
        }

        function confirmDeleteFile() {
            var entity = entityService.getEntity();
            entity[$scope.settings.fileIdPropertyName] = null;
            entity.image = null;

            if ($scope.defaultPath && $scope.settings.mode == "UploadAndShow") {
                $scope.showDeleteLink = false;
                entity.image = $scope.defaultPath;
            }

            setEntityData(entity);
            $scope.fileSelected = false;
            $scope.selectedFile = null;
            $scope.notify.show($scope.getResource('cms', 'interface', 'fileDeletedSuccessfully'), "success");
            var win = $('#confirmdeletefile').data('kendoWindow');
            win.close();
        }

        function saveFile() {
            $scope.showUploader = false;
            $scope.savingFile = true;

            var fd = new FormData();

            for (var i = 0; i < $scope.files.length; i++) {
                var file = $scope.files[i];
                fd.append(file.name, file);
            }

            var keys = Object.keys($scope.settings);

            for (var i = 0; i < keys.length; i++) {
                var key = keys[i];
                var item = $scope.settings[key];
                fd.append(key, item);
            }

            var deferred = $q.defer();

            $http({
                method: 'POST',
                url: strixIT.config.rootUrl + 'File/UploadFile',
                headers: {
                    'Content-Type': undefined
                },
                data: fd,
                transformRequest: fd
            }).success(function (data, status, headers, config) {
                deferred.resolve(data);
            }).error(function (data, status, headers, config) {
                deferred.reject(data);
            });

            deferred.promise.then(function (result) {
                $scope.savingFile = false;
                $scope.fileSelected = false;

                if ($scope.settings.mode == "MultiUpload") {
                    if (result[0].succeeded && $scope.settings.uploadCallback) {
                        $scope.notify.show(result[0].message, "success");
                        strixIT[$scope.settings.uploadCallback](result);
                        $scope.showUploader = true;
                    }
                    else {
                        $scope.notify.show(result[0].message, "error");
                        $scope.showUploader = true;
                    }
                }
                else {
                    result = result[0];
                    $scope.showDeleteLink = true;

                    if (result.succeeded) {
                        $scope.notify.show(result.message, "success");

                        if ($scope.settings.mode == "UploadAndShow") {
                            var entity = entityService.getEntity();
                            entity[$scope.settings.fileIdPropertyName] = result.fileId;
                            entity.image = result.image;
                            entity.documentType = result.documentType;
                            entity.extension = result.extension;
                            setEntityData(entity);

                            $scope.uploadedFile = { originalName: $scope.selectedFile, uploadedOn: new Date() };
                        }
                    }
                    else {
                        $scope.notify.show(result.message, "error");
                        $scope.showUploader = true;
                    }
                }
            }, function (result) {
                $scope.savingFile = false;
                $scope.showUploader = true;
                $scope.notify.show(result.statusText, "error");
            });
        }

        function setEntityData(entity) {
            $scope.iconToShow = entityService.getImageIcon(entity, $scope.settings.filePath);
            $scope.showIcon = !entity.image && $scope.iconToShow != null;
            $scope.entityImage = entity.image;
            $scope.entityName = entity.Name;

            $scope.showFile = $scope.entityImage || ($scope.showIcon && entity[$scope.settings.fileIdPropertyName] && entity[$scope.settings.fileIdPropertyName]!= strixIT.config.emptyGuid);
            $scope.showUploader = !$scope.showFile || ($scope.entityImage && $scope.entityImage == $scope.defaultPath);
        }
    }]);
})()