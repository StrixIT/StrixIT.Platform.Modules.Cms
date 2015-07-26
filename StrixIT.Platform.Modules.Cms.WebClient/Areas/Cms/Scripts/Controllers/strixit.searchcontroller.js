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

    angular.module('strixSite').controller('searchcontroller', ['$scope', '$window', 'dataService', function ($scope, $window, dataService) {
        var locators = null;

        $scope.searchTerm = null;
        $scope.displaySearchTerm = null;
        $scope.init = init;
        $scope.getItemLink = getItemLink;

        function init() {
            // Get the filter from the query string.
            var queryString = $window.location.search;

            var filter = JSON.parse(decodeURIComponent(queryString.substring(1, queryString.length)));

            $scope.searchresults = new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        {
                            $scope.searchTerm = options && options.data && options.data.filter && options.data.filter.filters.length > 0 ? options.data.filter.filters[0].value : null;
                            $scope.displaySearchTerm = $scope.searchTerm.replace('tag:', 'tag ');
                            dataService.callServer(strixIT.config.rootUrl + 'Search/Search', $.extend(true, options.data, this.read.data))
                                .then(function (result) {
                                    locators = result.locators;
                                    options.success(result);
                                },
                                function (result) {
                                    options.error(result);
                                });
                        }
                    }
                },
                schema: {
                    data: 'data',
                    total: 'total'
                },
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                filter: filter.filter,
                page: filter.page,
                pageSize: filter.pageSize
            });

            $scope.searchconfig = {
                template: kendo.template($('#searchtemplate').html()),
                dataSource: $scope.searchresults
            };
        }

        function getItemLink(item) {
            if (item && locators) {
                var valUrl = item.url.toLowerCase();
                var parentPart = valUrl.split('/')[0];
                var childPart = valUrl.split('/')[1];

                // Todo: improve this, do not use magic string.
                var allLocators = item.typeName == 'StrixIT.Platform.Cms.Html' ? [$.grep(locators, function (x) { return x.contentTypeName == item.typeName && x.pageUrl == item.url; })[0]] : $.grep(locators, function (x) { return x.contentTypeName == item.typeName && (x.contentUrl == null || x.contentUrl == valUrl || x.contentUrl == parentPart); });
                var isSubPage = item.typeName == 'StrixIT.Platform.Cms.Html' ? false : $.grep(allLocators, function (x) { return x.contentUrl != null && x.contentUrl != valUrl; }).length > 0;
                var locator = $.grep(allLocators, function (x) { return x.contentUrl == valUrl; })[0];
                locator = locator ? locator : allLocators[0];

                if (locator) {
                    var page = isSubPage ? locator.pageUrl + '/' + childPart : locator.pageUrl;
                    var url = !isSubPage && locator.contentUrl == null ? item.url : null;
                    return strixIT.config.rootUrl + (url != null ? page + '/' + url : page);
                }
            }
        }
    }]);
})();