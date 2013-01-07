csharp_add_library(
  SolidOpt.Services.Transformations.Optimizations.AST.SimplifyExpression
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.Optimizations.dll
  # Monodevelop 2.8 compatibility
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Cecil.Decompiler.dll
  ###
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
