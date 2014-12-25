##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

# TODO: Implement it as project(DataMorphose CS), it should need just renaming of
# the existing modules
project(DataMorphose NONE)

cmake_minimum_required(VERSION 2.6.2)

set(PUBLIC_KEY "${CMAKE_SOURCE_DIR}/public.snk")

# Tell cmake to look for modules in that path first.
set(CMAKE_MODULE_PATH "${CMAKE_CURRENT_SOURCE_DIR}/cmake")
if(NOT EXISTS CMAKE_MODULE_PATH)
  ### TODO: Remove hard dependency to SolidOpt
  set(CMAKE_MODULE_PATH "../../SolidOpt/cmake")
endif()
get_filename_component(CMAKE_MODULE_PATH "${CMAKE_MODULE_PATH}" ABSOLUTE)

# Disallow insource build since I never test that
string(COMPARE EQUAL "${CMAKE_CURRENT_SOURCE_DIR}" "${CMAKE_CURRENT_BINARY_DIR}" INSOURCE)
get_filename_component(PARENTDIR ${CMAKE_CURRENT_BINARY_DIR} PATH)
string(COMPARE EQUAL "${CMAKE_CURRENT_SOURCE_DIR}" "${PARENTDIR}" INSOURCESUBDIR)
if(INSOURCE OR INSOURCESUBDIR)
  message(FATAL_ERROR
    "DataMorphose requires an out of source Build. "
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

# Force minimal sdk and framework versions (overrides the default ones from 2.0/2)
set( CSHARP_FRAMEWORK_VERSION "4.0" CACHE STRING "C# .NET SDK version (e.g. 1.0, 1.1, 2.0, 3.0, 3.5, 4.0, 4.5, ...)" FORCE )
set( CSHARP_SDK_COMPILER "/sdk:4" CACHE STRING "C# Mono SDK commandline switch (e.g. /sdk:2, /sdk:4, /sdk:5)" FORCE )

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

include(${CMAKE_MODULE_PATH}/Version.cmake)
include(${CMAKE_MODULE_PATH}/Vendors.cmake)

# If no build type is set it should defaults to debug
if( NOT CMAKE_BUILD_TYPE )
  set( CMAKE_BUILD_TYPE Debug CACHE STRING "Choose a build type: [Debug Release]" FORCE )
endif()

# Set some default build flags.
set(CSBUILDFLAGS "")
set(MSBUILDFLAGS "")

# Set up the compiler flag for Debug/Release mode.
if (CMAKE_BUILD_TYPE STREQUAL "Debug")
  list(APPEND CSBUILDFLAGS "/debug:full" "/debug+" "/optimize-")
  list(APPEND MSBUILDFLAGS "/p:Configuration=Debug")
else()
  list(APPEND CSBUILDFLAGS "/debug-" "/optimize+")
  list(APPEND MSBUILDFLAGS "/p:Configuration=Release")
endif()

# Define whether to use assembly deleyed signing or not
set(DELAY_SIGN OFF CACHE BOOL "Enable delayed assembly signing.")
if(DELAY_SIGN)
  # The if-stmt converts On Y, YES to bool and from here on we want only a real
  # true or false.
  set(DELAY_SIGN "True") 
  list(APPEND CSBUILDFLAGS "/delaysign+" "/keyfile:${CMAKE_SOURCE_DIR}/public.snk")
else()
  set(DELAY_SIGN "False")
endif(DELAY_SIGN)

# Suppress the warnings coming from the /doc switch. There is no point warning 
# about missing documentation for certain methods. We all know about it :)
list(APPEND CSBUILDFLAGS "/nowarn:1591")

# Define whether to use assembly deleyed signing or not
set(DELAY_SIGN OFF CACHE BOOL "Enable delayed assembly signing.")
if(DELAY_SIGN)
  # The if-stmt converts On Y, YES to bool and from here on we want only a real
  # true or false.
  set(DELAY_SIGN "True") 
  list(APPEND CSBUILDFLAGS "/delaysign+" "/keyfile:${CMAKE_SOURCE_DIR}/public.snk")
else()
  set(DELAY_SIGN "False")
endif(DELAY_SIGN)

#-----------------------------------------------------------------------------
# Configure Dart testing support.  This should be done before any
# MESSAGE(FATAL_ERROR ...) commands are invoked.
INCLUDE(CTest)
ENABLE_TESTING()
IF(BUILD_TESTING)
  SET(BUILDNAME "${BUILDNAME}" CACHE STRING "Name of build on the dashboard")
  MARK_AS_ADVANCED(BUILDNAME)
ENDIF(BUILD_TESTING)

# The lock is used to avoid regeneration of the sln-s and csproj-s if the
# build system is steered by another one. For example if SolidOpt is embedded
# as a vendor to say SolidReflector. 
get_property(SLN_GEN_LOCK GLOBAL PROPERTY SLN_GEN_LOCK_PROPERTY)
if(NOT SLN_GEN_LOCK)
  set_property(GLOBAL PROPERTY SLN_GEN_LOCK_PROPERTY "DataMorphose")
endif()

# ...
add_subdirectory(${CMAKE_MODULE_PATH}/BuildInformation cmake/BuildInformation)
add_subdirectory(src)
add_subdirectory(plugins)
# FIXME:
# This is a bit hackish way, but it adds support for configuring just docs
# it is needed by our online doxygen system.
set(DOXYGEN_INPUT_DIR ${CMAKE_SOURCE_DIR})
add_subdirectory(doc)

# Resolve all target dependencies
csharp_resolve_dependencies()

get_property(SLN_GEN_LOCK GLOBAL PROPERTY SLN_GEN_LOCK_PROPERTY)
if (SLN_GEN_LOCK STREQUAL "DataMorphose")
  # Save solution
  csharp_save_vs_solution(DataMorphose)
endif()

#Configure the setenv script.
configure_file(
  ${CMAKE_CURRENT_SOURCE_DIR}/setenv.sh.in
  ${PROJECT_BINARY_DIR}/setenv.sh
  @ONLY
  )

#Give the user some instructions how to proceed.
MESSAGE(STATUS "==============================================================")
MESSAGE(STATUS "==           Please run: source setenv.sh && make           ==") 
MESSAGE(STATUS "==============================================================")
