
csharp_add_library(
  SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  )
