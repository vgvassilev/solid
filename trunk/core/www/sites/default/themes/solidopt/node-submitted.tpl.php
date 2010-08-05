<?php 
	$username = theme('username', $node);
	$datetime = format_date($node->created, 'custom', 'm/d/Y');
	$submitted = "$datetime &emsp; by $username";
	// node should have comments enabled and user permission to read them
	if ($node->comment == COMMENT_NODE_READ_WRITE && user_access("access comments")) {
	    if ($node->comment_count > 0) {
		$submitted .= "<span>" . l(t("Comments: ") . $node->comment_count, drupal_get_path_alias($node->nid)) . "</span>";
	    }
	    else {
		$submitted .= "<span>" . t("Comments: ") . $node->comment_count . "</span>";
	    }
	}
	print $submitted;
?>
