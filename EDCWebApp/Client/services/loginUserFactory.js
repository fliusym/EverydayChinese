angular.module('learnChineseApp.service')
.factory('loginUserFactory', ['$resource',
    'tokenKey',
    'transformHeaderFactory',
    '$q',
    '$location',
    'authenticationFactory',
    function ($resource, tokenKey, TransformHeaderService, $q, $location, authenticationFactory) {
        'use strict';
        var factory = {};
        var cachedStudentLearnRequests;
        factory.getUserResources = function (id) {
            var token = sessionStorage.getItem(tokenKey);
            //since the username is email address, has to add trailing slash
            var resource = $resource('/api/Students', null, {
                getUserResource: {
                    method: 'GET',
                    headers: { 'Authorization': ('Bearer ' + token) },
                    transformRequest: function (data, headerGetter) {
                        return TransformHeaderService.transform(data, headerGetter);
                    }
                }
            });

            return resource.getUserResource({ id: id });

        };

        factory.getTeacherResource = function (id) {
            var token = sessionStorage.getItem(tokenKey);
            var resource = $resource('/api/Teachers', null, {
                getUserResource: {
                    method: 'GET',
                    isArray: false,
                    headers: { 'Authorization': ('Bearer ' + token) },
                    transformRequest: function (data, headerGetter) {
                        return TransformHeaderService.transform(data, headerGetter);
                    }
                }
            });

            return resource.getUserResource({ id: id });
        }

        factory.removeLearnRequest = function (id) {
            var token = sessionStorage.getItem(tokenKey);
            var resource = $resource('/api/LearnRequests/:id', null, {
                removeItem: {
                    method: 'DELETE',
                    headers: {
                        'Authorization': ('Bearer ' + token),
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }
            });
            return resource.removeItem({ id: id });
        }

        factory.editLearnRequest = function (id,date,time) {
            var token = sessionStorage.getItem(tokenKey);
            var resource = $resource('/api/LearnRequests/:id', null, {
                editItem: {
                    method: 'PUT',
                    headers: {
                        'Authorization': ('Bearer ' + token),
                        // 'Content-Type': 'application/json'
                        //'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }
            });
            return resource.editItem({ id: id}, { date: date, time: time } );
        }

        factory.addLearnRequest = function (requestData) {
            var token = sessionStorage.getItem(tokenKey);
            //since the username is email address, has to add trailing slash
            var resource = $resource('/api/LearnRequests', null, {
                postLearnRequest: {
                    method: 'POST',
                    headers: {
                        'Authorization': ('Bearer ' + token),
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                }
            });
            var data = {
                'LearnRequests': requestData.data
            };
            return resource.postLearnRequest(data);
        }

        factory.addWord = function (date) {
            var token = sessionStorage.getItem(tokenKey);
            var resource = $resource('/api/Student/Words', null, {
                addWord: {
                    method: 'PUT',
                    headers: {
                        'Authorization': ('Bearer ' + token),
                        'Content-Type': 'application/json'
                        // 'Content-Type': 'application/x-www-form-urlencoded',

                    }
                }
            });
            resource.addWord(date).$promise.then(function () {
                var mydata = authenticationFactory.getLoginInfo();
                $location.path('/user').search({ user: mydata.user });
            }, function () {
                $location.path('/default');
           //     if (!$rootScope.$$phase) $rootScope.$apply();
            });
        }

        factory.removeWord = function (id) {
            var token = sessionStorage.getItem(tokenKey);
            var resource = $resource('/api/Words/:id', null, {
                deleteWord: {
                    method: 'DELETE',
                    headers: {
                        'Authorization': ('Bearer ' + token),
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    transformRequest: function (data, headerGetter) {
                        return TransformHeaderService.transform(data, headerGetter);
                    }
                }
            });
            return resource.deleteWord({ id: id });
        }
        factory.setCachedStudentLearnRequests = function (learnRequests) {
            cachedStudentLearnRequests = learnRequests;
        }
        factory.getCachedStudentLearnRequests = function () {
            return cachedStudentLearnRequests;
        }
        return factory;
    }]);