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

    angular.module('strixAdmin').controller('editlanguagepickercontroller', ['$rootScope', '$scope', '$route', 'dataService', 'entityService', function ($rootScope, $scope, $route, dataService, entityService) {
        $scope.hasLanguage = hasLanguage;
        $scope.isCurrentLanguage = isCurrentLanguage;
        $scope.getCulture = getCulture;
        $scope.setEditLanguage = setEditLanguage;

        function hasLanguage(language) {
            var entity = entityService.getEntity();
            return entity && entity.availableCultures && entity.availableCultures.indexOf(language) > -1;
        }

        function isCurrentLanguage(language) {
            var entity = entityService.getEntity();
            return entity && entity.culture == language;
        }

        function getCulture() {
            var entity = entityService.getEntity();
            return entity ? entity.culture : null;
        }

        function setEditLanguage(language) {
            var entity = entityService.getEntity();
            var data = { entityId: entity.entityId, culture: language };

            dataService.callServer(strixIT.config.rootUrl + strixIT.config.routePrefix + $route.current.data.baseRoute + '/GetEntity', data).then(
                function (data) {
                    if (data.id == strixIT.config.emptyGuid) {
                        entity.culture = language;
                        data.culture = language;
                        combineData(entity, data);
                        $rootScope.$broadcast('entityLoaded', data);
                    }
                    else {
                        if (entity.returnUrl && !data.returnUrl) {
                            data.returnUrl = entity.returnUrl;
                        }

                        entityService.storeEntity(data);
                        $rootScope.$broadcast('entityLoaded', data);
                    }
                });
        }

        function combineData(previous, next) {
            for (var key in next) {
                if (!next.hasOwnProperty(key)) {
                    continue;
                }

                var value = next[key];

                if (typeof value === "object" && value) {
                    combineData(previous[key], value);
                }
                else if (!value) {
                    next[key] = previous[key];
                }
            }
        }
    }]);
})()