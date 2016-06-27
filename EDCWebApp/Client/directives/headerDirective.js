/**
*@name edcHeader
*@description
* header directive 
*@param {expression} loggedinInfo  log in information used to show different context based who is logged in
*                    flag - if true means some user logs in, otherwise no log in
*                    user - string, log in user's name
*                    isTeacher - if true means teacher logs in, otherwise student logs in
*
*/
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