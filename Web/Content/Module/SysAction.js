var SysAction = function () {

    var c = 1112233;

    return {
        Init: function () {
            console.log(c)
        },
        set: function (d) {
            c = d;
        }
    }
}();