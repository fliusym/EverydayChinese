angular.module('learnChineseApp.directive')
.directive('edcHeader', ['authenticationFactory', function (authenticationFactory) {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/header.html',
        scope: {},

        bindToController: {
            loggedinInfo: '=?'
        },
        controller: function () {
            this.loggedinInfo = authenticationFactory.getLoginInfo();
        },
        controllerAs: 'ctrl'
    };
}]);