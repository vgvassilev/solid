<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="<?php print $language->language ?>" lang="<?php print $language->language ?>" dir="<?php print $language->dir ?>">

	<head>
		<?php print $head; ?>
		<title><?php print $head_title; ?></title>
		<?php print $styles; ?>
		<?php print $scripts; ?>
		<script type="text/javascript"><?php /* Needed to avoid Flash of Unstyled Content in IE */ ?> </script>
	</head>
	
	<body class="<?php print $body_classes; ?>">
	<div id="page">
		<div id="topmost">
			<?php print $topmost; ?>
		</div>
		
		<div id="header">
			<a href="<?php print $front_page; ?>"><img id="logo_title" src="<?php print $logo; ?>" alt="Logo"/></a>
			<a href="<?php print $front_page; ?>"><h1><?php print $site_slogan; ?></h1></a>
			<?php print $header; ?>
		</div>
		
	    <div id="content-header">
			<h2><?php print $title; ?></h2>
		</div>
	    <div id="wrapper">
			
			<!-- BEGIN SIDE CONTENT -->
			<div id="sidebar">
				<div><?php print $left; ?></div>
			</div>
			<!-- END SIDE CONTENT -->
			<div id="content">
			<!-- BEGIN MAIN CONTENT -->
				<?php print $content; ?>
			<!-- END MAIN CONTENT -->
			</div><!--#content-->
	    

		    <div id="footer">
				<ul id="footer-menu">
					<?php print $footer_message; ?>
					<?php if (!empty($footer)): print $footer; endif; ?>
				</ul>
		        <div style="clear: both;"></div>
		    </div>
	    
		</div><!--#wrapper-->
	</div><!--#page-->

<script type="text/javascript">
var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");
document.write(unescape("%3Cscript src='" + gaJsHost + "google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E"));
</script>
<script type="text/javascript">
try {
var pageTracker = _gat._getTracker("UA-11448832-1");
pageTracker._trackPageview();
} catch(err) {}</script>
<?php print $closure; ?>
</body></html>