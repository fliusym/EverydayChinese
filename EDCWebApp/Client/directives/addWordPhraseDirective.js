/**
*@name edcAddWordPhrase
*@description 
* this directive is used for adding word phrase, it should use edcAddWord directive as parent
*examples
* see addNewWord.html
*/
angular.module('learnChineseApp.directive')
.directive('edcAddWordPhrase', [function () {
    'use strict';

    return {
        restrict: 'E',
        require: '^^edcAddWord',
        transclude: true,
        templateUrl: '/Client/views/directiveViews/addWordPhrase.html',
        scope: {
        },
        controller: ['$scope', function ($scope) {

            $scope.phrase = {};
            $scope.index = 0;
            var examples = $scope.phrase['examples'] = [];
            this.addExample = function (example) {
                examples.push(example);
            }
        }],
        link: function (scope, element, attrs, wordCtrl) {
            wordCtrl.addPhrase(scope);
        }
    };

}]);