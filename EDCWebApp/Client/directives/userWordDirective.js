angular.module('learnChineseApp.directive')
.directive('edcWordItem', [function () {
    'use strict';
    return {
        require: '^edcItemContainer',
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/userWordItem.html',
        scope: {},

        bindToController: {
            item: '=',
            index: '=',
            remove: '&'
        },
        controller: function () {
            var ctrl = this;
            ctrl.removeItem = function () {
                this.remove()(this.index);
            }
        },
        controllerAs: 'ctrl'
    };
}]);