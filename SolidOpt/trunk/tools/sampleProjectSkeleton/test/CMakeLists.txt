##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

csharp_add_test_library(
  @PROJECT_NAME@.Test.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  @PROJECT_NAME@.Test.dll
  # Add dependent libraries here
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.Test.dll
  )

csharp_add_project_meta(
  @PROJECT_NAME@.Test.dll
  "TargetFrameworkVersion" "v4.0"
  )

add_subdirectory(TestCases)
