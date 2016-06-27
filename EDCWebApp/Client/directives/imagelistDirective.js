/**
*@name edc-image-list
*@description
* used to show images inside scenario
*@param {expression} images  list of images to be shown, for each image
*                src - string, the source of the image
*                words - array, words inside the image, for each word
*                       chinese - string, word's chinese
*                       pinyin - string, word's pinyin image source path
*                       audiosrc - string, word's audio source path
*/
angular.module('learnChineseApp.directive')
.directive('edcImageList', [function () {
    'use strict';
    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/imagelist.html',
        scope: {},

        bindToController: {
            images: '='
        },
        controller: function () { },
        controllerAs: 'ctrl'
    };
}]);