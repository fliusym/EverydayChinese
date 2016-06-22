angular.module('learnChineseApp.directive')
.directive('edcAddScenario', ['$compile', function ($compile) {
    'use strict';

    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/addScenario.html',
        transclude: true,
        scope: {
            scenario: '='
        },
        controller: ['$scope', function ($scope) {
            var images = $scope.scenario['images'] = [];
            $scope.imageIndex = 1;
            this.addImage = function (image) {
                image.index = $scope.imageIndex++;
                images.push(image);

            }
        }],
        link: function (scope, element, attrs) {
            scope.onAddMoreImage = function () {
                var el = $compile("<edc-add-scenario-image><edc-add-scenario-word></edc-add-scenario-word></edc-add-scenario-image>")(scope);
                var finalBr = angular.element(document.querySelector("#finalBreak"));
                if (finalBr) {
                    el.insertBefore(finalBr);
                }
            }
        }
    };

}]);