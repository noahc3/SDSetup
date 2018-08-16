window.menu_interop = {
	interop_registerClick: function () {
		console.log("fucking haha");
		$('.ui .item').on('click', function () {
			console.log("fucking haha numero dos");
			$('.ui .item').removeClass('active');
			$(this).addClass('active');
		});
		return 1;
	}
};