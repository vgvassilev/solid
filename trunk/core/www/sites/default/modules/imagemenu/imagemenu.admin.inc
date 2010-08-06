<?php
// $Id: imagemenu.admin.inc,v 1.1.2.5 2010/05/09 18:44:59 rayvaughn Exp $

/**
 * @file
 * Administrative page callbaks for imagemenu module.
 */


 /**
 * Menu callback which shows an overview page of all the custom imagemenus and their descriptions.
 */
function imagemenu_overview_page() {
  $sql = "SELECT mid, title, description FROM {imagemenu} WHERE pid = %d ORDER BY weight, title;";
  $result = db_query($sql, 0);
  $content = array();
  while ($menu = db_fetch_array($result)) {
    $menu['href'] = 'admin/build/menu/imagemenu-customize/'. $menu['mid'];
    $menu['localized_options'] = array();
    $content[] = $menu;
  }
  if (empty($content)) {
    $content[] = array(
      'description' => t('There are no Imagemenus yet.'),
      'localized_options' => array()
    );
  }
  return theme('admin_block_content', $content);
}

/**
 * Form for editing an entire imagemenu tree at once.
 *
 * Shows for one menu the imagemenu items and relevant operations.
 */
function imagemenu_overview_form(&$form_state, $menu) {
  $tree = imagemenu_tree_data($menu['mid']);
  $form = _imagemenu_overview_tree_form($tree);
  $form['#menu'] =  $menu;
  if (element_children($form)) {
    $form['submit'] = array(
      '#type' => 'submit',
      '#value' => t('Save configuration'),
    );
  }
  else {
    $form['empty_menu'] = array('#value' => t('There are no Imagemenu items yet.'));
  }
  return $form;
}

/**
 * Recursive helper function for imagemenu_overview_form().
 */
function _imagemenu_overview_tree_form($tree) {
  static $form = array('#tree' => TRUE);
  foreach ($tree as $data) {
    $title = '';
    $item = $data['link'];
    if ($item) {
      $mid = 'mid:'. $item['mid'];
      $form[$mid]['#item'] = $item;
      $form[$mid]['#attributes'] = (!$item['enabled']) ? array('class' => 'menu-disabled') : array('class' => 'menu-enabled');
      $form[$mid]['title']['#value'] = l($item['title'], $item['path']) . (!$item['enabled'] ? ' ('. t('disabled') .')' : '');
      $form[$mid]['enabled'] = array(
        '#type' => 'checkbox',
        '#default_value' => $item['enabled'],
      );
      $form[$mid]['type'] = array(
        '#type' => 'checkbox',
        '#default_value' => $item['type'],
      );
      $form[$mid]['target'] = array(
        '#type' => 'checkbox',
        '#default_value' => $data['link']['target'],
      );
      $form[$mid]['weight'] = array(
        '#type' => 'weight',
        '#delta' => 50,
        '#default_value' => isset($form_state[$mid]['weight']) ? $form_state[$mid]['weight'] : $item['weight'],
      );
      $form[$mid]['mid'] = array(
        '#type' => 'hidden',
        '#value' => $item['mid'],
      );
      $form[$mid]['pid'] = array(
        '#type' => 'textfield',
        '#default_value' => isset($form_state[$mid]['pid']) ? $form_state[$mid]['pid'] : $item['pid'],
        '#size' => 6,
      );
      // Build a list of operations.
      $operations = array();
      $operations['edit'] = l(t('edit'), 'admin/build/menu/imagemenu/item/'. $item['mid'] .'/edit');
      $operations['delete'] = l(t('delete'), 'admin/build/menu/imagemenu/item/'. $item['mid'] .'/delete');

      $form[$mid]['operations'] = array();
      foreach ($operations as $op => $value) {
        $form[$mid]['operations'][$op] = array('#value' => $value);
      }
    }

    if ($data['below']) {
      _imagemenu_overview_tree_form($data['below']);
    }
  }
  return $form;
}

/**
 * Submit handler for the menu overview form.
 */
function imagemenu_overview_form_submit($form, &$form_state) {
  //
  $order = array_flip(array_keys($form['#post'])); // Get the $_POST order.
  $form = array_merge($order, $form); // Update our original form with the new order.

  $updated_items = array();
  $fields = array('enabled', 'type', 'weight', 'pid', 'target');
  foreach (element_children($form) as $mid) {
    if (isset($form[$mid]['#item'])) {
      $element = $form[$mid];
      // Update any fields that have changed in this menu item.
      foreach ($fields as $field) {
        if ($element[$field]['#value'] != $element[$field]['#default_value']) {
          $element['#item'][$field] = $element[$field]['#value'];
          $updated_items[$mid] = $element['#item'];
        }
      }
    }
  }
  // Save all our changed items to the database.
  foreach ($updated_items as $item) {
    db_query("UPDATE {imagemenu} SET pid = %d, enabled = %d, type = %d, weight = %d, target = %d WHERE mid = %d",
      $item['pid'], $item['enabled'], $item['type'], $item['weight'], $item['target'], $item['mid'] );
  }
}

/**
 * Theme the menu overview form into a table.
 */
function theme_imagemenu_overview_form($form) {
  drupal_add_tabledrag('menu-overview', 'match', 'parent', 'menu-pid', 'menu-pid', 'menu-mid', TRUE, MENU_MAX_DEPTH - 1);
  drupal_add_tabledrag('menu-overview', 'order', 'sibling', 'menu-weight');

  $header = array(
    t('Menu item'),
    array('data' => t('Enabled'), 'class' => 'checkbox'),
    array('data' => t('Expanded'), 'class' => 'checkbox'),
    array('data' => t('New Window'), 'class' => 'checkbox'),
    t('Weight'),
    array('data' => t('Operations'), 'colspan' => '3'),
  );

  $rows = array();
  foreach (element_children($form) as $mid) {
    if (isset($form[$mid]['enabled'])) {
      $element = &$form[$mid];
      // Build a list of operations.
      $operations = array();
      foreach (element_children($element['operations']) as $op) {
        $operations[] = drupal_render($element['operations'][$op]);
      }
      while (count($operations) < 2) {
        $operations[] = '';
      }

      // Add special classes to be used for tabledrag.js.
      $element['pid']['#attributes']['class'] = 'menu-pid';
      $element['mid']['#attributes']['class'] = 'menu-mid';
      $element['weight']['#attributes']['class'] = 'menu-weight';

      // Change the parent field to a hidden. This allows any value but hides the field.
      $element['pid']['#type'] = 'hidden';

      $row = array();
      $row[] = theme('indentation', $element['#item']['depth']) . drupal_render($element['title']);
      $row[] = array('data' => drupal_render($element['enabled']), 'class' => 'checkbox');
      $row[] = array('data' => drupal_render($element['type']), 'class' => 'checkbox');
      $row[] = array('data' => drupal_render($element['target']), 'class' => 'checkbox');
      $row[] = drupal_render($element['weight']) . drupal_render($element['pid']) . drupal_render($element['mid']);
      $row = array_merge($row, $operations);

      $row = array_merge(array('data' => $row), $element['#attributes']);
      $row['class'] = !empty($row['class']) ? $row['class'] .' draggable' : 'draggable';
      $rows[] = $row;
    }
  }
  $output = '';
  if ($rows) {
    $output .= theme('table', $header, $rows, array('id' => 'menu-overview'));
  }
  $output .= drupal_render($form);
  return $output;
}

/**
 * Menu callback; Build the form that handles the adding/editing of a custom menu.
 */
function imagemenu_edit_menu(&$form_state, $type, $menu = array()) {
  if ($type == 'edit') {
    $form['mid'] = array('#type' => 'value', '#value' => $menu['mid']);
    $form['#insert'] = FALSE;
    $form['delete'] = array(
      '#type' => 'submit',
      '#value' => t('Delete'),
      '#submit' => array('imagemenu_delete_menu_submit'),
      '#weight' => 10,
    );
  }
  else {
    $menu = array('title' => '', 'description' => '');
    $form['#insert'] = TRUE;
  }
  $form['#title'] = $menu['title'];
  $form['title'] = array(
    '#type' => 'textfield',
    '#title' => t('Title'),
    '#default_value' => $menu['title'],
    '#required' => TRUE,
  );
  $form['description'] = array(
    '#type' => 'textarea',
    '#title' => t('Description'),
    '#default_value' => $menu['description'],
  );
  $form['submit'] = array(
    '#type' => 'submit',
    '#value' => t('Save'),
  );

  return $form;
}

/**
 * Submit function for adding or editing a custom menu.
 */
function imagemenu_edit_menu_submit($form, &$form_state) {
  $menu = $form_state['values'];
  $path = 'admin/build/menu/imagemenu-customize/';
  if ($form['#insert']) {
    db_query("INSERT INTO {imagemenu} (pid, title, description) VALUES (%d, '%s', '%s')", 0, $menu['title'], $menu['description']);
    $menu['mid'] = db_last_insert_id('imagemenu', 'mid');
  }
  else {
    db_query("UPDATE {imagemenu} SET title = '%s', description = '%s' WHERE mid = %d", $menu['title'], $menu['description'], $menu['mid']);
  }
  $form_state['redirect'] = $path . $menu['mid'];
}

/**
 * Submit function for the 'Delete' button on the menu editing form.
 */
function imagemenu_delete_menu_submit($form, &$form_state) {
  $form_state['redirect'] = 'admin/build/menu/imagemenu-customize/'. $form_state['values']['mid'] .'/delete';
}

/**
 * Build a confirm form for deletion of a custom menu.
 */
function imagemenu_delete_menu_confirm(&$form_state, $menu) {
  $form['#menu'] = $menu;
  $caption = '<p>'. t('This action cannot be undone.') .'</p>';
  return confirm_form($form, t('Are you sure you want to delete the imagemenu %title?', array('%title' => $menu['title'])), 'admin/build/menu/imagemenu-customize/'. $menu['mid'], $caption, t('Delete'));
}

/**
 * Delete a imagemenu and all items in it.
 */
function imagemenu_delete_menu_confirm_submit($form, &$form_state) {
  $menu = $form['#menu'];
  $form_state['redirect'] = 'admin/build/menu/imagemenu';
  $pid = $menu['mid'];
  $items = imagemenu_fetch_rows($pid, TRUE);
  if ($items) {
    foreach ($items as $item) {
      db_query('DELETE FROM {imagemenu} WHERE mid = %d', $item['mid']);
    }
  }
  db_query('DELETE FROM {imagemenu} WHERE mid = %d', $pid);

  cache_clear_all();
  $t_args = array('%title' => $menu['title']);
  drupal_set_message(t('The menu %title and all its menu items have been deleted.', $t_args));
  watchdog('imagemenu', 'Deleted menu %title and all its menu items.', $t_args, WATCHDOG_NOTICE);
}

/**
 * Menu callback; Build the menu link editing form.
 */
function imagemenu_edit_item(&$form_state, $type, $item, $menu) {
    if ($type == 'add' || empty($item)) {
       $mid = isset($menu['mid']) ? $menu['mid'] : 0;
    }
    if ($type == 'edit') {
      $mid = $item['mid'];
    }
    $form['menu'] = array(
    '#type' => 'fieldset',
    '#title' => t('Menu settings'),
    '#collapsible' => FALSE,
    '#tree' => TRUE,
    '#weight' => -2,
    '#attributes' => array('class' => 'menu-item-form'),
    '#item' => $item,
  );
  if ($type == 'add' || empty($item)) {
    // This is an add form, initialize the menu link.
    $item = array('mid' => 0, 'pid' => $mid, 'title' => '', 'imagepath' => '', 'mouseover' => '', 'path' => '', 'alt' => '', 'enabled' => 1, 'type' => 0, 'weight' => 0);
  }
  if ($type == 'edit') {
    $form['delete'] = array(
      '#type' => 'submit',
      '#value' => t('Delete'),
      '#submit' => array('imagemenu_item_delete_submit'),
      '#weight' => 10,
    );
  }
  foreach (array('mid', 'pid', 'title', 'imagepath', 'mouseover', 'path', 'alt', 'enabled', 'type', 'weight') as $key) {
    $form['menu'][$key] = array('#type' => 'value', '#value' => $item[$key]);
  }

  $form['menu']['title'] = array(
    '#type' => 'textfield',
    '#title' => t('Title'),
    '#default_value' => $item['title'],
    '#description' => t('The name of the menu item (displayed when hovering over a menu image).'),
    '#required' => TRUE,
    '#weight' => -10,
  );
  $form['menu']['imagepath'] = array(
    '#type' => 'textfield',
    '#title' => t('Image Path'),
    '#default_value' => $item['imagepath'],
    '#description' => t('The path to the image.'),
    '#required' => TRUE,
    '#weight' => -9,
  );
  $form['menu']['mouseover'] = array(
    '#type' => 'textfield',
    '#title' => t('Mouseover image path'),
    '#default_value' => $item['mouseover'],
    '#description' => t('Optional. The path to an image to display on mouseover.'),
    '#weight' => -8,
  );
  if (module_exists('imce')) {
    $form['menu']['imagepath']['#description'] = t('The path to the image. ').'<input type="button" value="Browse" 
      onClick="window.open(\'/?q=imce&app=imagemenu|url%40edit-menu-imagepath\', \'\', \'width=760,height=560,resizable=1\')"';
    $form['menu']['mouseover']['#description'] = t('The path to the image. ').'<input type="button" value="Browse" 
      onClick="window.open(\'/?q=imce&app=imagemenu|url%40edit-menu-mouseover\', \'\', \'width=760,height=560,resizable=1\')"';
  }
  $form['menu']['path'] = array(
    '#type' => 'textfield',
    '#title' => t('Path'),
    '#default_value' => $item['path'],
    '#description' => t('The path this menu item links to. This can be an internal Drupal path such as %add-node or an external URL such as %drupal. Enter %front to link to the front page.', array('%front' => '<front>', '%add-node' => 'node/add', '%drupal' => 'http://drupal.org')),
    '#weight' => -7,
  );
  $form['menu']['alt'] = array(
    '#type' => 'textfield',
    '#title' => t('Description'),
    '#default_value' => $item['alt'],
    '#description' => t('The ALT tag for the image if its not displayed.'),
    '#weight' => -6,
  );
  $form['menu']['enabled'] = array(
    '#type' => 'checkbox',
    '#title' => t('Enabled'),
    '#default_value' => ($item['enabled'] ? 1 : 0),
    '#description' => t('Menu items that are not enabled will not be listed in any menu.'),
    '#weight' => -5,
  );
  $form['menu']['expanded'] = array(
    '#type' => 'checkbox',
    '#title' => t('Expanded'),
    '#default_value' => ($item['type'] ? 1 : 0),
    '#description' => t('If selected and this menu item has children, the menu will always appear expanded.'),
    '#weight' => -4,
  );
  $options = imagemenu_parents($mid);
  $basetitle = isset($menu['title']) ? ('<' . $menu['title'] . '>') : '<base>';
  if (!$options) $options = array($mid => $basetitle);
  $form['menu']['pid'] = array(
    '#type' => 'select',
    '#title' => t('Parent item'),
    '#default_value' => $item['pid'],
    '#options' => $options,
    '#attributes' => array('class' => 'menu-title-select'),
    '#weight' => -3,
  );
  $form['menu']['weight'] = array(
    '#type' => 'weight',
    '#title' => t('Weight'),
    '#delta' => 50,
    '#default_value' => $item['weight'],
    '#description' => t('Optional. In the menu, the heavier items will sink and the lighter items will be positioned nearer the top.'),
    '#weight' => -2,
  );
  // We have to find and pass the parent menu for this item to allow
  // comfortable redirects after adding/editing
  if ($type == 'add') {
    $pmid = $mid;
  }
  else {
    $pmid = imagemenu_find_parent($item['mid']);
  }
  $form['menu']['pmid'] = array(
    '#type' => 'hidden',
    '#value' => $pmid,
  );
  $form['submit'] = array('#type' => 'submit', '#value' => t('Save'));

  return $form;
}

/**
 * Validate form values for a menu link being added or edited.
 */
function imagemenu_edit_item_validate($form, &$form_state) {
  $item = &$form_state['values']['menu'];
  // if the path begins with a slash strip it off
  if (substr($item['imagepath'], 0, 1) == '/') {
    $item['imagepath'] = substr($item['imagepath'], 1);
  }
  $check = is_readable($item['imagepath']);
  if (!$check) form_set_error('imagepath', t('File not found.'));
  $check = is_readable($item['mouseover']);
  if (!$check && $item['mouseover']) form_set_error('mouseover', t('File not found.'));
}

/**
 * Process menu and menu item add/edit form submissions.
 */
function imagemenu_edit_item_submit($form, &$form_state) {
  $item = $form_state['values']['menu'];

  if ($item['path'] == 'node') $item['path'] = '<front>';
  if (!$item['mid']) {
    db_query("INSERT INTO {imagemenu} (pid, path, imagepath, mouseover, title, alt, weight, type) VALUES (%d, '%s', '%s', '%s', '%s', '%s', %d, %d)", $item['pid'], $item['path'], $item['imagepath'], $item['mouseover'], $item['title'], $item['alt'], $item['weight'], $item['expanded']);
    drupal_set_message(t('Menu item successfully added.'));
    $t_args = array('%title' => $item['title']);
    watchdog('imagemenu', 'Added new Imagemenu menu item %title', $t_args, WATCHDOG_NOTICE);
  }
  else {
    db_query("UPDATE {imagemenu} SET pid = %d, path = '%s', imagepath = '%s', mouseover = '%s', title = '%s', alt = '%s', weight = %d, type = %d, enabled = %d WHERE mid = %d", $item['pid'], $item['path'], $item['imagepath'], $item['mouseover'], $item['title'], $item['alt'],  $item['weight'], $item['expanded'], $item['enabled'], $item['mid']);
    drupal_set_message(t('Menu item successfully updated.'));
    $t_args = array('%title' => $item['title']);
    watchdog('imagemenu', 'Updated Imagemenu menu item %title', $t_args, WATCHDOG_NOTICE);
  }
  $form_state['redirect'] = 'admin/build/menu/imagemenu-customize/' . $item['pmid'];
}

/**
 * Submit function for the delete button on the menu item editing form.
 */
function imagemenu_item_delete_submit($form, &$form_state) {
  $form_state['redirect'] = 'admin/build/menu/imagemenu/item/'. $form_state['values']['menu']['mid'] .'/delete';
}

/**
 * Build a confirm form for deletion of a single menu link.
 */
function imagemenu_item_delete_form(&$form_state, $item) {
  $form['#item'] = $item;
  $pmid = imagemenu_find_parent($item['mid']);
  return confirm_form($form, t('Are you sure you want to delete the imagemenu item %item?', array('%item' => $item['title'])), 'admin/build/menu/imagemenu-customize/'. $pmid);
}

/**
 * Process menu delete form submissions.
 */
function imagemenu_item_delete_form_submit($form, &$form_state) {
  $item = $form['#item'];
  $pmid = imagemenu_find_parent($item['mid']);
  db_query('DELETE FROM {imagemenu} WHERE mid = %d', $item['mid']);
  $t_args = array('%title' => $item['title']);
  drupal_set_message(t('The imagemenu item %title has been deleted.', $t_args));
  watchdog('menu', 'Deleted imagemenu item %title.', $t_args, WATCHDOG_NOTICE);
  $form_state['redirect'] = 'admin/build/menu/imagemenu-customize/'. $pmid;
}

/**
 * Menu callback; Build the form presenting menu configuration options.
 */
function imagemenu_configure() {
  $form['intro'] = array(
    '#type' => 'item',
    '#value' => t('Administration Screen Settings:'),
  );

  /* $menu_options = imagemenu_base_rows();
  $form['imagemenu_block'] = array(
    '#type' => 'select',
    '#title' => t('Default menu for content'),
    '#default_value' => variable_get('imagemenu_block', '0'),
    '#options' => $menu_options ? $menu_options : array(0 => t('No menus created')),
    '#description' => t('Which menu to display in the imagemenu block.'),
  ); */
  $form['imagemenu_layout'] = array(
    '#type' => 'select',
    '#title' => t('Layout'),
    '#default_value' => variable_get('imagemenu_layout', 'vertical'),
    '#options' => array('vertical' => 'vertical', 'horizontal' => 'horizontal'),
    '#tree' => FALSE,
    '#description' => t('The orientation of the menu.'),
  );
  $form['imagemenu_new_window'] = array(
    '#type' => 'checkbox',
    '#title' => t('Use target="_blank" for new window (not XHTML compliant)'),
    '#default_value' => variable_get('imagemenu_new_window', FALSE),
    '#description' => t('If use target="_blank" is checked imagemenu will insert the HTML target attribute 
      for links set to open in a new window for browsers without Javascript. '),
  );
  $form['imagemenu_active_state'] = array(
    '#type' => 'checkbox',
    '#title' => t('Use rollover image for active menu item.'),
    '#default_value' => variable_get('imagemenu_active_state', FALSE),
  );
  return system_settings_form($form);
}
