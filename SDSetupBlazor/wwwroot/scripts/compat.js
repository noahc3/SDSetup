
function checkIE() {
	if (getBrowserCompatInfo() == 2) {
		$('body').replaceWith(
			'<div class="ui active dimmer" style="height:100%;width:100%">' +
			'<img class="ui small image" src="/img/fail.png">' +
			'<h3 style="color:#ffffff;margin-top:1.5em">Unfortunately, Internet Explorer is not compatible with this site.</h3>' +
			'<h6 style="color:#ffffff">(You should seriously consider using a better browser like Firefox or Chrome)</h6>' +
			'</div>'
		);
		return;
	}
}

function getBrowserCompatInfo() {
	if (window.navigator.userAgent.indexOf("Edge") > -1) {
		return 1
	}
	if (window.navigator.userAgent.indexOf('Trident/') > -1) {
		return 2
	}
	if (window.navigator.userAgent.indexOf('MSIE ') > -1) {
		return 2
	}
	if (window.navigator.userAgent.indexOf('Firefox') > -1) {
		return 3;
	}
	return 0;
}

navigator.browserSpecs = (function () {
    var ua = navigator.userAgent, tem,
        M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
    if (/trident/i.test(M[1])) {
        tem = /\brv[ :]+(\d+)/g.exec(ua) || [];
        return { name: 'IE', version: (tem[1] || '') };
    }
    if (M[1] === 'Chrome') {
        tem = ua.match(/\b(OPR|Edge)\/(\d+)/);
        if (tem != null) return { name: tem[1].replace('OPR', 'Opera'), version: tem[2] };
    }
    M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
    if ((tem = ua.match(/version\/(\d+)/i)) != null)
        M.splice(1, 1, tem[1]);
    return { name: M[0], version: M[1] };
})();