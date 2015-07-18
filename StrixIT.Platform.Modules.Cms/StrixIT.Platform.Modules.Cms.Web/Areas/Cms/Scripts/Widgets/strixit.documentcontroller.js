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

    angular.module('strixSite').controller('documentcontroller', ['$scope', '$element', '$timeout', 'dataService', function ($scope, $element, $timeout, dataService) {
        $scope.initList = initList;
        $scope.initItem = initItem;

        function initList(jsonFilter) {
            var filters = jsonFilter ? JSON.parse(jsonFilter) : null;

            $scope.documents = dataService.createDataSource({ url: strixIT.config.rootUrl + 'Document/List', filter: { logic: 'and', filters: filters } });

            $scope.documentslistconfig = {
                template: kendo.template($('#documentstemplate').html()),
                dataSource: $scope.documents,
            };
        }

        function initItem(id, mediaType, path) {
            dataService.init('Cms').then(function () {
                $timeout(function () {
                    var container = $element;
                    var image = $('img', container);
                    var videocontainer = $('.videocontainer', container);
                    var video = $('video', container);
                    var dimensions = getDimensions(container);

                    if (mediaType == dataService.getEnum('Cms', 'documentType').video) {
                        var maxSize = dimensions.height >= dimensions.width ? dimensions.width : dimensions.height;
                        var ratio = dimensions.width / dimensions.height;

                        if (dimensions.width > dimensions.height) {
                            dimensions.height = maxSize / ratio;
                            dimensions.width = maxSize;
                        }
                        else {
                            dimensions.height = maxSize;
                            dimensions.width = maxSize * ratio;
                        }

                        dimensions.width = Math.floor(Math.max(dimensions.width / 50, 1)) * 50;
                        dimensions.height = Math.floor(Math.max(dimensions.height / 50, 1)) * 50;

                        video.attr('width', dimensions.width);
                        video.attr('height', dimensions.height);
                    }
                    else {
                        image.attr('src', '/Image/' + dimensions.width + '/' + dimensions.height + '/' + path);
                    }
                }, 0);
            });
        }

        function getDimensions(container) {
            var width = Math.floor(container.width() / 50) * 50;
            var height = Math.floor(container.height() / 50) * 50;
            return { width: width, height: height };
        }
    }]);
})()