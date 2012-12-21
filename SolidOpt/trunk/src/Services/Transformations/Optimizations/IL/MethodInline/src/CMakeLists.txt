csharp_add_library(
  SolidOpt.Services.Transformations.Optimizations.IL.MethodInline
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.Optimizations.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.Optimizations.Annotations.dll
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.Rocks.dll
  ###
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  )
