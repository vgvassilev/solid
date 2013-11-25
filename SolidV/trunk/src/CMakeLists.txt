csharp_add_library(
  SolidV.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs

  ${CMAKE_CURRENT_SOURCE_DIR}/Cairo/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Gtk/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Gdk/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Ide/DockLibrary/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Ide/ObjectInspector/*.cs   
  ${CMAKE_CURRENT_SOURCE_DIR}/Ide/ObjectInspector.Editors/*.cs
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
  Mono.Posix.dll
  System.Xml.dll
  # Packages
  "pkg:gtk-sharp-2.0"
  System.Drawing.dll
  )
csharp_add_project_meta(
  SolidV.dll
  )

execute_process(
  COMMAND ${CMAKE_COMMAND} -E create_symlink ../lib ${CMAKE_RUNTIME_OUTPUT_DIR}/bin/Debug
  WORKING_DIRECTORY ${CMAKE_RUNTIME_OUTPUT_DIR}
)
