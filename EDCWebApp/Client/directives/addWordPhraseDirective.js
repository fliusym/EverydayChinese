angular.module('learnChineseApp.directive')
.directive('edcAddWordPhrase', ['$compile', function ($compile) {
    'use strict';

    return {
        restrict: 'E',
        require: '^^edcAddWord',
        transclude: true,
        templateUrl: '/Client/views/directiveViews/addWordPhrase.html',
        scope: {
        },
        controller: ['$scope', function ($scope) {

            $scope.phrase = {};
            $scope.index = 0;
            var examples = $scope.phrase['examples'] = [];
            this.addExample = function (example) {
                examples.push(example);
            }
        }],
        link: function (scope, element, attrs, wordCtrl) {
            wordCtrl.addPhrase(scope);
        }
    };

}]);