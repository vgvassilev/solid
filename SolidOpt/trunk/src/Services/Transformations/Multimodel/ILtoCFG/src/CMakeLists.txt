csharp_add_library(
  SolidOpt.Services.Transformations.MultiModel.ILtoCFG
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.MultiModel.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  )
