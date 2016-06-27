/**
*@name edcAddWordSlang
*@description
* this directive is used for adding slang to word. use edcAddWord as parent
*/
angular.module('learnChineseApp.directive')
.directive('edcAddWordSlang', [function () {
    'use strict';

    return {
        restrict: 'E',
        require:'^^edcAddWord',
        templateUrl: '/Client/views/directiveViews/addWordSlang.html',
        scope: {
        },
        controller: ['$scope', function ($scope) {

        }],
        link: function (scope, element, attrs, wordCtrl) {
            wordCtrl.addSlang(scope);
        }
    };

}]);