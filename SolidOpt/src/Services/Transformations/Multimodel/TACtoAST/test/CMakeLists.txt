##
## $Id: CMakeLists.txt 993 2013-05-25 17:14:45Z vvassilev $
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

csharp_add_test_library(
  SolidOpt.Services.Transformations.Multimodel.TACtoAST.Test.dll
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  SolidOpt.Services.Transformations.Multimodel.TACtoAST.Test.dll

  # Add dependent libraries here
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.Test.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.Multimodel.TACtoAST.dll
  # Monodevelop 2.8 compatibility
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.Transformations.MultiModel.dll
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/nunit.framework.dll
  )

csharp_add_project_meta(
  SolidOpt.Services.Transformations.Multimodel.TACtoAST.Test.dll
  "TargetFrameworkVersion" "v4.0"
  )

add_subdirectory(TestCases)
