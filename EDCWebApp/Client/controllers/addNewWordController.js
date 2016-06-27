'use strict';
/**
*@name AddNewWordController
*@description
*AddNewWordController is used for adding word from teacher
*/
angular.module('learnChineseApp.controller').controller('AddNewWordController', ['loginUserFactory',
    '$location','errorFactory',
    function (loginUserFactory, $location, errorFactory) {
   
    var vm = this;
    vm.word = {};
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

        if (vm.word.slangs) {
            addObj['slangs'] = vm.word.slangs.map(function (obj) {
                return obj.slang;
            }).filter(function (p) { return p !== undefined });
        }


        loginUserFactory.addNewWord(addObj).$promise.then(function (data) {
            vm.error = null;
            $location.path('/addNewWord');
        }, function (error) {
            errorFactory.setErrorFromException(error);
            vm.error = angular.copy(errorFactory.getErrorMsg());
            vm.error.persistent = true;
        });
    }
}]);