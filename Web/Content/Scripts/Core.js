var Core = function () {

    const _Script_Module_Root_ = "/Content/Module/";
    const _J_Frame_Prefix_ = "JF";
    var _Script_Module_Stack_ = [];
    var _Select_ID_ = [];

    // Add body-small class if window less than 768px
    var BodySmallClass = function () {
        if ($(document).width() < 769) {
            $('body').addClass('body-small')
        } else {
            $('body').removeClass('body-small')
        }
    }

    //Collapse ibox function
    var CollapseIBox = function () {
        $('.collapse-link').click(function () {
            var ibox = $(this).closest('div.ibox');
            var button = $(this).find('i');
            var content = ibox.find('div.ibox-content');
            content.slideToggle(200);
            button.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
            ibox.toggleClass('').toggleClass('border-bottom');
            setTimeout(function () {
                ibox.resize();
                ibox.find('[id^=map-]').resize();
            }, 50);
        });
    }

    //Close ibox function
    var CloseIBox = function () {
        $('.close-link').click(function () {
            var content = $(this).closest('div.ibox');
            content.remove();
        });
    }

    // Minimalize menu
    var MinimalizeMenu = function () {
        $('.navbar-minimalize').click(function () {
            $("body").toggleClass("mini-navbar");
            SmoothlyMenu();

        });
    }

    // Full height of sidebar
    var fix_height = function () {
        var heightWithoutNavbar = $("body > #wrapper").height() - 61;
        $(".sidebard-panel").css("min-height", heightWithoutNavbar + "px");

        var navbarHeigh = $('nav.navbar-default').height();
        var wrapperHeigh = $('#page-wrapper').height();

        if (navbarHeigh > wrapperHeigh) {
            $('#page-wrapper').css("min-height", navbarHeigh + "px");
        }

        if (navbarHeigh < wrapperHeigh) {
            $('#page-wrapper').css("min-height", $(window).height() + "px");
        }

        if ($('body').hasClass('fixed-nav')) {
            if (navbarHeigh > wrapperHeigh) {
                $('#page-wrapper').css("min-height", navbarHeigh - 60 + "px");
            } else {
                $('#page-wrapper').css("min-height", $(window).height() - 60 + "px");
            }
        }

    }

    var SmoothlyMenu = function () {
        if (!$('body').hasClass('mini-navbar') || $('body').hasClass('body-small')) {
            // Hide menu in order to smoothly turn on when maximize menu
            $('#side-menu').hide();
            // For smoothly turn on menu
            setTimeout(
                function () {
                    $('#side-menu').fadeIn(400);
                }, 200);
        } else if ($('body').hasClass('fixed-sidebar')) {
            $('#side-menu').hide();
            setTimeout(
                function () {
                    $('#side-menu').fadeIn(400);
                }, 100);
        } else {
            // Remove all inline style from jquery fadeIn function to reset menu state
            $('#side-menu').removeAttr('style');
        }
    }

    // Dragable panels
    var WinMove = function () {
        var element = "[class*=col]";
        var handle = ".ibox-title";
        var connect = "[class*=col]";
        $(element).sortable(
            {
                handle: handle,
                connectWith: connect,
                tolerance: 'pointer',
                forcePlaceholderSize: true,
                opacity: 0.8
            })
            .disableSelection();
    }

    //Calculate the label width
    var TabsWidthCount = function (tabs) {
        var width = 0;
        $(tabs).each(function () { width += $(this).outerWidth(true) });
        return width;
    }

    //Tab bar scroll
    var TabsRoll = function () {
        var float_left = Math.abs(parseInt($(".page-tabs-content").css("margin-left")));
        var tabs_width = TabsWidthCount($(".content-tabs").children().not(".J_menuTabs"));
        var track_width = $(".content-tabs").outerWidth(true) - tabs_width;
        var p = 0;
        if ($(".page-tabs-content").width() < track_width) return false;
        var m = $(".J_menuTab:first");
        var n = 0;
        while ((n + $(m).outerWidth(true)) <= float_left) {
            n += $(m).outerWidth(true);
            m = $(m).next()
        }
        n = 0;

        if ($(this).hasClass("J_tabLeft")) {
            if (TabsWidthCount($(m).prevAll()) > track_width) {
                while ((n + $(m).outerWidth(true)) < (track_width) && m.length > 0) {
                    n += $(m).outerWidth(true);
                    m = $(m).prev()
                }
                p = TabsWidthCount($(m).prevAll())
            }
        } else {
            while ((n + $(m).outerWidth(true)) < (track_width) && m.length > 0) {
                n += $(m).outerWidth(true);
                m = $(m).next()
            }
            p = TabsWidthCount($(m).prevAll());
            if (p <= 0) return false;
        }
        $(".page-tabs-content").animate({ marginLeft: 0 - p + "px" }, "fast")
    }

    //Open the tab
    var JOpenPage = function () {

        var href = $(this).attr("href"),
            index = $(this).data("index"),
            name = $.trim($(this).text()),
            already_open = true;
        if (href == undefined || $.trim(href).length == 0) { return false }
        $(".J_menuTab").each(function () {
            if ($(this).data("id") == index) {
                if (!$(this).hasClass("active")) {
                    $(this).addClass("active").siblings(".J_menuTab").removeClass("active");
                    FloatMenuTab(this);
                    $("#J_mainContent .J_iframe").each(function () { if ($(this).data("id") == index) { $(this).show().siblings(".J_iframe").hide(); return false } })
                }
                already_open = false;
                return false;
            }
        });
        if (already_open) {
            var p = '<a href="javascript:;" class="active J_menuTab" data-id="' + index + '">' + name + ' <i class="fa fa-times-circle"></i></a>';
            $(".J_menuTab").removeClass("active");
            var frame = $('<div class="J_iframe" id="' + _J_Frame_Prefix_ + index + '" data-id="' + index + '"></div>');
            if ($(".J_iframe:visible").size())
                SuspendPageScript($(".J_iframe:visible").attr("id"))
            $("#J_mainContent").find("div.J_iframe").hide();
            $("#J_mainContent").append(frame);
            $(".J_menuTabs .page-tabs-content").append(p);
            frame.load(href);
            FloatMenuTab($(".J_menuTab.active"))
        }
        return false

    }

    //Float Menu Tab
    var FloatMenuTab = function (tag) {
        var bother = TabsWidthCount($(tag).prevAll()),
            next = TabsWidthCount($(tag).nextAll());
        var tabs = TabsWidthCount($(".content-tabs").children().not(".J_menuTabs"));
        var k = $(".content-tabs").outerWidth(true) - tabs;
        var p = 0;
        if ($(".page-tabs-content").outerWidth() < k) { p = 0 } else {
            if (next <= (k - $(tag).outerWidth(true) - $(tag).next().outerWidth(true))) {
                if ((k - $(tag).next().outerWidth(true)) > next) {
                    p = bother;
                    var m = tag;
                    while ((p - $(m).outerWidth()) > ($(".page-tabs-content").outerWidth() - k)) {
                        p -= $(m).prev().outerWidth();
                        m = $(m).prev()
                    }
                }
            } else { if (bother > (k - $(tag).outerWidth(true) - $(tag).prev().outerWidth(true))) { p = bother - $(tag).prev().outerWidth(true) } }
        }
        $(".page-tabs-content").animate({ marginLeft: 0 - p + "px" }, "fast")
    }

    //Click to select a tag event
    var SelectTag = function () {
        if (!$(this).hasClass("active")) {
            var id = $(this).data("id");
            SuspendPageScript($(".J_iframe:visible").attr("id"))
            $("#J_mainContent .J_iframe").hide();
            RecoveryPageScript(_J_Frame_Prefix_ + id);
            $("#J_mainContent .J_iframe[data-id='" + id + "']").show();
            $(this).addClass("active").siblings(".J_menuTab").removeClass("active");
            FloatMenuTab(this);
        }
        return false;
    }

    //Close the tab window
    var CloseFrame = function () {
        var self = $(this).parents(".J_menuTab");
        ReleasePageScript(_J_Frame_Prefix_ + self.data("id"))
        if (self.hasClass("active")) {
            var showid = null;
            if (self.next(".J_menuTab").size()) {
                showid = self.next(".J_menuTab:first").addClass("active").data("id");
                var n = parseInt($(".page-tabs-content").css("margin-left"));
                if (n < 0) { $(".page-tabs-content").animate({ marginLeft: (n + self.width()) + "px" }, "fast") }
            } else if (self.prev(".J_menuTab").size()) {
                showid = self.prev(".J_menuTab:last").addClass("active").data("id");
            }
            RecoveryPageScript(_J_Frame_Prefix_ + showid)
            $("#J_mainContent .J_iframe[data-id='" + showid + "']").show().siblings(".J_iframe").hide();
        }
        $("#J_mainContent .J_iframe[data-id='" + self.data("id") + "']").remove();

        self.remove();
        FloatMenuTab($(".J_menuTab.active"))

        return false
    }

    var ClockRun = function () { $("#localtime").html(new Date().getLocalFormat()); }

    var DefaultBtnAction = {
        Refresh: function () {
            Core.Refersh();
        },
        Delete: function () {
            var ids = Core.GetSelectId();
            if (ids.length == 0) { Core.Alert("请选择至少一条数据"); return; }

            Core.Confirm("是否确定删除数据", function (result) {
                if (result) {
                    $.post(defualt_Path + "Delete", { "ids": ids }, function (data) {
                        if (data > 0) {
                            location.href = defualt_Path;
                        }
                        else {
                            Core.Alert("删除失败");
                        }
                    });
                }

            });
        }
    }

    //Set Ajax Loader Shadow
    var AjaxSetup = function () {
        $(document).ajaxStart(function () {
            $.Shadow.show()
        }).ajaxComplete(function () {
            $.Shadow.hide()
        }).ajaxError(function () {
            Core.alert("404 Not Found");
        })
    }

    var TableRowSelect = function () {
        $("#J_mainContent").on("change", "table :checkbox", function () {
            var self = $(this);
            if ($(this).hasClass("selectAll")) {
                self.closest("table").find(":checkbox:not(.selectAll)").each(function () {
                    $(this).prop("checked", self.is(":checked")).change();
                });
            } else {
                var index = _Select_ID_.indexOf(self.val());
                if (self.is(":checked")) {
                    if (index < 0) {
                        _Select_ID_.push(self.val());
                    }
                } else {
                    if (index > -1) {
                        _Select_ID_.splice(index, 1)
                    }
                }
            }
        })
    }

    //find suspend module stuck
    var FindModuleStack = function (id) {
        for (var i in _Script_Module_Stack_)
            if (_Script_Module_Stack_[i].id == id)
                return _Script_Module_Stack_[i];
        return undefined;
    }

    var ReleaseStackModule = function (id) {
        for (var i in _Script_Module_Stack_) {
            if (_Script_Module_Stack_[i].id == id) {
                for (var name in _Script_Module_Stack_[i].modules)
                    RemoveWindowVariable(name);
                _Script_Module_Stack_[i] = undefined;
                _Script_Module_Stack_.splice(i, 1);
                break;
            }
        }
    }

    var RemoveWindowVariable = function (name) {
        if (! delete window[name])
            window[name] = undefined;
    }

    var SaveScriptModule = function (module, frameid) {
        if (!$.isArray(module)) { module = [module]; }
        var stack = FindModuleStack(frameid);
        if (stack == undefined) {
            _Select_ID_ = [];
            stack = { id: frameid, modules: new Object(), selected: _Select_ID_ };
            _Script_Module_Stack_.push(stack);
        }
        for (var i in module) {
            var obj = window[module[i]];
            if (obj == undefined) continue;
            stack.modules[module[i]] = obj;
        }
    }

    var ReleasePageScript = function (frameid, module) {
        if (frameid == undefined || frameid == null)
            frameid = GetActiveFrameId();
        var stack = FindModuleStack(frameid);
        if (module == undefined) {
            ReleaseStackModule(frameid);
            stack = null;
        } else {
            stack.modules[module] = undefined;
        }
    }

    var SuspendPageScript = function (frameid) {
        if (frameid == undefined || frameid == null)
            frameid = GetActiveFrameId();
        var stack = FindModuleStack(frameid);
        if (stack == undefined) return false;
        for (var name in stack.modules)
            RemoveWindowVariable(name);
    }

    var RecoveryPageScript = function (frameid) {
        if (frameid == undefined || frameid == null)
            frameid = GetActiveFrameId();
        var stack = FindModuleStack(frameid);
        if (stack == undefined) return false;
        for (var name in stack.modules)
            eval(name + " = stack.modules['" + name + "']");
        _Select_ID_ = stack.selected;
    }

    var GetActiveFrameId = function () {
        return $(".J_menuTab.active").size() ? _J_Frame_Prefix_ + $(".J_menuTab.active").data("id") : null;
    }


    return {
        Init: function () {
            BodySmallClass();
            CollapseIBox();
            CloseIBox();
            MinimalizeMenu();
            $('#side-menu').metisMenu();
            $('.modal').appendTo("body");
            fix_height();
            $(window).bind("resize", BodySmallClass);
            $(".J_tabLeft,.J_tabRight").on("click", TabsRoll);
            $(".J_menuItem").on("click", JOpenPage);
            $(".J_menuTabs").on("click", ".J_menuTab", SelectTag);
            $(".J_menuTabs").on("click", ".J_menuTab i", CloseFrame);
            $(".J_tabShowActive").on("click", function () { FloatMenuTab($(".J_menuTab.active")) })
            $(".J_tabCloseOther").on("click", function () { $(".J_menuTab.active").siblings().find("i").click(); });
            $(".J_tabCloseAll").on("click", function () { $(".J_menuTab i").click(); })
            ClockRun();
            setInterval(ClockRun, 1000);
            AjaxSetup()
            TableRowSelect();
        },
        InitAjaxEdit: function (ticket) {
            var script = $("#" + ticket);
            var iframe = script.closest(".J_iframe");
            iframe.find("form[data-ajax-update='#" + ticket + "']").attr("data-ajax-update", "#" + _J_Frame_Prefix_ + iframe.data("id"));
            script.remove();
            iframe.find("select[multiple]").chosen();
            iframe.find(".btn-lookup[data-url]").click(function () {
                Core.lookup($(this).data("url"), $(this).data("width"), $(this).data("height"), "Core.lookupDone");
            })
        },
        goToUrl: function (url, frameId) {
            if (frameId == undefined) {
                var active = $(".J_menuTab.active");
                if (active.length == 0) return;
                frameId = active.data("id");
            }
            $(".J_iframe[data-id='" + frameId + "']").load(url);
        },
        alert: function (msg, func) {
            bootbox.alert(msg, func);
            return false;
        },
        prompt: function (msg, func) {
            bootbox.prompt(msg, func);
            return false;
        },
        confirm: function (msg, func) {
            bootbox.confirm(msg, func);
            return false;
        },
        loadScript: function (module, success) {
            var file = _Script_Module_Root_ + module + ".js";
            Core.loadFileScript(file, module, success);

        },
        loadFileScript: function (file, module, success) {
            jQuery.getScript(file, function () {
                SaveScriptModule(module, GetActiveFrameId());
                success();
            });
        },
        getActiveFrameId: function () {
            return GetActiveFrameId();
        },
        saveScriptModule: function (module, frameid) {
            SaveScriptModule(module, frameid);
        },
        releasePageScript: function (frameid, module) {
            ReleasePageScript(frameid, module);
        },
        getSelectId: function () {
            var ids = [];
            for (i in _Select_ID_) ids.push(_Select_ID_[i]);
            return ids;
        },
        modal: function (url, w, h) {
            var modalId = "modal-" + Math.floor(Math.random() * 10000);
            var param = "";
            if (w != undefined && parseInt(w) != NaN) { param += " width:" + parseInt(w) + "px;"; }
            if (h != undefined && parseInt(h) != NaN) { param += " height:" + parseInt(h) + "px;"; }
            var $modalDiv = $('<div id="' + modalId + '" class="modal container fade" tabindex="-1" style="' + param + '"></div>').appendTo('body');
            if (url.indexOf("?") > 0) { url += "&"; }
            else { url += "?"; }
            url += "_target=" + modalId;

            //var parentModel = App.getModel();

            $modalDiv.load(url, '', function () {

                $modalDiv.modal().on('hidden.bs.modal', function () {
                    $modalDiv.remove();
                    //App.releaseModel(parentModel);
                });
            });
        },
        lookup: function (url, callbackName, width, height) {
            url += "&callback=" + callbackName;
            this.modal(url, width, height);
        },
        lookupDone: function (lookupId, multi) {
            var ids = Core.getSelected();
            var names = Core.getSelectedNames();
            if (ids.length == 0) return Core.alert("请选择数据！");
            if (multi == 0) {
                var id = ids[0];
                var name = names[0];
                $('#' + lookupId).val(id);
                $('#' + lookupId + "Name").val(name);
            } else {
                $('input[Name="' + lookupId + '"]').remove();
                for (var i = 0; i < ids.length; i++) {
                    $('<input type="hidden" id="' + lookupId + '" name="' + lookupId + '" value="' + ids[i] + '" />').insertBefore($('#' + lookupId + "Name"));
                }
                $('#' + lookupId + "Name").val(names.join(","));
            }
        },
    }

}();