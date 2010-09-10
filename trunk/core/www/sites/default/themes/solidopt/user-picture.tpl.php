<?php
// $Id: user-picture.tpl.php,v 1.2 2007/08/07 08:39:36 goba Exp $

/**
 * @file user-picture.tpl.php
 * Default theme implementation to present an picture configured for the
 * user's account.
 *
 * Available variables:
 * - $picture: Image set by the user or the site's default. Will be linked
 *   depending on the viewer's permission to view the users profile page.
 * - $account: Array of account information. Potentially unsafe. Be sure to
 *   check_plain() before use.
 *
 * @see template_preprocess_user_picture()
 */
?>

<?php 
if ($picture):
  if ( $account->content['Personal Info']['profile_cv'] ) :
    global $vvpic; 
    $vvpic = $picture; 
  else :
?>
  <div class="picture">
    <?php print $picture; ?> 
  </div>
  <?php endif; ?>
<?php endif; ?>
