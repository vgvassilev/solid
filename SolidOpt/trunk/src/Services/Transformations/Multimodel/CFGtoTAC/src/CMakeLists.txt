csharp_add_library(
  SolidOpt.Services.Transformations.MultiModel.CFGtoTAC
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.MultiModel.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  )
