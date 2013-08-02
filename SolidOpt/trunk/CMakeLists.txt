##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

# TODO: Implement it as project(SolidOpt CS), it should need just renaming of
# the existing modules
project(SolidOpt NONE)

cmake_minimum_required(VERSION 2.6.2)

# Tell cmake to look for modules in that path first.
set(CMAKE_MODULE_PATH "${CMAKE_CURRENT_SOURCE_DIR}/cmake")

# Disallow insource build since I never test that
string(COMPARE EQUAL "${CMAKE_CURRENT_SOURCE_DIR}" "${CMAKE_CURRENT_BINARY_DIR}" INSOURCE)
get_filename_component(PARENTDIR ${CMAKE_CURRENT_BINARY_DIR} PATH)
string(COMPARE EQUAL "${CMAKE_CURRENT_SOURCE_DIR}" "${PARENTDIR}" INSOURCESUBDIR)
if(INSOURCE OR INSOURCESUBDIR)
  message(FATAL_ERROR
    "SolidOpt requires an out of source Build. "
    "Please create a separate binary directory and run CMake there.")
endif()

# Define generator properties
set(VS10SLN OFF CACHE BOOL "Force generation of SLN.")

# Find C#
find_package( CSharp REQUIRED )
include( ${CSHARP_USE_FILE} )

# Find MSBuild
find_package( MSBuild REQUIRED )
include( ${MSBuild_USE_FILE} )

# Find Testing Prerequisites
# TODO: Maybe this should be a variable eg. Dev=1...
find_package( TestingPrerequisites )
include( ${TestingPrerequisites_USE_FILE} )

# Setup build locations.
if(NOT CMAKE_RUNTIME_OUTPUT_DIR)
  set(CMAKE_RUNTIME_OUTPUT_DIR ${CMAKE_CURRENT_BINARY_DIR}/bin)
endif()
if(NOT CMAKE_LIBRARY_OUTPUT_DIR)
  set(CMAKE_LIBRARY_OUTPUT_DIR ${CMAKE_CURRENT_BINARY_DIR}/lib)
endif()

# Make sure the nested directory structure exists
file(MAKE_DIRECTORY ${CMAKE_RUNTIME_OUTPUT_DIR})
file(MAKE_DIRECTORY ${CMAKE_LIBRARY_OUTPUT_DIR})

include(Version.cmake)

# If no build type is set it should defaults to debug
if( NOT CMAKE_BUILD_TYPE )
  set( CMAKE_BUILD_TYPE Debug CACHE STRING "Choose a build type: [Debug Release]" FORCE )
endif()

# Set some default build flags.
set(CSBUILDFLAGS "")
set(MSBUILDFLAGS "")

list(APPEND CSBUILDFLAGS "/delaysign+" "/keyfile:${CMAKE_SOURCE_DIR}/public.snk")

# Suppress the warnings coming from the /doc switch. There is no point warning 
# about missing documentation for certain methods. We all know about it :)
list(APPEND CSBUILDFLAGS "/nowarn:1591")

# Set up the compiler flag for Debug/Release mode.
if (CMAKE_BUILD_TYPE STREQUAL "Debug")
  list(APPEND CSBUILDFLAGS "/debug:full" "/debug+" "/optimize-")
  list(APPEND MSBUILDFLAGS "/p:Configuration=Debug")
else()
  list(APPEND CSBUILDFLAGS "/debug-" "/optimize+")
  list(APPEND MSBUILDFLAGS "/p:Configuration=Release")
endif()

#-----------------------------------------------------------------------------
# Configure Dart testing support.  This should be done before any
# MESSAGE(FATAL_ERROR ...) commands are invoked.
INCLUDE(CTest)
ENABLE_TESTING()
IF(BUILD_TESTING)
  SET(BUILDNAME "${BUILDNAME}" CACHE STRING "Name of build on the dashboard")
  MARK_AS_ADVANCED(BUILDNAME)
ENDIF(BUILD_TESTING)

# ...
add_subdirectory(src)
# FIXME:
# This is a bit hackish way, but it adds support for configuring just docs
# it is needed by our online doxygen system.
set(DOXYGEN_INPUT_DIR ${CMAKE_SOURCE_DIR})
add_subdirectory(doc)

# Resolve all target dependencies
csharp_resolve_dependencies()

# Save solution
csharp_save_vs_solution(SolidOpt)

# Display the welcome msg.
file (READ "${CMAKE_MODULE_PATH}/Welcome.txt" WELCOME)
MESSAGE(STATUS ${WELCOME})
