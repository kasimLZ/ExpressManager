Test = function () {

    var val = 111;

    return {
        get: function () { console.log(val); },
        set: function (value) { val = value;  }
    };
}()

Temp = 1123;