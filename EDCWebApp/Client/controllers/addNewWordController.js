angular.module('learnChineseApp.controller').controller('AddNewWordController', ['loginUserFactory',
    '$location','errorFactory',
    function (loginUserFactory, $location, errorFactory) {
    'use strict';
    var vm = this;
    vm.word = {
        'svgPath': '',
        'pinyinPath': '',
        'audioPath': '',
        'basicMeanings': '',
        'date': '',
        'explanation': ''

    };
    vm.onAdd = function () {

        var addObj = {};
        addObj['character'] = vm.word.svgPath;
        addObj['pinyin'] = vm.word.pinyinPath;
        addObj['audio'] = vm.word.audioPath;
        addObj['basicMeanings'] = vm.word.basicMeanings;
        addObj['explanation'] = vm.word.explanation;
        addObj['date'] = vm.word.date;
        if (vm.word.phrases) {
            addObj['phrases'] = vm.word.phrases.map(function (obj) {
                if (obj.phrase.examples) {
                    obj.phrase.examples = obj.phrase.examples.map(function (value) {
                        return value.example;
                    }).filter(function (f) { return f !== undefined });
                }
                return obj.phrase;
            });
        }

        if (vm.word.quotes) {
            addObj['quotes'] = vm.word.quotes.map(function (obj) {
                return obj.quote;
            }).filter(function (p) { return p !== undefined });
        }


        loginUserFactory.addNewWord(addObj).$promise.then(function (data) {
            $location.path('/addNewWord');
        }, function (error) {
            errorFactory.setErrorFromException(error);
            vm.error = errorFactory.getErrorMsg();
        });
    }
}]);