csharp_add_library(
  SolidOpt.Services.Transformations.Optimizations.IL.MethodInline
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.Optimizations.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.Optimizations.Annotations.dll
  # Monodevelop 2.8 compatibility
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.Rocks.dll
  ###
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
