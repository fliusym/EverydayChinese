angular.module('learnChineseApp.directive')
.directive('edcPwdCompare', [function () {
    'use strict';
    return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            otherPwd: '=clPwdCompare'
        },
        link: function (scope, element, attrs, ngModel) {
            ngModel.$validators.compareTo = function (value) {
                return value == scope.otherPwd;
            };
            scope.$watch('otherPwd', function () {
                ngModel.$validate();
            });
        }
    };
}]);