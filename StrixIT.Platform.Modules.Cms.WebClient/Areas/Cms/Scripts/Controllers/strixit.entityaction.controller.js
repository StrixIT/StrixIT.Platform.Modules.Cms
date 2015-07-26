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

// Code to group services taken from http://stackoverflow.com/questions/14800862/how-can-i-group-data-with-an-angular-filter.
(function () {
    'use strict';

    angular.module('strixAdmin').controller('entityactioncontroller', ['$scope', '$location', '$timeout', '$templateCache', 'dataService', function ($scope, $location, $timeout, $templateCache, dataService) {
        var indexedServices = [];
        var tc = $templateCache;
        var dataSource = new kendo.data.DataSource();

        $scope.entityServices = [];
        $scope.selectedType = null;
        $scope.loadingServiceData = false;
        $scope.savingServiceData = false;

        $scope.servicesToFilter = getServicesToFilter;
        $scope.filterServices = filterServices;
        $scope.save = save;

        $scope.selectOptions = {
            dataSource: dataSource,
            dataTextField: 'item1',
            dataValueField: 'item2',
            change: changeSelect
        }

        $scope.$on("$routeChangeSuccess", function (event, params) { initServiceData(); });

        function initServiceData() {
            $scope.loadingServiceData = true;
            dataService.callServer($location.path() + '/GetData').then(function (data) {
                $scope.loadingServiceData = false;
                $scope.entityServices = data;
                dataSource.data(data);
                $scope.selectedType = data[0];
            });
        }

        function getServicesToFilter() {
            indexedServices = [];

            if ($scope.selectedType) {
                return $scope.selectedType.item3;
            }
        }

        function filterServices(service) {
            var serviceIsNew = indexedServices.indexOf(service.service) == -1;
            if (serviceIsNew) {
                indexedServices.push(service.service);
            }
            return serviceIsNew;
        }

        function save() {
            $scope.savingServiceData = true;

            dataService.callServer($location.path() + '/SaveData', $scope.entityServices).then(function (data) {
                $scope.savingServiceData = false;
                $scope.notify.show($scope.getResource('web', 'interface', 'saveSuccessful'), "success");
                tc.removeAll();
            }, function (error) {
                $scope.notify.show($scope.getResource('web', 'interface', 'saveFailed'), "error");
            });
        }

        function changeSelect(e) {
            $scope.$apply(function () {
                var value = e.sender.element.children().eq(e.sender.selectedIndex)[0].value;

                for (var n in $scope.entityServices) {
                    var service = $scope.entityServices[n];

                    if (service.item2 == value) {
                        $scope.selectedType = service;
                    }
                }
            });
        }
    }]);
})()