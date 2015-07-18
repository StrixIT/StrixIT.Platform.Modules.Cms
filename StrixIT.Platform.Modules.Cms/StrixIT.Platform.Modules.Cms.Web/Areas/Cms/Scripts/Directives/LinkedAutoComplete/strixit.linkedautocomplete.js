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

    angular.module('strixDirectives').directive('strixLinkedAutoComplete', ['$rootScope', '$timeout', 'entityService', function ($rootScope, $timeout, entityService) {
        return {
            require: 'ngModel',
            templateUrl: strixIT.config.resourceRootUrl + 'Areas/Cms/Scripts/Directives/LinkedAutoComplete/linked-auto-complete.html',
            link: function (scope, element, attrs, ngModel) {
                var name, modelString, linkedValues, dataSource, textProperty, filteredDataSource, autoComplete;
                scope.name = '';
                scope.textProperty = '';
                checkSetup();
                initAutoComplete();
                scope.clear = clearAutoComplete;
                scope.$watch(modelString, updateAutoComplete);
                scope.$on("kendoWidgetCreated", getAutoComplete);
                scope.$on('filterLinkedAutoCompletes', filterLinkedAutoCompletes);

                function checkSetup() {
                    name = attrs['name'];
                    modelString = attrs['ngModel'];
                    linkedValues = attrs['strixLinkedValues'];
                    dataSource = attrs['strixDataSource'];
                    textProperty = attrs['strixTextProperty']

                    if (!name) {
                        throw new Error('No name specified for linked autocomplete');
                    }

                    if (!linkedValues) {
                        throw new Error('No linked values specified for linked autocomplete with name ' + name);
                    }

                    if (!dataSource) {
                        throw new Error('No data source specified for linked autocomplete with name ' + name);
                    }

                    if (!ngModel) {
                        throw new Error('ngModel not specified for linked autocomplete with name ' + name);
                    }

                    if (!textProperty) {
                        textProperty = 'name';
                    }
                }

                function initAutoComplete() {
                    scope.name = name;
                    scope.textProperty = textProperty;
                    filteredDataSource = new kendo.data.DataSource();
                    linkedValues = linkedValues.split(',');

                    for (var n in linkedValues) {
                        linkedValues[n] = linkedValues[n].trim();
                    }

                    scope.options = {
                        dataSource: filteredDataSource,
                        dataTextField: textProperty,
                        change: function (e) {
                            ngModel.$setViewValue(this.value());
                            $rootScope.$broadcast('filterLinkedAutoCompletes', scope.$id);
                        }
                    };
                }

                function clearAutoComplete() {
                    ngModel.$setViewValue('');
                    autoComplete.value('');
                    $rootScope.$broadcast('filterLinkedAutoCompletes', scope.$id);
                }

                function getAutoComplete(e, widget) {
                    if (widget.$angular_scope.$id == scope.$id) {
                        autoComplete = widget;
                    }
                }

                function updateAutoComplete(newValue, oldValue) {
                    $timeout(function () {
                        if (newValue != oldValue) {
                            autoComplete.element.val(newValue);
                        }

                        filterDataSource();
                    }, 50);
                }

                function filterLinkedAutoCompletes(ev, triggeringScopeId) {
                    if (triggeringScopeId != scope.$id) {
                        filterDataSource();
                    }
                }

                function filterDataSource() {
                    var allValues = scope[dataSource].data();
                    var entity = entityService.getEntity();

                    var filteredValues = $.grep(allValues, function (d) {
                        var inUse = false;

                        for (var n in linkedValues) {
                            if (getPropertyValue(linkedValues[n], entity) == d[textProperty]) {
                                inUse = true;
                                break;
                            }
                        }

                        return !inUse;
                    });

                    filteredDataSource.data(filteredValues);
                }

                function getPropertyValue(name, data) {
                    var propertyName = name;
                    var parts = propertyName.split('.');
                    var propertyValue = null;

                    if (parts.length > 1) {
                        var selector = data;

                        for (var n in parts) {
                            selector = selector[parts[n]];
                        }

                        propertyValue = selector;
                    }
                    else {
                        propertyValue = data[propertyName];
                    }

                    return propertyValue;
                }
            }
        }
    }]);
})()