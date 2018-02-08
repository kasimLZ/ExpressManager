$.extend({
    /*1.9以后判断浏览器的兼容解决方案*/
    browser: {
        userAgent: navigator.userAgent.toLowerCase(),
        mozilla: /firefox/.test(navigator.userAgent.toLowerCase()),
        webkit: /webkit/.test(navigator.userAgent.toLowerCase()),
        opera: /opera/.test(navigator.userAgent.toLowerCase()),
        msie: (/msie/.test(navigator.userAgent.toLowerCase()) || (!!window.ActiveXObject || "ActiveXObject" in window))
    },
    /*判断对象是否为数组*/
    isArray: function (object) {
        if (object == null) return false;
        return object.constructor == Array;
    },
    /*判断对象是否为字符串*/
    isString: function (object) {
        return (typeof object == 'string') && object.constructor == String;
    },
    /*判断对象是否为对象*/
    isObject: function (object) {
        return (typeof object == 'object') && object.constructor == Object;
    },
    /*判断对象是否为数字*/
    isNumber: function (object) {
        return !isNaN(object);
    },
    /*判断对象是否为方法*/
    isFunction: function (object){
        return (typeof object == 'function')
    },
    /*获取时间戳*/
    timeStamp: function (seed) {
        return (seed != null ? new Date(seed) : new Date()).valueOf();
    },
    /*将Json转化为表单*///<--当初为啥要做这个？
    jsonToForm: function (url, json, method, target) {
        var _target_type_ = ["blank", "parent", "top", "self"];
        var _method_type_ = ["post", "get"];
        if (_method_type_.indexOf(method.toLowerCase()) < 0) method = _method_type_[0];
        if (_target_type_.indexOf(target.toLowerCase()) < 0) target = _target_type_[0];

        var form = $("<form></form>")
        form.attr("target", "_" + target).attr("method", method).attr("action", url);

    },
    /*全角转半角*/
    ToDBC: function (txtstring) {
        var tmp = "";
        for (var i = 0; i < txtstring.length; i++) {
            if (txtstring.charCodeAt(i) == 32) {
                tmp = tmp + String.fromCharCode(12288);
            }
            if (txtstring.charCodeAt(i) < 127) {
                tmp = tmp + String.fromCharCode(txtstring.charCodeAt(i) + 65248);
            }
        }
        return tmp;
    },
    /*半角转全角*/
    ToCDB: function (str) {
        var tmp = "";
        for (var i = 0; i < str.length; i++) {
            if (str.charCodeAt(i) > 65248 && str.charCodeAt(i) < 65375) {
                tmp += String.fromCharCode(str.charCodeAt(i) - 65248);
            }
            else {
                tmp += String.fromCharCode(str.charCodeAt(i));
            }
        }
        return tmp
    },
    /*遮罩扩展，需要引用loader.css*/
    Shadow: function(){
        var id = null;
        var count = 0;
        var icon = "circle";

        return {
            style: function (set) {
                icon = set;
                console.log(icon);
                if (id != null) $("#" + id + " div[data-loader]").attr("data-loader","circle-side")
            },
            show: function () {
                if (count == 0) {
                    id = "shadow-" + Math.floor(Math.random() * 10000);
                    var shadow = $('<div class="loading_bg" id="' + id + '"><div class="loading_box"><div data-loader="' + icon+'"></div></div></div>');
                    $("body").append(shadow);
                }
                count++;
            },
            hide: function () {
                if (count > 0) {
                    count--;
                }
                if (count == 0) {
                    $("#" + id).remove();
                    id = null;
                }
            }
        }
    }(),
})
$.fn.extend({
    datas: function (object) {
        var data = new Object();
        for (i in this[0].attributes) {
            if ($.isNumber(i)) {
                var attr = this[0].attributes[i];
                if (!$.isFunction(attr.value) && attr.name.indexOf("data-") == 0) {
                    data[attr.name.replace("data-", "")] = attr.value;
                }
            }
        }
        return data;
    }
})

if (window.navigator.userAgent.indexOf("MSIE") >= 1 || !!window.ActiveXObject || "ActiveXObject" in window) {
    if (JSON.parse == undefined) JSON.parse = function (str) { return eval("(" + str + ")"); }
};

Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

Date.prototype.getLocalFormat = function() {
    var str, colorhead, colorfoot;
    var yy = this.getYear();
    if (yy < 1900) yy = yy + 1900;
    var MM = this.getMonth() + 1;
    if (MM < 10) MM = '0' + MM;
    var dd = this.getDate();
    if (dd < 10) dd = '0' + dd;
    var hh = this.getHours();
    if (hh < 10) hh = '0' + hh;
    var mm = this.getMinutes();
    if (mm < 10) mm = '0' + mm;
    var ss = this.getSeconds();
    if (ss < 10) ss = '0' + ss;
    var ww = this.getDay();
    if (ww == 0) colorhead = "<font color=\"#FF0000\">";
    if (ww > 0 && ww < 6) colorhead = "<font color=\"#373737\">";
    if (ww == 6) colorhead = "<font color=\"#008000\">";
    if (ww == 0) ww = "星期日";
    if (ww == 1) ww = "星期一";
    if (ww == 2) ww = "星期二";
    if (ww == 3) ww = "星期三";
    if (ww == 4) ww = "星期四";
    if (ww == 5) ww = "星期五";
    if (ww == 6) ww = "星期六";
    colorfoot = "</font>"
    str = colorhead + yy + "年" + MM + "月" + dd + "日" + hh + ":" + mm + ":" + ss + " " + ww + colorfoot;
    return (str);
}

String.prototype.replaceAll = function (exp, newStr) {
    return this.replace(new RegExp(exp, "gm"), newStr);
};

String.prototype.format = function (args) {
    var result = this;
    if (arguments.length < 1) {
        return result;
    }

    var data = arguments; // 如果模板参数是数组
    if (arguments.length == 1 && typeof (args) == "object") {
        // 如果模板参数是对象
        data = args;
    }
    for (var key in data) {
        var value = data[key];
        if (undefined != value) {
            result = result.replaceAll("\\{" + key + "\\}", value);
        }
    }
    return result;
}

String.prototype.endWith = function (s) {
    if (s == null || s == "" || this.length == 0 || s.length > this.length)
        return false;
    if (this.substring(this.length - s.length) == s)
        return true;
    else
        return false;
    return true;
}
String.prototype.startWith = function (s) {
    if (s == null || s == "" || this.length == 0 || s.length > this.length)
        return false;
    if (this.substr(0, s.length) == s)
        return true;
    else
        return false;
    return true;
}