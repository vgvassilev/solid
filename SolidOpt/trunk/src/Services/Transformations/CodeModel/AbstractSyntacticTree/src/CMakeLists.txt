
csharp_add_library(
  SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Cecil.Decompiler.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  )
