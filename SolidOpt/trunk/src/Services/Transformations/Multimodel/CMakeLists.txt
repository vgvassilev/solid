
csharp_add_library(
  SolidOpt.Services.Transformations.MultiModel
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Compatibility.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/nunit.core.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/nunit.core.interfaces.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/nunit.framework.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  )
