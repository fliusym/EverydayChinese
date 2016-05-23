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
        },
        setErrorFromException: function (exception) {
            if (exception && exception.data && exception.data.ModelState) {

                errorMsgInfo.message = exception.data.ModelState.Message[0];
                errorMsgInfo.type = exception.data.ModelState.ErrorType[0] === 'Error' ? msgType.error : msgType.warning;
                errorMsgInfo.debugOnly = false;

            }
        }
    };
}]);