
csharp_add_library(
  SolidOpt.Services.Transformations
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  )

add_subdirectory(Optimizations)
