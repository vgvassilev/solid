csharp_add_dependency("../SolidOpt.Services")

csharp_add_library(
  SolidOpt.Services.Transformations
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  )
