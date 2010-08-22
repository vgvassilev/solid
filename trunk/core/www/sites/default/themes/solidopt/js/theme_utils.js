function ShowMore() {
	var elemCenterStyle = document.getElementById('center');
	var elemRightStyle = document.getElementById('right');

	var image = document.getElementById('showmore');

	if (elemCenterStyle.style.width == 'auto') {
		elemCenterStyle.style.width = '58%';
		elemCenterStyle.style.marginRight = '0';
		if (elemRightStyle){ 
			elemRightStyle.style.display = 'block'; 
		}

		image.src = "/sites/default/themes/solidopt/css/images/menu/more.png";
		image.alt = ">>";
	}
	else {
		elemCenterStyle.style.width = 'auto';
		elemCenterStyle.style.marginRight = '10px';
		if (elemRightStyle) {
			elemRightStyle.style.display = 'none';
		}

		image.src = "/sites/default/themes/solidopt/css/images/menu/less.png";
		image.alt = "<<";
	}

	return true;
}
