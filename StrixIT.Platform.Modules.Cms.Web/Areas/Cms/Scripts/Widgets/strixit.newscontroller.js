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

    angular.module('strixSite').controller('newscontroller', ['$scope', 'dataService', function ($scope, dataService) {
        $scope.dataAvailable = false;

        $scope.init = function () {
            $scope.news = dataService.createDataSource({ url: strixIT.config.rootUrl + 'News/List', readCallBack: readCallBack });

            $scope.newslistconfig = {
                template: kendo.template($('#newstemplate').html()),
                dataSource: $scope.news,
            };
        }

        function readCallBack(result) {
            if (result.total > 0) {
                $scope.dataAvailable = true;
            }
        }

    }]);
})()