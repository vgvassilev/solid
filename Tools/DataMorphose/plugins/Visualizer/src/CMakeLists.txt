csharp_add_library(
  DataMorphose.Plugins.Visualizer.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  DataMorphose.Plugins.Visualizer.dll
  ${CMAKE_RUNTIME_OUTPUT_DIR}/DataMorphose.exe
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/MonoDevelop.Dock.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidV.dll
  # Packages
  "pkg:gtk-sharp-2.0"
  Mono.Cairo.dll
  )
