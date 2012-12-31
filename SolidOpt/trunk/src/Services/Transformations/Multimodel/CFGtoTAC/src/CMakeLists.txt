csharp_add_library(
  SolidOpt.Services.Transformations.MultiModel.CFGtoTAC
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.MultiModel.dll
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.dll
  ###
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
