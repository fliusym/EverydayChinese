angular.module('learnChineseApp.service')
.factory('errorFactory', [function () {
    'use strict';
    var msgType = {
        error: 'error',
        warning: 'warning'
    };
    var errorMsgInfo = {
        message: '',
        type: msgType.error,
        debugOnly: false
    };
    return {
        getErrorMsg: function () {
            return errorMsgInfo;
        },
        setErrorMsg: function (msg) {
            errorMsgInfo = msg;
        }
    };
}]);