##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

# Build our vendor drops using MSBuild
# Mono Cecil
csharp_add_msbuild_project(${CMAKE_CURRENT_SOURCE_DIR}/Mono.Cecil/Mono.Cecil.csproj)
# Mono Cecil Rocks
csharp_add_msbuild_project(${CMAKE_CURRENT_SOURCE_DIR}/Mono.Cecil/rocks/Mono.Cecil.Rocks.csproj)
# Cecil Decompiler
csharp_add_msbuild_project(${CMAKE_CURRENT_SOURCE_DIR}/Cecil.Decompiler/Cecil.Decompiler/Cecil.Decompiler.csproj)
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
