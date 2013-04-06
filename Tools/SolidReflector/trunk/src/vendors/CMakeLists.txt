##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

# Build our vendor drops using MSBuild, CMake, Copy, ...

# SolidOpt
add_subdirectory(SolidOpt)

# SolidV
csharp_add_msbuild_project(${CMAKE_CURRENT_SOURCE_DIR}/SolidV/src/SolidV.csproj)

# MonoDevelop Dock (binary vendor library)
csharp_add_msbuild_project(${CMAKE_CURRENT_SOURCE_DIR}/MonoDevelop/MonoDevelop.Dock.dll)
