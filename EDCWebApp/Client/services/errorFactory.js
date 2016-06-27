angular.module('learnChineseApp.service')
.factory('errorFactory', [function () {
    'use strict';
    var msgType = {
        error: 'error',
        warning: 'warning'
    };
    var errorMsgInfo = {
        message: 'There are some errors.',
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
                if (exception.data.ModelState.Message && exception.data.ModelState.Message.length > 0) {
                    errorMsgInfo.message = exception.data.ModelState.Message[0];
                } else {
                    errorMsgInfo.message = "There is something wrong.";
                }
                if (exception.data.ModelState.ErrorType && exception.data.ModelState.ErrorType.length > 0) {
                    errorMsgInfo.type = exception.data.ModelState.ErrorType[0] === 'Error' ? msgType.error : msgType.warning;
                }
                else {
                    errorMsgInfo.type = msgType.error;
                }
                errorMsgInfo.debugOnly = false;

            }
        }
    };
}]);