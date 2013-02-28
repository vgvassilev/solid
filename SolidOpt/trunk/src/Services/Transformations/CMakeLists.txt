##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

csharp_add_library(
  SolidOpt.Services.Transformations
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  SolidOpt.Services.Transformations
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  )

add_subdirectory(CodeModel)
add_subdirectory(Multimodel)
add_subdirectory(Optimizations)
add_subdirectory(test)

