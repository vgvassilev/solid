<?php function phptemplate_menu_item_link($link) {
  if (function_exists('menuclass_to_link')) {
    menuclass_to_link($link);
  }
  return theme_menu_item_link($link);
}