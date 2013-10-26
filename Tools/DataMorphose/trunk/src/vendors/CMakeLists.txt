##
## $Id: CMakeLists.txt 912 2013-04-06 00:09:35Z apenev $
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

# Build our vendor drops using MSBuild, CMake, Copy, ...

# NUnit
csharp_add_msbuild_project(${CMAKE_CURRENT_SOURCE_DIR}/NUnit/NUnitCore/core/nunit.core.dll.csproj)
csharp_add_msbuild_project(${CMAKE_CURRENT_SOURCE_DIR}/NUnit/NUnitCore/interfaces/nunit.core.interfaces.dll.csproj)
# NUnit Framework
csharp_add_msbuild_project(${CMAKE_CURRENT_SOURCE_DIR}/NUnit/NUnitFramework/framework/nunit.framework.dll.csproj)
# NUnit console needed by the runner
csharp_add_msbuild_project(${CMAKE_CURRENT_SOURCE_DIR}/NUnit/ConsoleRunner/nunit-console/nunit-console.csproj)
csharp_add_msbuild_project(${CMAKE_CURRENT_SOURCE_DIR}/NUnit/ConsoleRunner/nunit-console-exe/nunit-console.exe.csproj)
# NUnit util needed by the console
csharp_add_msbuild_project(${CMAKE_CURRENT_SOURCE_DIR}/NUnit/ClientUtilities/util/nunit.util.dll.csproj)

# MonoDevelop Dock (binary vendor library)
csharp_add_library_binary(${CMAKE_CURRENT_SOURCE_DIR}/MonoDevelop/MonoDevelop.Dock.dll)

# SolidOpt.Serives
add_subdirectory(SolidOpt.Services)

# SolidV
add_subdirectory(SolidV)
