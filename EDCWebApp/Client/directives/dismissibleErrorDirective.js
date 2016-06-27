/**
*@name edcDismissibleError
*@description
* show error message 
*@param {expression} error The error object should be shown including
*                          message - string 
*                           type - 'error' or 'warning'
*                           persistent - if true, this error message can't be dismissed, otherwise it can
*/
angular.module('learnChineseApp.directive')
.directive('edcDismissibleError', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/dismissibleError.html',

        scope: {},

        bindToController: {
            error: '='
        },
        controller: function () { },
        controllerAs: 'ctrl'
    };
}]);