##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

csharp_add_library(
  ControlFlowDomain.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  ControlFlowDomain.dll
  ${CMAKE_RUNTIME_OUTPUT_DIR}/SolidIDE.exe
  ${CMAKE_RUNTIME_OUTPUT_DIR}/plugins/Toolbox.dll
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidV.dll
  # External
  Mono.Cairo.dll
  # GAC
  pkg:gtk-sharp-2.0
  )
