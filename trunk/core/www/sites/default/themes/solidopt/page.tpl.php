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
		<div id="wrapper">
			<div id="topmost">
				<?php print $topmost; ?>
			</div>
		
			<div id="header">
				<a href="<?php print $front_page; ?>"><img id="logo_title" src="<?php print $logo; ?>" alt="Logo"/></a>
				<a href="<?php print $front_page; ?>"><h1><?php print $site_slogan; ?></h1></a>
				<?php print $header; ?>
				<?php if (isset($primary_links)) : ?>
					<div class="clear"></div>
					<?php print theme('links', $primary_links, array('class' => 'links primary-links')) ?>
				<?php endif; ?>
			</div>
	
			<div id="navigation">
				<h2><?php print $title; ?></h2>
			</div>	

			<div class="holder" id="center">
				<div id="content">
				<!-- BEGIN MAIN CONTENT -->
					<?php if ($tabs): print '<ul class="tabs primary">'. $tabs .'</ul>'; endif; ?>
					<?php if ($tabs2): print '<ul class="tabs secondary">'. $tabs2 .'</ul>'; endif; ?>
					<?php print $content; ?>
				<!-- END MAIN CONTENT -->
				</div><!--#content-->
			</div>
		
			<div class="holder" id="left">
				<!-- BEGIN LEFT SIDE CONTENT -->
				<div id="leftsidebar">
					<div><?php print $left; ?></div>
				</div>	
				<!-- END LEFT SIDE CONTENT -->
			</div>
			<div class="holder" id="right">
				<!-- BEGIN RIGHT SIDE CONTENT -->
				<div id="rightsidebar">
					<div><?php print $right; ?></div>
				</div>	
				<!-- END RIGHT SIDE CONTENT -->
			</div>
				  	    
		</div><!--#wrapper-->
		<div id="footer">
			<ul id="footer-menu">
				<?php print $footer_message; ?>
				<?php if (!empty($footer)): print $footer; endif; ?>
			</ul>
		</div>
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
