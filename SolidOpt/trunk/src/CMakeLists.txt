##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

add_subdirectory(vendors)
add_subdirectory(Services)

#add all-in-one C# project
csharp_add_library(
  SolidOpt.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/GUI/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/src/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Subsystems/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Subsystems/Cache/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Subsystems/Cache/src/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Subsystems/Configurator/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Subsystems/Configurator/src/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Subsystems/Configurator/src/Mappers/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Subsystems/Configurator/src/Sources/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Subsystems/Configurator/src/Sources/Generators/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Subsystems/Configurator/src/Targets/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Subsystems/Configurator/src/TypeResolvers/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Subsystems/HetAccess/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Subsystems/HetAccess/src/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Subsystems/HetAccess/src/Exporters/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Subsystems/HetAccess/src/Importers/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/CodeModel/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/CodeModel/ControlFlowGraph/src/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/CodeModel/AbstractSyntacticTree/src/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/CodeModel/CallGraph/src/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/CodeModel/ThreeAddressCode/src/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/Multimodel/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/Multimodel/ASTtoIL/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/Multimodel/ASTtoIL/src/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/Multimodel/CFGtoIL/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/Multimodel/CFGtoIL/src/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/Multimodel/CFGtoTAC/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/Multimodel/CFGtoTAC/src/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/Multimodel/ILtoAST/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/Multimodel/ILtoAST/src/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/Multimodel/ILtoCFG/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/Multimodel/ILtoCFG/src/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/Multimodel/ILtoCG/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Services/Transformations/Multimodel/ILtoCG/src/*.cs
)

csharp_add_dependencies(
  SolidOpt.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Cecil.Decompiler.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/nunit.framework.dll
  System.Data
  System.Web
)

csharp_add_project_meta(
  SolidOpt.dll
  "TargetFrameworkVersion" "v4.0"
)
