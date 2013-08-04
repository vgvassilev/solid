##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

csharp_add_test_library(
  DataMorphose.Plugins.Visualizer.Test.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  DataMorphose.Plugins.Visualizer.Test.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/DataMorphose.Plugins.Visualizer.dll
  ${CMAKE_RUNTIME_OUTPUT_DIR}/DataMorphose.exe
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.BuildInformation.dll
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/nunit.framework.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/nunit.core.dll
  )

csharp_add_project_meta(
  DataMorphose.Plugins.Visualizer.Test.dll
  "TargetFrameworkVersion" "v4.0"
  )

#add_subdirectory(TestCases)
