angular.module('learnChineseApp.directive')
.directive('edcAddWordQuote', [function () {
    'use strict';

    return {
        restrict: 'E',
        require:'^^edcAddWord',
        templateUrl: '/Client/views/directiveViews/addWordQuote.html',
        scope: {
        },
        controller: ['$scope', function ($scope) {

        }],
        link: function (scope, element, attrs, wordCtrl) {
            wordCtrl.addQuote(scope);
        }
    };

}]);