csharp_add_library(
  DataMorphose.Plugins.ImportExport.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Import/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Export/*.cs
  )
csharp_add_dependencies(
  DataMorphose.Plugins.ImportExport.dll
  ${CMAKE_RUNTIME_OUTPUT_DIR}/DataMorphose.exe
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/MonoDevelop.Dock.dll
  # GAC
  gtk-sharp.dll
  gdk-sharp.dll
  )
