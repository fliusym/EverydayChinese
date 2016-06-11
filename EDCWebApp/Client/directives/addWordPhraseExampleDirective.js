angular.module('learnChineseApp.directive')
.directive('edcAddWordPhraseExample', [function () {
    'use strict';

    return {
        restrict: 'E',
        require: '^^edcAddWordPhrase',
        templateUrl: '/Client/views/directiveViews/addWordPhraseExample.html',
        scope: {
        },
        controller: ['$scope', function ($scope) {

        }],
        link: function (scope, element, attrs, phraseCtrl) {
            phraseCtrl.addExample(scope);
        }
    };

}]);