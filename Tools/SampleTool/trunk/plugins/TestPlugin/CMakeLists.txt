csharp_add_library(
  TestPlugin.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  TestPlugin.dll
  ${CMAKE_RUNTIME_OUTPUT_DIR}/SampleTool.exe
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.MultiModel.ILtoCFG.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/MonoDevelop.Dock.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidV.dll
  # GAC
  Mono.Cairo.dll
  gtk-sharp.dll
  gdk-sharp.dll
  )
