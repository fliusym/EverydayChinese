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

        factory.getUserResources = function (id) {
            var token = sessionStorage.getItem(tokenKey);
            //since the username is email address, has to add trailing slash
            var resource = $resource('/api/LCLoginStudents/:id', null, {
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
            var resource = $resource('/api/LCLoginTeachers/:id', null, {
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

        factory.removeLearnRequest = function (userName, date, startTime, duration) {
            var token = sessionStorage.getItem(tokenKey);
            var resource = $resource('/api/LCLearnRequest/Remove', null, {
                removeItem: {
                    method: 'DELETE',
                    headers: {
                        'Authorization': ('Bearer ' + token),
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }
            });
            return resource.removeItem({ StudentName: userName, Date: date, StartTime: startTime, Duration: duration });
        }

        factory.editLearnRequest = function (oldDate, date, time, oldTime) {
            var token = sessionStorage.getItem(tokenKey);
            var resource = $resource('/api/LCLearnRequest/Edit', null, {
                editItem: {
                    method: 'PUT',
                    headers: {
                        'Authorization': ('Bearer ' + token),
                        // 'Content-Type': 'application/json'
                        //'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }
            });
            return resource.editItem({ oldDate: oldDate, date: date, time: time, oldTime: oldTime });
        }

        factory.getUserAvailableTime = function (user) {
            this.getUserResources(user).$promise.then(function (data) {
                if (data) {
                    var learnRequests = data.LearnRequests;
                    var availables = AddLearnRequestService.getAvailableTime();
                    //get rid of those times the user has already taken
                    for (var i = 0; i < availables.length; i++) {
                        for (var j = 0; j < learnRequests.length; j++) {
                            if (availables[i].date === learnRequests[j].Date) {
                                //check if they have the same start to end time
                                if (isExisted(availables[i].morningChoose, learnRequests[j])) {
                                    availables[i].morningChoose = '';
                                }
                                if (isExisted(availables[i].afternoonChoose, learnRequests[j])) {
                                    availables[i].afternoonChoose = '';
                                }
                                if (isExisted(availables[i].eveningChoose, learnRequests[j])) {
                                    availables[i].eveningChoose = '';
                                }
                            }
                        }
                    }
                    return $q(function (resolve, reject) {
                        if (availables.length > 0) {
                            resolve(availables);
                        } else {
                            reject('no available learn request time');
                        }
                    });
                }
            });
        }

        factory.addWord = function (date) {
            var token = sessionStorage.getItem(tokenKey);
            var resource = $resource('/api/Default/Edit', null, {
                editWord: {
                    method: 'PUT',
                    headers: {
                        'Authorization': ('Bearer ' + token),
                        'Content-Type': 'application/json'
                        // 'Content-Type': 'application/x-www-form-urlencoded',

                    }
                }
            });
            resource.editWord(date).$promise.then(function () {
                var mydata = AuthenticationService.getLoginInfo();
                $location.path('/user').search({ user: mydata.user });
            }, function () {
                $location.path('/default');
                if (!$rootScope.$$phase) $rootScope.$apply();
            });
        }

        factory.removeWord = function (id) {
            var token = sessionStorage.getItem(tokenKey);
            var resource = $resource('/api/LCLoginStudents/Word/:id', null, {
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
        return factory;
    }]);