##
## $Id: CMakeLists.txt 912 2013-04-06 00:09:35Z apenev $
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

# Build our vendor drops using MSBuild, CMake, Copy, ...

# SolidOpt
add_subdirectory(SolidOpt.Services)

# MonoDevelop Dock (binary vendor library)
csharp_add_msbuild_project(${CMAKE_CURRENT_SOURCE_DIR}/MonoDevelop/MonoDevelop.Dock.dll)
