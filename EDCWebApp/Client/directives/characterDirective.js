/**
*@name edcCharacter
*@description
* show the chinese character
*@param {expression} info information needs to be shown, including svgname of the character
*@param {expression} add  callback to add this character to user   
*/
angular.module('learnChineseApp.directive')
.directive('edcCharacter', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/character.html',
        scope: {

        },
        bindToController: {
            //svgname: '@',
            info: '=',
            add: '&'
        },
        controller: [function () {
            var ctrl = this;
            ctrl.canadd = false;
            ctrl.onAdd = function () {
                ctrl.canadd = false;
                this.add()(this.info.id);
            }
        }],
        controllerAs: 'ctrl',
    }
}]);