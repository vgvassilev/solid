csharp_add_library(
  SolidOpt.Services
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )

add_subdirectory(Compatibility)
add_subdirectory(Subsystems)
add_subdirectory(Transformations)
