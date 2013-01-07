csharp_add_library(
  SolidOpt.Services.Transformations.MultiModel.ILtoCFG
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.MultiModel.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph.dll
  # Monodevelop 2.8 compatibility
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  # Externals
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.dll
  ###
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
