angular.module('learnChineseApp.filter').filter('trusted', ['$sce', function ($sce) {
    'use strict';
    return function (url) {
        if (url) {
            return $sce.trustAsResourceUrl(url);
        }

    };
}]);