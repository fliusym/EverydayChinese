angular.module('learnChineseApp.directive')
.directive('edcAudio', ['$sce', function ($sce) {
    'use strict';

    return {
        restrict: 'E',
        templateUrl: '/Client/views/directiveViews/audio.html',
        scope: {
            audioid: '@'
        },
        link: function (scope, element, attrs) {
            scope.onAudio = function () {
                var audio = (element[0].querySelector('audio'));
                if (audio) {
                    audio.play();
                }

            }
        }
    };

}]);