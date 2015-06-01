##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

csharp_add_library(
  Timeline.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  Timeline.dll
  ${CMAKE_RUNTIME_OUTPUT_DIR}/SolidIDE.exe
  ${CMAKE_RUNTIME_OUTPUT_DIR}/plugins/Designer.dll
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidV.dll
  # External
  Mono.Cairo.dll
  #Mono.Posix.dll
  # GAC
  pkg:gtk-sharp-2.0
  )
