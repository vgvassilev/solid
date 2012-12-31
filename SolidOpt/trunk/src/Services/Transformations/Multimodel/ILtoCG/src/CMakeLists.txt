csharp_add_library(
  SolidOpt.Services.Transformations.MultiModel.ILtoCG
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.MultiModel.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.CodeModel.CallGraph.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  # Externals
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.dll
  ###
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
