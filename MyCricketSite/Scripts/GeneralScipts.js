/*! Lazy Load 1.9.4 - MIT license - Copyright 2010-2013 Mika Tuupola http://www.appelsiini.net/projects/lazyload*/
!function (a, b, c, d) { var e = a(b); a.fn.lazyload = function (f) { function g() { var b = 0; i.each(function () { var c = a(this); if (!j.skip_invisible || c.is(":visible")) if (a.abovethetop(this, j) || a.leftofbegin(this, j)); else if (a.belowthefold(this, j) || a.rightoffold(this, j)) { if (++b > j.failure_limit) return !1 } else c.trigger("appear"), b = 0 }) } var h, i = this, j = { threshold: 0, failure_limit: 0, event: "scroll", effect: "show", container: b, data_attribute: "original", skip_invisible: !0, appear: null, load: null, placeholder: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXYzh8+PB/AAffA0nNPuCLAAAAAElFTkSuQmCC" }; return f && (d !== f.failurelimit && (f.failure_limit = f.failurelimit, delete f.failurelimit), d !== f.effectspeed && (f.effect_speed = f.effectspeed, delete f.effectspeed), a.extend(j, f)), h = j.container === d || j.container === b ? e : a(j.container), 0 === j.event.indexOf("scroll") && h.bind(j.event, function () { return g() }), this.each(function () { var b = this, c = a(b); b.loaded = !1, (c.attr("src") === d || c.attr("src") === !1) && c.is("img") && c.attr("src", j.placeholder), c.one("appear", function () { if (!this.loaded) { if (j.appear) { var d = i.length; j.appear.call(b, d, j) } a("<img />").bind("load", function () { var d = c.attr("data-" + j.data_attribute); c.hide(), c.is("img") ? c.attr("src", d) : c.css("background-image", "url('" + d + "')"), c[j.effect](j.effect_speed), b.loaded = !0; var e = a.grep(i, function (a) { return !a.loaded }); if (i = a(e), j.load) { var f = i.length; j.load.call(b, f, j) } }).attr("src", c.attr("data-" + j.data_attribute)) } }), 0 !== j.event.indexOf("scroll") && c.bind(j.event, function () { b.loaded || c.trigger("appear") }) }), e.bind("resize", function () { g() }), /(?:iphone|ipod|ipad).*os 5/gi.test(navigator.appVersion) && e.bind("pageshow", function (b) { b.originalEvent && b.originalEvent.persisted && i.each(function () { a(this).trigger("appear") }) }), a(c).ready(function () { g() }), this }, a.belowthefold = function (c, f) { var g; return g = f.container === d || f.container === b ? (b.innerHeight ? b.innerHeight : e.height()) + e.scrollTop() : a(f.container).offset().top + a(f.container).height(), g <= a(c).offset().top - f.threshold }, a.rightoffold = function (c, f) { var g; return g = f.container === d || f.container === b ? e.width() + e.scrollLeft() : a(f.container).offset().left + a(f.container).width(), g <= a(c).offset().left - f.threshold }, a.abovethetop = function (c, f) { var g; return g = f.container === d || f.container === b ? e.scrollTop() : a(f.container).offset().top, g >= a(c).offset().top + f.threshold + a(c).height() }, a.leftofbegin = function (c, f) { var g; return g = f.container === d || f.container === b ? e.scrollLeft() : a(f.container).offset().left, g >= a(c).offset().left + f.threshold + a(c).width() }, a.inviewport = function (b, c) { return !(a.rightoffold(b, c) || a.leftofbegin(b, c) || a.belowthefold(b, c) || a.abovethetop(b, c)) }, a.extend(a.expr[":"], { "below-the-fold": function (b) { return a.belowthefold(b, { threshold: 0 }) }, "above-the-top": function (b) { return !a.belowthefold(b, { threshold: 0 }) }, "right-of-screen": function (b) { return a.rightoffold(b, { threshold: 0 }) }, "left-of-screen": function (b) { return !a.rightoffold(b, { threshold: 0 }) }, "in-viewport": function (b) { return a.inviewport(b, { threshold: 0 }) }, "above-the-fold": function (b) { return !a.belowthefold(b, { threshold: 0 }) }, "right-of-fold": function (b) { return a.rightoffold(b, { threshold: 0 }) }, "left-of-fold": function (b) { return !a.rightoffold(b, { threshold: 0 }) } }) }(jQuery, window, document);

/*  http://git.aaronlumsden.com/tabulous.js/ */
!function (s) { function t(t, e) { this.element = t, this.$elem = s(this.element), this.options = s.extend({}, a, e), this._defaults = a, this._name = i, this.init() } var i = "tabulous", a = { effect: "scale" }; t.prototype = { init: function () { { var t = this.$elem.find("a"), i = this.$elem.find("li:first-child").find("a"); this.$elem.find("li:last-child").after('<span class="tabulousclear"></span>') } "scale" == this.options.effect ? tab_content = this.$elem.find("div").not(":first").not(":nth-child(1)").addClass("hidescale") : "slideLeft" == this.options.effect ? tab_content = this.$elem.find("div").not(":first").not(":nth-child(1)").addClass("hideleft") : "scaleUp" == this.options.effect ? tab_content = this.$elem.find("div").not(":first").not(":nth-child(1)").addClass("hidescaleup") : "flip" == this.options.effect && (tab_content = this.$elem.find("div").not(":first").not(":nth-child(1)").addClass("hideflip")); var a = this.$elem.find("#tabs_container"), e = a.find("div:first").height(), d = this.$elem.find("div:first").find("div"); d.css({ position: "absolute", top: "40px" }), a.css("height", e + "px"), i.addClass("tabulous_active"), t.bind("click", { myOptions: this.options }, function (i) { i.preventDefault(); var e = i.data.myOptions, n = e.effect, l = s(this), h = l.parent().parent().parent(), o = l.attr("href"); a.addClass("transition"), t.removeClass("tabulous_active"), l.addClass("tabulous_active"), thisdivwidth = h.find("div" + o).height(), "scale" == n ? (d.removeClass("showscale").addClass("make_transist").addClass("hidescale"), h.find("div" + o).addClass("make_transist").addClass("showscale")) : "slideLeft" == n ? (d.removeClass("showleft").addClass("make_transist").addClass("hideleft"), h.find("div" + o).addClass("make_transist").addClass("showleft")) : "scaleUp" == n ? (d.removeClass("showscaleup").addClass("make_transist").addClass("hidescaleup"), h.find("div" + o).addClass("make_transist").addClass("showscaleup")) : "flip" == n && (d.removeClass("showflip").addClass("make_transist").addClass("hideflip"), h.find("div" + o).addClass("make_transist").addClass("showflip")), a.css("height", thisdivwidth + "px") }) }, yourOtherFunction: function () { } }, s.fn[i] = function (s) { return this.each(function () { new t(this, s) }) } }(jQuery, window, document);



function getSiteUrl() {
    // return location.protocol + '//' + location.hostname + (location.port ? ':' + location.port + "/" : '/');

    var loc = document.location.toString();
    if (loc.toLowerCase().indexOf('localhost:') > -1)
        //return location.protocol + '//' + location.hostname + (location.port ? ':' + location.port + "/" : '/');
        return location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '/');
    else {
        var applicationNameIndex = loc.indexOf('/', loc.indexOf('://') + 3);
        var applicationName = loc.substring(0, applicationNameIndex) + '/';
        // var webFolderIndex = _location.indexOf('/', _location.indexOf(applicationName) + applicationName.length);
        /// var webFolderFullPath = _location.substring(0, webFolderIndex) + '/';
        //    return webFolderFullPath;
        return applicationName;
    }
}

function OpenPopup(popupsrc) {
    return $.magnificPopup.open({
        items: {
            src: popupsrc, // can be a HTML string, jQuery object, or CSS selector
            type: 'inline'
        },
        fixedContentPos: false,
        fixedBgPos: true,
        overflowY: 'auto',
        closeBtnInside: true,
        preloader: false,
        midClick: true,
        removalDelay: 300,
        mainClass: 'my-mfp-zoom-in',

    });
}




Date.prototype.customFormat = function (formatString) {
    /*
     * token:     description:             example:
#YYYY#     4-digit year             1999
#YY#       2-digit year             99
#MMMM#     full month name          February
#MMM#      3-letter month name      Feb
#MM#       2-digit month number     02
#M#        month number             2
#DDDD#     full weekday name        Wednesday
#DDD#      3-letter weekday name    Wed
#DD#       2-digit day number       09
#D#        day number               9
#th#       day ordinal suffix       nd
#hhh#      military/24-based hour   17
#hh#       2-digit hour             05
#h#        hour                     5
#mm#       2-digit minute           07
#m#        minute                   7
#ss#       2-digit second           09
#s#        second                   9
#ampm#     "am" or "pm"             pm
#AMPM#     "AM" or "PM"             PM
     * 
     */
    var YYYY, YY, MMMM, MMM, MM, M, DDDD, DDD, DD, D, hhh, hh, h, mm, m, ss, s, ampm, AMPM, dMod, th;
    var dateObject = this;
    YY = ((YYYY = dateObject.getFullYear()) + "").slice(-2);
    MM = (M = dateObject.getMonth() + 1) < 10 ? ('0' + M) : M;
    MMM = (MMMM = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"][M - 1]).substring(0, 3);
    DD = (D = dateObject.getDate()) < 10 ? ('0' + D) : D;
    DDD = (DDDD = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"][dateObject.getDay()]).substring(0, 3);
    th = (D >= 10 && D <= 20) ? 'th' : ((dMod = D % 10) == 1) ? 'st' : (dMod == 2) ? 'nd' : (dMod == 3) ? 'rd' : 'th';
    formatString = formatString.replace("#YYYY#", YYYY).replace("#YY#", YY).replace("#MMMM#", MMMM).replace("#MMM#", MMM).replace("#MM#", MM).replace("#M#", M).replace("#DDDD#", DDDD).replace("#DDD#", DDD).replace("#DD#", DD).replace("#D#", D).replace("#th#", th);

    h = (hhh = dateObject.getHours());
    if (h == 0) h = 24;
    if (h > 12) h -= 12;
    hh = h < 10 ? ('0' + h) : h;
    AMPM = (ampm = hhh < 12 ? 'am' : 'pm').toUpperCase();
    mm = (m = dateObject.getMinutes()) < 10 ? ('0' + m) : m;
    ss = (s = dateObject.getSeconds()) < 10 ? ('0' + s) : s;
    return formatString.replace("#hhh#", hhh).replace("#hh#", hh).replace("#h#", h).replace("#mm#", mm).replace("#m#", m).replace("#ss#", ss).replace("#s#", s).replace("#ampm#", ampm).replace("#AMPM#", AMPM);
}