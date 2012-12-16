
csharp_add_library(
  SolidOpt.Services.Transformations.Optimizations
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  )

add_subdirectory(AST)
