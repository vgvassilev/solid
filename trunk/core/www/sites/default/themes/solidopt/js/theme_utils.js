function ShowMore() {
	var elemCenterStyle = document.getElementById('center').style;
	var elemRightStyle = document.getElementById('right').style;

	var image = document.getElementById('showmore');

	if (elemCenterStyle.width == 'auto') {
		elemCenterStyle.width = '58%';
		elemCenterStyle.marginRight = '0';
		elemRightStyle.display = 'block';

		image.src = "/sites/default/themes/solidopt/css/images/menu/more.png";
		image.alt = ">>";
	}
	else {
		elemCenterStyle.width = 'auto';
		elemCenterStyle.marginRight = '10px';
		elemRightStyle.display = 'none';

		image.src = "/sites/default/themes/solidopt/css/images/menu/less.png";
		image.alt = "<<";
	}

	return true;
}
