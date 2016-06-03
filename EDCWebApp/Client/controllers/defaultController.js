angular.module('learnChineseApp.controller').controller('DefaultController',
    ['wordFactory', '$location', 'errorFactory','timeFactory',
        function (wordFactory, $location, errorFactory, timeFactory) {
            'use strict';
            var vm = this;
            var getCurrentDayWord = function (date) {
                wordFactory.getWord(date).$promise.then(function (word) {
                    vm.info = {};
                    vm.info['svgname'] = word.Character;
                    vm.info['id'] = word.Id;
                //    vm.svgname = word.Character;
                    vm.wordEnglish = word.BasicMeanings;
                    vm.pinyin = "/" + word.Pinyin;
                    vm.audioSrc = "/" + word.Audio;
                    vm.phraselist = [];
                    for (var i = 0; i < word.Phrases.length; i++) {
                        var examples = [];
                        if (word.Phrases[i].Examples) {
                            for (var j = 0; j < word.Phrases[i].Examples.length; j++) {
                                examples.push({
                                    chinese: word.Phrases[i].Examples[j].Chinese,
                                    english: word.Phrases[i].Examples[j].English
                                });
                            }
                        }

                        vm.phraselist.push({
                            chinese: word.Phrases[i].Chinese,
                            english: word.Phrases[i].English,
                            audioid: "/" + word.Phrases[i].Pinyin,
                            examples: examples

                        });
                    }
                }, function (error) {
                    errorFactory.setErrorFromException(error);
                    var test = errorFactory.getErrorMsg();
                    vm.error = test;
                });
            };
            vm.currentDate = new Date();
            
            getCurrentDayWord(vm.currentDate);

            vm.quotetitle = {
                chinese: '名人语录',
                english: 'Quotes'
            };
            vm.quotes = [
                {
                    chinese: '人民群众是一切知识的力量和源泉，中国人民解放军,人民群众是一切知识的力量和源泉，中国人民解放军',
                    hasfooter: true,
                    who: '毛泽东',
                    where: '为人民服务'
                },
                {
                    chinese: '人民群众是一切知识的力量和源泉，中国人民解放军',
                    hasfooter: true,
                    who: '毛泽东',
                    where: '为人民服务'
                }
            ];

            vm.scenariotitle = {
                chinese: '生活场景介绍',
                english: 'Everyday life scenario'
            };
            vm.scenarioquotes = [
                {
                    chinese: '如果你的家人或者朋友生病了，你想要表达一下你的关心',
                    hasfooter: true,
                    where: 'If your family or friend gets sick, you wants to offer your sympathy'
                }
            ];

            vm.imagelist = [
                {
                    src: 'Content/Scenario/scenariofirst.png'
                },
                {
                    src: 'Content/Scenario/scenariofirst.png'
                },
                {
                    src: 'Content/Scenario/scenariofirst.png'
                },

                {
                    src: 'Content/Scenario/scenariofirst.png'
                }
            ];

            vm.datechange = function (date) {
                vm.currentDate = date;
                getCurrentDayWord(date);
            }
            vm.add = function (id) {
                timeFactory.setCurrentDate(vm.currentDate);
                wordFactory.setCurrentWordIdToAdd(id);
                $location.path('/addWord');
            }
        }]);