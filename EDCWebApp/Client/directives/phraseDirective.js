/**
*@name edcPhrase
*@description
* used to word phrases
*@param {expression} phraseList  list of phrases to be shown, for each phrase
*                       chinese - string, phrase's chinese
*                       english - string, phrase's english
*                       audioid - string, phrase's audio source path
*                       examples - array, for each example
*                               chinese - string, example's chinese
*                               english - string, example's english
*/
angular.module('learnChineseApp.directive')
.directive('edcPhrase', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/phrase.html',
        scope: {},

        bindToController: {
            phraseList: '=',
        },
        controller: function () { },
        controllerAs: 'ctrl'
    };
}]);