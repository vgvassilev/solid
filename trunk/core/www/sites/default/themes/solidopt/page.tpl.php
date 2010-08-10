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
				<?php print $lang; ?>
			</div>
			<div id="header">
				<?php print $header; ?>
				<div class="clear"></div>
				<div id="logo">
					<a href="<?php print $front_page; ?>"><img id="logo_title" src="<?php print $logo; ?>" alt="Logo"/></a>
					<a href="<?php print $front_page; ?>"><h1><?php print $site_slogan; ?></h1></a>
				</div>
				<?php if (isset($primary_links)) : ?>

					<?php print $mainmenu ?>
				<?php endif; ?>
				
			</div>
	
			<div id="navigation">
				<div id="quicklinks">
					<?php 
					print "<span>" . t("Quick start:") . "</span>"; 
					print $quickstart; 
					?>
				</div>
				<div id="search">
					<?php 				
					print $searchbar;
					?>
				</div>
				
			</div>	

			<?php if ($left): ?>
			<div id="left">
				<!-- BEGIN LEFT SIDE CONTENT -->
				<div id="leftsidebar">
					<div><?php print $left; ?></div>
				</div>	
				<!-- END LEFT SIDE CONTENT -->
			</div>
			<?php ; endif ?>

			<?php if ($right): ?>
			<div id="right">
				<!-- BEGIN RIGHT SIDE CONTENT -->
				<div id="rightsidebar">
					<div><?php print $right; ?></div>
				</div>	
				<!-- END RIGHT SIDE CONTENT -->
			</div>			
			<?php ; endif ?>
			
			<div id="center">
				<div id="content">
				<!-- BEGIN MAIN CONTENT -->
					<?php if ($tabs): print '<ul class="tabs primary">'. $tabs .'</ul>'; endif; ?>
					<?php if ($tabs2): print '<ul class="tabs secondary">'. $tabs2 .'</ul>'; endif; ?>
					<?php print $content; ?>
				<!-- END MAIN CONTENT -->
				</div><!--#content-->
			</div>	

				  	    
		</div><!--#wrapper-->
		<div class="clear"></div>
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
