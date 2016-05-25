angular.module('learnChineseApp.service')
.factory('broadcastFactory', ['$rootScope', function ($rootScope) {
    'use strict';
    return {
        send: function (msg, data) {
            var d = data || {};
            $rootScope.$broadcast(msg, d);
        }
    }
}]).factory('transformHeaderFactory', [function () {
    'use strict';
    return {
        transform: function (data, headerGetter) {
            var str = [];
            for (var d in data) {
                str.push(encodeURIComponent(d) + "=" + encodeURIComponent(data[d]));
            }
            return str.join("&");
        }
    }
}]).factory('timeFactory', [function () {
    'use strict';
    var curDate;
    return {
        getStartTime: function (str) {
            if (str) {
                var splits = str.split('-');
                return splits[0].slice(0, -1);
            }
        },
        getEndTime: function (str) {
            if (str) {
                var splits = str.split('-');
                return splits[1].trim();
            }
        },
        setCurrentDate: function (date) {
            curDate = date;
        },
        getCurrentDate: function () {
            return curDate;
        }
    }
}]);