angular.module('learnChineseApp.service')
.factory('learnRequestFactory',[function(){
    'use strict';
    var formateDate = function (date) {
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();
        return (m + 1) + '/' + d + '/' + y;
    }
    var filterLearnRequest = function (value, learnRequest) {
        if (value && learnRequest) {
            var startToEndTime = learnRequest.startTime + ' - ' + learnRequest.endTime;
            if (value.indexOf(startToEndTime) > -1) {
                value.splice(value.indexOf(startToEndTime),1);
            }
        }
    }
    var dateSimpleCompare = function (date1, date2) {
        var d1 = date1.getDate();
        var d2 = date2.getDate();

        var m1 = date1.getMonth();
        var m2 = date2.getMonth();

        var y1 = date1.getFullYear();
        var y2 = date2.getFullYear();

        if (d1 === d2 && m1 === m2 && y1 === y2) {
            return true;
        }
        return false;
    }
    return {
        filterUserLearnRequests: function (availables, learnRequests) {
            for (var i = 0; i < availables.length; i++) {
                for (var j = 0; j < learnRequests.length; j++) {
                    if (availables[i].date === learnRequests[j].date) {
                        filterLearnRequest(availables[i].morning, learnRequests[j]);
                        filterLearnRequest(availables[i].afternoon, learnRequests[j]);
                        filterLearnRequest(availables[i].evening, learnRequests[j]);
                    }
                }
            }
        },
        getAvailableTime: function () {
            var ret = [];
            //wed evening saturday morning and evening, sunday morning
            var now = new Date();
            var saved = new Date(now);
            var hour = now.getHours();
            var minute = now.getMinutes();
            for (var i = 0; i < 8; i++) {
                var d = now.getDay();
                if (d === 3) {
                    var canRequest = true;
                    if (dateSimpleCompare(saved, now)) {
                        if (hour > 13) {
                            canRequest = false;
                        }
                    }
                    if (canRequest) {
                        ret.push({
                            'date': formateDate(now),
                            'day': 'Wednesday',
                            'evening': [
                                '5:00 pm - 6:00 pm',
                                '7:00 pm - 8:00 pm'
                            ],
                            'morningChoose': '',
                            'afternoonChoose': '',
                            'eveningChoose': ''
                        });
                    }
                } else if (d === 6) {
                    var canRequest = true;
                    if (dateSimpleCompare(saved, now)) {
                        if (hour > 8) {
                            canRequest = false;
                        }
                    }
                    if (canRequest) {
                        ret.push({
                            'date': formateDate(now),
                            'day': 'Saturday',
                            'morning': [
                                '10:00 am - 11:00 am'
                            ],
                            'afternoon': [
                                '2:00 pm - 3:00 pm',
                                '3:00 pm - 4:00 pm'
                            ],
                            'morningChoose': '',
                            'afternoonChoose': '',
                            'eveningChoose': ''
                        });
                    }
                } else if (d === 0) {
                    var canRequest = true;
                    if (dateSimpleCompare(saved, now)) {
                        if (hour > 8) {
                            canRequest = false;
                        }
                    }
                    if (canRequest) {
                        ret.push({
                            'date': formateDate(now),
                            'day': 'Sunday',
                            'morning': [
                                '9:00 am - 10:00 am',
                                '10:00 am - 11:00 am'
                            ],
                            'morningChoose': '',
                            'afternoonChoose': '',
                            'eveningChoose': ''
                        });
                    }
                }
                now.setDate(now.getDate() + 1);
            }
            return ret;
        }
    };
}]);