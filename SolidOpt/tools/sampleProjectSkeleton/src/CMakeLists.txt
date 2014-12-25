##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

csharp_add_library(
  @PROJECT_NAME@.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  @PROJECT_NAME@.dll
  # Add dependent libraries here
  System.Core.dll
  )
csharp_add_project_meta(
  @PROJECT_NAME@.dll
  "TargetFrameworkVersion" "v4.0"
  )
