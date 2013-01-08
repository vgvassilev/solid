csharp_add_library(
  SolidOpt.Services.Subsystems.HetAccess
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Exporters/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Importers/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
