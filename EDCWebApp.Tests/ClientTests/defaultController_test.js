


describe('DefaultController test', function () {
    beforeEach(module('learnChineseApp.service'));
    beforeEach(module('learnChineseApp.controller'));
    var mockWords = function () {
        var today = new Date();
        var tomorrow = new Date();
        tomorrow.setTime(today.getTime() + 1 * 86400000);
        var todayString = (today.getMonth() + 1) + '/' + today.getDate() + '/' + today.getFullYear();
        var tomorrowString = (tomorrow.getMonth() + 1) + '/' + tomorrow.getDate() + '/' + tomorrow.getFullYear();
        return [
            {
                Id: 1,
                Date: todayString,
                Character: 'tie',
                BasicMeanings: 'iron',
                Pinyin: 'tie.png',
                Audio: 'tie.m4a',
                Phrases: [
                    {
                        Chinese: 'tie ren',
                        English: 'iron man',
                        Pinyin: 'tieren.png',
                        Examples: [
                            {
                                Chinese: 'tie ren fei chang bang',
                                English: 'iron man is very good'
                            }
                        ]
                    },
                    {
                        Chinese: 'gang tie',
                        English: 'iron',
                        Pinyin: 'gangtie.png',
                        examples: [
                            {
                                Chinese: 'gang tie shi zen yang',
                                English: 'how to form an iron'
                            }
                        ]
                    }
                ],
                Slangs: [
                    {
                        SlangEnglish: 'tie da de',
                        SlangChinese: 'tie da de',
                        SlangExampleEnglish: 'tie da de hen hao',
                        SlangExampleChinese: 'tie da de very good'
                    }
                ]
            },
            {
                Id: 2,
                Date: tomorrowString,
                Character: 'zhe',
                BasicMeanings: 'this',
                Pinyin: 'zhe.png',
                Audio: 'zhe.m4a',
                Phrases: [
                    {
                        Chinese: 'zhe ren',
                        English: 'this man',
                        Pinyin: 'zheren.png',
                        Examples: [
                            {
                                Chinese: 'zhe ren fei change hao',
                                English: 'this man is very good'
                            },
                            {
                                Chinese: 'zhe ren fei change huai',
                                English: 'this man is very bad'
                            }
                        ]
                    }
                ],
                Slangs: [
                    {
                        SlangEnglish: 'zhe ge',
                        SlangChinese: 'zhe ge',
                        SlangExampleEnglish: 'zhe ge fei change bang',
                        SlangExampleChinese: 'this thing is very good'
                    },
                    {
                        SlangEnglish: 'zhe zhong',
                        SlangChinese: 'this kind',
                        SlangExampleEnglish: 'this kind of plant is very popular',
                        SlangExampleChinese: 'zhe zhong zhi wu fei change shou huan ying'
                    }
                ]
            }
        ];
    };
    var mockScenes = function () {
        var today = new Date();
        var tomorrow = new Date();
        tomorrow.setTime(today.getTime() + 1 * 86400000);
        var todayString = (today.getMonth() + 1) + '/' + today.getDate() + '/' + today.getFullYear();
        var tomorrowString = (tomorrow.getMonth() + 1) + '/' + tomorrow.getDate() + '/' + tomorrow.getFullYear();
        return [
            {
                Id: 1,
                Date: todayString,
                ThemeChinese: 'wen hou',
                ThemeEnglish: 'Greetings',
                Images: [
                    {
                        Image: 'mygreeting_1.png',
                        Words: [
                            {
                                Word: 'zao shang hao',
                                Pinyin: 'zaoshanghao.png',
                                Audio: 'zaoshanghao.m4a'
                            }
                        ]
                    },
                    {
                        Image: 'mygreeting_2.png',
                        Words: [
                            {
                                Word: 'xia wu hao',
                                Pinyin: 'xiawuhao.png',
                                Audio: 'xiawuhao.m4a'
                            }
                        ]
                    }
                ]
            },
            {
                Id: 2,
                Date: tomorrowString,
                ThemeChinese: 'ni hao ma',
                ThemeEnglish: 'how are you',
                Images: [
                    {
                        Image: 'howareyou_1.png',
                        Words: [
                            {
                                Word: 'zui jin',
                                Pinyin: 'zuijin.png',
                                Audio: 'zuijin.m4a'
                            }
                        ]
                    },
                    {
                        Image: 'howareyou_2.png',
                        Words: [
                            {
                                Word: 'zen yang',
                                Pinyin: 'zenyang.png',
                                Audio: 'zenyang.m4a'
                            }
                        ]
                    }
                ]
            }
        ]
    };
    var $httpBackend, $controller;
    beforeEach(inject(function (_$controller_) {
        $controller = _$controller_;
    }));
    beforeEach(inject(function ($injector) {
        $httpBackend = $injector.get('$httpBackend');
    }));
    //afterEach(function () {
    //    $httpBackend.verifyNoOutstandingExpectation();
    //    $httpBackend.verifyNoOutstandingRequest();
    //});
    it("should default controller defined", function () {
        var scope = {};
        var vm = $controller('DefaultController', { $scope: scope });
        expect(vm).toBeDefined();
    });
    it("set the current date to today by default", function () {
        var scope = {};
        var vm = $controller('DefaultController', { $scope: scope });
        var today = new Date();
        var curDate = vm.currentDate;
        expect(curDate.getDate()).toEqual(today.getDate());
        expect(curDate.getMonth()).toEqual(today.getMonth());
        expect(curDate.getFullYear()).toEqual(today.getFullYear());
    });

    it('when get the word from one date', function () {
        $httpBackend.whenRoute('GET', '/api/Words')
            .respond(function (method, url, data, headers, params) {

                var words = mockWords();
                var paramDate = new Date(params.date);
                var dateString = (paramDate.getMonth() + 1) + '/' + paramDate.getDate() + '/' + paramDate.getFullYear();
                var filtered = words.filter(function (element, index, array) {
                    return (element.Date === dateString);
                });
                return [
                    200,
                    filtered ? filtered[0] : null
                ]
            });
        $httpBackend.whenRoute('GET', '/api/Scenes')
        .respond(function (method, url, data, headers, params) {
            var scenes = mockScenes();
            var paramDate = new Date(params.date);
            var dateString = (paramDate.getMonth() + 1) + '/' + paramDate.getDate() + '/' + paramDate.getFullYear();
            var filtered = scenes.filter(function (element, index, array) {
                return (element.Date === dateString);
            });
            return [
                200,
                filtered ? filtered[0] : null
            ]
        });
        var scope = {};
        var vm = $controller('DefaultController', { $scope: scope });
        $httpBackend.flush();
        expect(vm.pinyin).toEqual('tie.png');
        expect(vm.audioSrc).toEqual('tie.m4a');
        expect(vm.phraselist.length).toEqual(2);
        expect(vm.slang.slangs.length).toEqual(1);
        expect(vm.slang.slangs[0].english).toEqual("tie da de");
        expect(vm.scenario.scenarioquotes[0].chinese).toEqual('wen hou');
    });

    it('should get correct word when change to another valid date', function () {
        var scope = {};
        var vm = $controller('DefaultController', { $scope: scope });
        $httpBackend.whenRoute('GET', '/api/Words')
        .respond(function (method, url, data, headers, params) {

            var words = mockWords();
            var paramDate = new Date(params.date);
            var dateString = (paramDate.getMonth() + 1) + '/' + paramDate.getDate() + '/' + paramDate.getFullYear();
            var filtered = words.filter(function (element, index, array) {
                return (element.Date === dateString);
            });
            return [
                200,
                filtered ? filtered[0] : null
            ]
        });
        $httpBackend.whenRoute('GET', '/api/Scenes')
        .respond(function (method, url, data, headers, params) {
            var scenes = mockScenes();
            var paramDate = new Date(params.date);
            var dateString = (paramDate.getMonth() + 1) + '/' + paramDate.getDate() + '/' + paramDate.getFullYear();
            var filtered = scenes.filter(function (element, index, array) {
                return (element.Date === dateString);
            });
            return [
                200,
                filtered ? filtered[0] : null
            ]
        });
        var today = new Date();
        var tomorrow = new Date();
        tomorrow.setTime(today.getTime() + 1 * 86400000);
        var tomorrowString = (tomorrow.getMonth() + 1) + '/' + tomorrow.getDate() + '/' + tomorrow.getFullYear();
        vm.datechange(tomorrowString);
        $httpBackend.flush();
        expect(vm.pinyin).toEqual('zhe.png');
        expect(vm.scenario.scenarioquotes[0].chinese).toEqual('ni hao ma');
    });

    it('should get error when change to another invalid date', function () {
        var scope = {};
        var vm = $controller('DefaultController', { $scope: scope });
        $httpBackend.whenRoute('GET', '/api/Words')
        .respond(function (method, url, data, headers, params) {

            var words = mockWords();
            var paramDate = new Date(params.date);
            var dateString = (paramDate.getMonth() + 1) + '/' + paramDate.getDate() + '/' + paramDate.getFullYear();
            var filtered = words.filter(function (element, index, array) {
                return (element.Date === dateString);
            });

            return [
            200,
            filtered? filtered[0] : null
            ]
           

        });
        $httpBackend.whenRoute('GET', '/api/Scenes')
        .respond(function (method, url, data, headers, params) {
            var scenes = mockScenes();
            var paramDate = new Date(params.date);
            var dateString = (paramDate.getMonth() + 1) + '/' + paramDate.getDate() + '/' + paramDate.getFullYear();
            var filtered = scenes.filter(function (element, index, array) {
                return (element.Date === dateString);
            });
            return [
                404
            ]
        });
        var today = new Date();
        var tomorrow = new Date();
        tomorrow.setTime(today.getTime() + 3 * 86400000);
        var tomorrowString = (tomorrow.getMonth() + 1) + '/' + tomorrow.getDate() + '/' + tomorrow.getFullYear();
        vm.datechange(tomorrowString);
        $httpBackend.flush();
        expect(vm.pinyin).toBeUndefined();
        expect(vm.scenario).toBeUndefined();
    });
});