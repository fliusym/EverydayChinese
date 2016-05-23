angular.module('learnChineseApp.directive')
.directive('edcQuote', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/quote.html',
        scope: {},

        bindToController: {
            quotes: '=',
            quotetitle: '='
        },
        controller: function () { },
        controllerAs: 'ctrl'
    };
}]);