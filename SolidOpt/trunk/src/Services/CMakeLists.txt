csharp_add_library(
  SolidOpt.Services
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  )
add_subdirectory(Compatibility)
add_subdirectory(Transformations)
