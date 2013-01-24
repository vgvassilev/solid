csharp_add_library(
  SolidOpt.Loader
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
