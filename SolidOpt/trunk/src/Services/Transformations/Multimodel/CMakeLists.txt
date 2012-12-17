
csharp_add_library(
  SolidOpt.Services.Transformations.MultiModel
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  )
