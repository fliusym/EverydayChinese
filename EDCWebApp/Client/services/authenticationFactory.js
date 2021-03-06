﻿angular.module('learnChineseApp.service')
.factory('authenticationFactory', ['$resource',
    '$rootScope',
    '$location',
    'broadcastFactory',
    'transformHeaderFactory',
    'tokenKey',
    'userInfo',
    'errorFactory',
function ($resource, $rootScope, $location, broadcastFactory, transformHeaderFactory, tokenKey, userInfo, errorFactory) {
    'use strict';
    var factory = {};

    var redirectUrlAfterLogin = '';
    //register
    factory.register = function (account) {
        var resource = $resource('/api/Account/Register', null, null);
        return resource.save({
            Email: account.email,
            Password: account.password,
            ConfirmPassword: account.confirmPassword
        });
    };


    //log in
    factory.login = function (mydata) {
        //get token
        var token = $resource('/Token', null, {
            generateToken: {
                method: 'POST',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                transformRequest: function (data, headerGetter) {
                    return transformHeaderFactory.transform(data, headerGetter);
                }

            }
        });
        token.generateToken({
            grant_type: 'password',
            username: mydata.email,
            password: mydata.password
        }).$promise.then(function (tokenBack) {
            sessionStorage.setItem(tokenKey, tokenBack.access_token);
            var isTeacher = false;
            factory.getAccountInfo().$promise.then(function (data) {
                isTeacher = data.IsTeacher;
                if (isTeacher) {
                    $location.path('/teacher').search({ user: mydata.email });
                } else {
                    if (redirectUrlAfterLogin) {
                        $location.path(redirectUrlAfterLogin);
                    } else {
                        $location.path('/user').search({ user: mydata.email });
                    }
                }
                var savedUser = {
                    flag: true,
                    user: mydata.email,
                    isTeacher: isTeacher
                };
                sessionStorage.setItem(userInfo, JSON.stringify(savedUser));
            });

            broadcastFactory.send('loginSuccess', {
                username: mydata.email
            });
        }, function (reason) {
            var user = {
                flag: false,
                user: mydata.email,
                reason: reason,
                isTeacher: false
            };
            errorFactory.setErrorFromException(reason);
            sessionStorage.setItem(userInfo, JSON.stringify(user));
            broadcastFactory.send('loginFail', reason);
        });
    };

    //log out
    factory.logout = function () {
        sessionStorage.removeItem(userInfo);
        sessionStorage.removeItem(tokenKey);
        redirectUrlAfterLogin = '';
        broadcastFactory.send('logout');
        $location.path('/');
    };

    //forgot password
    factory.forgotPassword = function (email) {
        var resource = $resource('/api/Account/ForgotPassword', null, null);
        resource.save({ UserEmail: email }).$promise.then(function (data) {
            $location.path('/forgot_password_confirmed');
        }, function (error) {
            //need to work later for error
            errorFactory.setErrorFromException(error);
        });
    };
    //reset password
    factory.resetPassword = function (userEmail, code, password, confirmPassword) {
        var resource = $resource('/api/Account/ResetPassword', null, null);
        resource.save({
            Email: userEmail,
            NewPassword: password,
            ConfirmPassword: confirmPassword,
            Code: code
        }).$promise.then(function () {
            $location.path('/login').search({ userEmail: null, code: null });
        }, function (error) {
            errorFactory.setErrorFromException(error);
            //need to work later for error
        });
    }
    factory.getLoginInfo = function () {
        var user = sessionStorage.getItem(userInfo);
        if (user) {
            return JSON.parse(sessionStorage.getItem(userInfo));
        }

    };
    factory.getAccountInfo = function () {
        var token = sessionStorage.getItem(tokenKey);
        var resource = $resource('/api/Account/AccountInfo', null, {
            getUserResource: {
                method: 'GET',
                headers: { 'Authorization': ('Bearer ' + token) },
                transformRequest: function (data, headerGetter) {
                    return transformHeaderFactory.transform(data, headerGetter);
                }
            }
        });
        return resource.getUserResource();
    };

    factory.setRedirectUrlAfterLogin = function (url) {
        redirectUrlAfterLogin = url;
    };

    return factory;
}]);