angular.module('learnChineseApp.directive')
.directive('edcAddScenarioWord', [function () {
    'use strict';

    return {
        restrict: 'E',
        require: '^^edcAddScenarioImage',
        templateUrl: '/Client/views/directiveViews/addScenarioWord.html',
        scope: {
        },
        controller: ['$scope', function ($scope) {
            $scope.word = {};
        }],
        link: function (scope, element, attrs, imageCtrl) {
            imageCtrl.addWord(scope);
        }
    };

}]);