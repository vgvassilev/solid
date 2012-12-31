csharp_add_library(
  SolidOpt.Services.Transformations.Optimizations
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )

add_subdirectory(SolidOpt.Services.Optimizations.Annotations) # Many depend on that
add_subdirectory(AST)
add_subdirectory(CFG)
add_subdirectory(IL)
