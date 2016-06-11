angular.module('learnChineseApp.directive')
.directive('edcAddWord', ['$compile', function ($compile) {
    'use strict';

    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/addWord.html',
        transclude: true,
        scope: {
            word: '='
        },
        controller: ['$scope',function($scope){
            var phrases = $scope.word['phrases'] = [];
            var quotes = $scope.word['quotes'] = [];
            $scope.phraseIndex = 1;
            this.addPhrase = function (phrase) {
                phrase.index = $scope.phraseIndex++;
                phrases.push(phrase);

            }
            this.addQuote = function (quote) {
                quotes.push(quote);
            }
        }],
        link: function (scope, element, attrs) {
            scope.onAddMorePhrase = function () {
                var el = $compile("<br/><div class='row'><edc-add-word-phrase></edc-add-word-phrase></div>")(scope);
                var finalBr = angular.element(document.querySelector('#finalBreak'));
                el.insertBefore(finalBr);
            }
        }
    };

}]);