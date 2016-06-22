angular.module('learnChineseApp.directive')
.directive('edcAddScenarioImage', [ '$compile', function ($compile) {
    'use strict';

    return {
        restrict: 'E',
        require: '^^edcAddScenario',
        transclude: true,
        templateUrl: '/Client/views/directiveViews/addScenarioImage.html',
        scope: {
        },
        controller: ['$scope', function ($scope) {
            $scope.index = 0;
            $scope.image = {};
            $scope.wordIndex = 1;
            var words = $scope.image['words'] = [];
            this.addWord = function (word) {
                word.index = $scope.wordIndex++;
                word.imageIndex = $scope.index;
                words.push(word);
            }
        }],
        link: function (scope, element, attrs, scenarioCtrl) {
            scenarioCtrl.addImage(scope);
            scope.onAddMoreScenarioWord = function () {
                var el = $compile("<edc-add-scenario-word></edc-add-scenario-word>")(scope);
                var finalBrId = "#imageFinalBr" + scope.index;
                var finalBr = angular.element(document.querySelector(finalBrId));
                if (finalBr) {
                    el.insertBefore(finalBr);
                  //  finalBr.append(el);
                }
            }
        }
    };

}]);