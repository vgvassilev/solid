##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

csharp_add_library(
  AssemblyBrowser.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  AssemblyBrowser.dll
  ${CMAKE_RUNTIME_OUTPUT_DIR}/SolidReflector.exe
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/MonoDevelop.Dock.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/Mono.Cecil.dll
  # Packages
  "pkg:gtk-sharp-2.0"
  )
