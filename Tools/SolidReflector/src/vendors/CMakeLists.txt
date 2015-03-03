##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

# Build our vendor drops using MSBuild, CMake, Copy, ...

# NUnit
#TODO: Fix vendors
#TODO: Remove hard dependency to SolidOpt vendors
csharp_add_msbuild_project("${CMAKE_CURRENT_SOURCE_DIR}/NUnit/NUnitCore/core/nunit.core.dll.csproj" "${CMAKE_CURRENT_SOURCE_DIR}/../../../../SolidOpt/src/vendors/NUnit/NUnitCore/core/nunit.core.dll.csproj")
csharp_add_msbuild_project("${CMAKE_CURRENT_SOURCE_DIR}/NUnit/NUnitCore/interfaces/nunit.core.interfaces.dll.csproj" "${CMAKE_CURRENT_SOURCE_DIR}/../../../../SolidOpt/src/vendors/NUnit/NUnitCore/interfaces/nunit.core.interfaces.dll.csproj")
# NUnit Framework
csharp_add_msbuild_project("${CMAKE_CURRENT_SOURCE_DIR}/NUnit/NUnitFramework/framework/nunit.framework.dll.csproj" "${CMAKE_CURRENT_SOURCE_DIR}/../../../../SolidOpt/src/vendors/NUnit/NUnitFramework/framework/nunit.framework.dll.csproj")
# NUnit console needed by the runner
csharp_add_msbuild_project("${CMAKE_CURRENT_SOURCE_DIR}/NUnit/ConsoleRunner/nunit-console/nunit-console.csproj" "${CMAKE_CURRENT_SOURCE_DIR}/../../../../SolidOpt/src/vendors/NUnit/ConsoleRunner/nunit-console/nunit-console.csproj")
csharp_add_msbuild_project("${CMAKE_CURRENT_SOURCE_DIR}/NUnit/ConsoleRunner/nunit-console-exe/nunit-console.exe.csproj" "${CMAKE_CURRENT_SOURCE_DIR}/../../../../SolidOpt/src/vendors/NUnit/ConsoleRunner/nunit-console-exe/nunit-console.exe.csproj")
# NUnit util needed by the console
csharp_add_msbuild_project("${CMAKE_CURRENT_SOURCE_DIR}/NUnit/ClientUtilities/util/nunit.util.dll.csproj" "${CMAKE_CURRENT_SOURCE_DIR}/../../../../SolidOpt/src/vendors/NUnit/ClientUtilities/util/nunit.util.dll.csproj")


# SolidOpt
#TODO: Fix vendors
add_vendor(SolidOpt ../../../../SolidOpt)

# SolidV
#TODO: Fix vendors
add_vendor(SolidV ../../../../SolidV)
