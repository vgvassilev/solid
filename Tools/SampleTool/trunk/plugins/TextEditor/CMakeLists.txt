csharp_add_library(
  TextEditor.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  TextEditor.dll
  ${CMAKE_RUNTIME_OUTPUT_DIR}/SampleTool.exe
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidV.dll
  # GAC
  gtk-sharp.dll
  gdk-sharp.dll
  )
