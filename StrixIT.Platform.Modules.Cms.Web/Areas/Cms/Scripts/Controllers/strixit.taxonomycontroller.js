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

    angular.module('strixAdmin').controller('taxonomycontroller', ['$scope', 'dataService', function ($scope, dataService) {
        $scope.newTag = {};
        $scope.tags = [];
        $scope.invalidName = false;
        $scope.exists = false;

        $scope.addTag = addTag;

        $scope.$on('entityLoaded', entityLoaded);

        function entityLoaded(event, vocabulary) {
            $scope.newTag.name = '';
            $scope.newTag.vocabularyId = vocabulary.id;

            if (!$scope.isNew(vocabulary)) {
                dataService.callServer('/Vocabulary/GetTags', { vocabularyId: vocabulary.id }).then(function (data) {
                    $scope.tags = data;
                });
            }
        }

        function addTag() {
            $scope.invalidName = false;
            $scope.exists = false;
            $scope.newTag.name = $scope.newTag.name.trim();
            var isEmpty = $scope.newTag.name == '';

            if (isEmpty) {
                $scope.invalidName = true;
                return;
            }

            var exists = $.grep($scope.tags, function (x) { return x.name.toLowerCase() == $scope.newTag.name.toLowerCase() }).length > 0;

            if (exists) {
                $scope.exists = true;
                return;
            }

            dataService.callServer('/Vocabulary/SaveTag', { model: $scope.newTag }).then(function () {
                $scope.tags.push(angular.copy($scope.newTag));
                $scope.newTag.name = null;
            });
        }
    }]);
})()