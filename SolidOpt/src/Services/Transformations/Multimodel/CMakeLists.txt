##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

csharp_add_library(
  SolidOpt.Services.Transformations.MultiModel.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  SolidOpt.Services.Transformations.MultiModel.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.dll
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/nunit.core.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/nunit.core.interfaces.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/nunit.framework.dll
  )

add_subdirectory(ASTtoIL)
add_subdirectory(CFGtoIL)
add_subdirectory(ILtoTAC)
add_subdirectory(ILtoAST)
add_subdirectory(ILtoCFG)
add_subdirectory(ILtoCG)
add_subdirectory(TACtoAST)
