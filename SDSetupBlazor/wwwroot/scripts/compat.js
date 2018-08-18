
function checkIE() {
	if (window.navigator.userAgent.indexOf('Trident/') > -1) {
		$('body').replaceWith(
			'<div class="ui active dimmer" style="height:100%;width:100%">' +
			'<img class="ui small image" src="/img/fail.png">' +
			'<h3 style="color:#ffffff;margin-top:1.5em">Unfortunately, Internet Explorer is not compatible with this site.</h3>' +
			'<h6 style="color:#ffffff">(You should seriously consider using a better browser like Firefox or Chrome)</h6>' +
			'</div>'
		);
		return;
	}
	if (window.navigator.userAgent.indexOf('MSIE ') > -1) {
		$('body').html(
			'<div class="ui active dimmer" style="height:100%;width:100%">' +
				'<img class="ui small image" src="/img/fail.png">' +
				'<h3 style="color:#ffffff;margin-top:1.5em">Unfortunately, Internet Explorer is not compatible with this site.</h3>' +
				'<h6 style="color:#ffffff">(You should seriously consider using a better browser like Firefox or Chrome)</h6>' +
			'</div>'
		);
		return;
	}
	return;
}


