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
            var resource = $resource('/api/Students/LearnRequests/:id', null, {
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
        //compared with removeLearnRquest, this function will remove 
        //the learn request from backend, while the function removeLearnRequest 
        //most of time only remove the student from learn request's registered students 
        //it can only be called from teacher
        factory.deleteLearnRequest = function (id) {
            var token = sessionStorage.getItem(tokenKey);
            var resource = $resource('/api/Teachers/LearnRequests/:id', null, {
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
            var resource = $resource('/api/Students/LearnRequests/:id', null, {
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

        factory.addLearnRequest = function (name,requestData) {
            var token = sessionStorage.getItem(tokenKey);
            //since the username is email address, has to add trailing slash
            var resource = $resource('/api/Students/:name/LearnRequests', null, {
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
            return resource.postLearnRequest({name:name},data);
        }

        factory.addWord = function (id) {
            var token = sessionStorage.getItem(tokenKey);
            var resource = $resource('/api/Students/Words', null, {
                addWord: {
                    method: 'PUT',
                    headers: {
                        'Authorization': ('Bearer ' + token),
                        'Content-Type': 'application/json'
                        // 'Content-Type': 'application/x-www-form-urlencoded',

                    }
                }
            });
            resource.addWord(id).$promise.then(function () {
                var mydata = authenticationFactory.getLoginInfo();
                $location.path('/user').search({ user: mydata.user });
            }, function () {
                $location.path('/default');
           //     if (!$rootScope.$$phase) $rootScope.$apply();
            });
        }

        factory.removeWord = function (name, id) {
            var token = sessionStorage.getItem(tokenKey);
            var resource = $resource('/api/Students/:name/Words/:id', null, {
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
            return resource.deleteWord({ name:name, id: id });
        }
        factory.setCachedStudentLearnRequests = function (learnRequests) {
            cachedStudentLearnRequests = learnRequests;
        }
        factory.getCachedStudentLearnRequests = function () {
            return cachedStudentLearnRequests;
        }
        return factory;
    }]);