csharp_add_library(
  SolidV.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs

  ${CMAKE_CURRENT_SOURCE_DIR}/Cairo/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Gtk/ObjectInspector/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/MVC/Controller/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/MVC/Controller/VisualCodeControllers/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/MVC/Model/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/MVC/Model/VisualCodeModels/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/MVC/View/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/MVC/View/VisualCodeViewers/*.cs
  )
csharp_add_dependencies(
  SolidV.dll
  # External
  Mono.Cairo.dll
  # GAC
  "gtk-sharp.dll"
  "gdk-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f"
  "gdk-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f"
  "glib-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f"
  "glade-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f"
  "pango-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f"
  "atk-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f"
  )
csharp_add_project_meta(
  SolidV.dll
  )

execute_process(
  COMMAND ${CMAKE_COMMAND} -E create_symlink ../lib ${CMAKE_RUNTIME_OUTPUT_DIR}/bin/Debug
  WORKING_DIRECTORY ${CMAKE_RUNTIME_OUTPUT_DIR}
)
