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
        //convert: function (timeString) {
        //    var time = new Date();
        //    var parts = timeString.match(/(\d+):(\d+) (am|pm)/);
        //    if (parts) {
        //        var hours = parseInt(parts[1]),
        //            minutes = parseInt(parts[2]),
        //            tt = parts[3];
        //        if (tt === 'pm' && hours < 12) hours += 12;
        //        time.setHours(hours, minutes, 0, 0);
        //        return time;
        //    }
        //},
        //addHours: function (date, h) {
        //    date.setHours(date.getHours() + h);
        //},
        //formatDateAMPM: function (date) {
        //    if (date) {
        //        var hours = date.getHours(),
        //            minutes = date.getMinutes();
        //        var ampm = hours >= 12 ? 'pm' : 'am';
        //        hours = hours % 12;
        //        hours = hours ? hours : 12;
        //        minutes = minutes < 10 ? '0' + minutes : minutes;
        //        var strTime = hours + ':' + minutes + ' ' + ampm;
        //        return strTime;
        //    }
        //},
        getStartTime: function (str) {
            if (str) {
                var splits = str.split('-');
                return splits[0].slice(0, -1);
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