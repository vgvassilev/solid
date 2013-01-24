##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

## This file is based on the work of SimpleITK:
##   https://github.com/SimpleITK/SimpleITK/blob/master/CMake/UseCSharp.cmake

# A CMake Module for finding and using C# (.NET and Mono).
#
# The following variables are set:
#   CSHARP_TYPE - the type of the C# compiler (eg. ".NET" or "Mono")
#   CSHARP_COMPILER - the path to the C# compiler executable (eg. "C:/Windows/Microsoft.NET/Framework/v4.0.30319/csc.exe")
#   CSHARP_VERSION - the version number of the C# compiler (eg. "v4.0.30319")
#
# The following macros are defined:
#   CSHARP_ADD_EXECUTABLE( name references [(ref_files | source_files)*] ) - Define C# executable with the given name
#   CSHARP_ADD_GUI_EXECUTABLE( name references [(ref_files | source_files)*] ) - Define C# gui executable with the given name
#   CSHARP_ADD_LIBRARY( name references [(ref_files | source_files)*] ) - Define C# library with the given name
#
# Examples:
#   CSHARP_ADD_EXECUTABLE( MyExecutable "" "Program.cs" )
#   CSHARP_ADD_EXECUTABLE( MyExecutable "ref1.dll ref2.dll" "Program.cs File1.cs" )
#   CSHARP_ADD_EXECUTABLE( MyExecutable "ref1.dll;ref2.dll" "Program.cs;File1.cs" )
#

# Check something was found
if( NOT CSHARP_COMPILER )
  message( WARNING "A C# compiler executable was not found on your system" )
endif( NOT CSHARP_COMPILER )

# Include type-based USE_FILE
if( CSHARP_TYPE MATCHES ".NET" )
  include( ${DotNetFrameworkSdk_USE_FILE} )
elseif ( CSHARP_TYPE MATCHES "Mono" )
  include( ${Mono_USE_FILE} )
endif ( CSHARP_TYPE MATCHES ".NET" )

include(VisualStudioGenerator)

# Init global solution info lists
set_property(GLOBAL PROPERTY target_guid_property)
set_property(GLOBAL PROPERTY target_proj_file_property)
set_property(GLOBAL PROPERTY target_name_property)
set_property(GLOBAL PROPERTY target_type_property)
set_property(GLOBAL PROPERTY target_output_type_property)
set_property(GLOBAL PROPERTY target_refs_property)
set_property(GLOBAL PROPERTY target_sources_dep_property)
set_property(GLOBAL PROPERTY target_src_dir_property)
set_property(GLOBAL PROPERTY target_bin_dir_property)

# Macros

macro( CSHARP_ADD_TEST_LIBRARY name )
  CSHARP_ADD_PROJECT( "test_library" ${name} ${ARGN} )
endmacro( CSHARP_ADD_TEST_LIBRARY )

macro( CSHARP_ADD_LIBRARY name )
  CSHARP_ADD_PROJECT( "library" ${name} ${ARGN} )
endmacro( CSHARP_ADD_LIBRARY )

macro( CSHARP_ADD_EXECUTABLE name )
  CSHARP_ADD_PROJECT( "exe" ${name} ${ARGN} )
endmacro( CSHARP_ADD_EXECUTABLE )

macro( CSHARP_ADD_GUI_EXECUTABLE name )
  CSHARP_ADD_PROJECT( "gui" ${name} ${ARGN} )
endmacro( CSHARP_ADD_GUI_EXECUTABLE )

macro( CSHARP_ADD_DEPENDENCY cur_target depends_on )
  # For now we assume all dependencies are libs
  # The targets doesn't contain the file extension.
  # If we have an extension we have to strip it
  STRING( REGEX REPLACE "(\\.dll)[^\\.dll]*$" "" cur_target_we ${cur_target} )
  STRING( REGEX REPLACE "(\\.dll)[^\\.dll]*$" "" depends_on_we ${depends_on} )

  if ( TARGET ${depends_on_we} )
    list(FIND target_name ${depends_on_we} idx)
    if (idx GREATER -1)
      MESSAGE(STATUS "  ->Depends on[Target/Project]: ${depends_on_we}")
    else ()
      MESSAGE(STATUS "  ->Depends on[Target/Vendor]: ${depends_on_we}")
    endif ()
    add_dependencies( ${cur_target_we} ${depends_on_we} )
  else ( )
    MESSAGE(STATUS "  ->Depends on[External]: ${depends_on}")
  endif ( TARGET ${depends_on_we} )
endmacro( CSHARP_ADD_DEPENDENCY )

# Private macro
macro( CSHARP_ADD_PROJECT type name )
  # Generate AssemblyInfo.cs
  MESSAGE( STATUS "Generating AssemblyInfo.cs for ${name}" )
  string(SUBSTRING "${SolidOpt_LastDate}" 0 4 SolidOpt_LastYear)
  configure_file(
    ${CMAKE_MODULE_PATH}/AssemblyInfo.cs.in
    ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
    @ONLY
  )

  set( refs "/reference:System.dll" )
  set( depRefs "System.dll" )
  set( sources )
  set( sources_dep )

  if( ${type} MATCHES "library" )
    set( output "dll" )
    set( output_type "library" )
    set( TYPE_UPCASE "LIBRARY" )
  elseif( ${type} MATCHES "exe" )
    set( output "exe" )
    set( output_type "exe" )
    set( TYPE_UPCASE "RUNTIME" )
  elseif( ${type} MATCHES "gui" )
    set( output "exe" )
    set( output_type "winexe" )
    set( TYPE_UPCASE "RUNTIME" )
  elseif( ${type} MATCHES "test_library" )
    set( output "dll" )
    set( output_type "library" )
    set( TYPE_UPCASE "LIBRARY" )
  endif( ${type} MATCHES "library" )

  # Step through each argument
  foreach( it ${ARGN} )
    if( ${it} MATCHES "(.*)(dll)" )
       # Argument is a dll, add reference
       list( APPEND refs /reference:${it} )
       list( APPEND depRefs ${it} )
    else( )
      # Argument is a source file
      if( EXISTS ${it} )
        list( APPEND sources ${it} )
        list( APPEND sources_dep ${it} )
      elseif( EXISTS ${CMAKE_CURRENT_BINARY_DIR}/${it} )
        list( APPEND sources ${it} )
        list( APPEND sources_dep ${it} )
      elseif( ${it} MATCHES "[*]" )
        # For dependencies, we need to expand wildcards
        FILE( GLOB it_glob ${it} )
        list( APPEND sources ${it} )
        list( APPEND sources_dep ${it_glob} )
      endif( )
    endif ( )
  endforeach( )
  # Check we have at least one source
  list( LENGTH sources_dep sources_length )
  if ( ${sources_length} LESS 1 )
    MESSAGE( SEND_ERROR "No C# sources were specified for ${type} ${name}" )
  endif ()
  list( SORT sources_dep )

  # Set up the compiler flag for Debug/Release mode.
  set(BUILD_TYPE "optimize")
  if (CMAKE_BUILD_TYPE STREQUAL "Debug")
    set(BUILD_TYPE "debug")
  endif()

  # Add custom target and command
  MESSAGE( STATUS "Adding C# ${type} ${name}: '${CSHARP_COMPILER} /t:${output_type} /out:${name}.${output} /platform:${CSHARP_PLATFORM} /${BUILD_TYPE} ${CSHARP_SDK_COMPILER} ${refs} ${sources}'" )
  add_custom_command(
    COMMENT "Compiling C# ${type} ${name}: '${CSHARP_COMPILER} /t:${output_type} /out:${name}.${output} /platform:${CSHARP_PLATFORM} /${BUILD_TYPE} ${CSHARP_SDK_COMPILER} ${refs} ${sources}'"
    OUTPUT ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name}.${output}
    COMMAND ${CSHARP_COMPILER}
    ARGS /t:${output_type} /out:${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name}.${output} /platform:${CSHARP_PLATFORM} /${BUILD_TYPE} ${CSHARP_SDK_COMPILER} ${refs} ${sources}
    WORKING_DIRECTORY ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}
    DEPENDS ${sources_dep}
  )
  add_custom_target(
    ${name} ALL
    DEPENDS ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name}.${output}
    SOURCES ${sources_dep}
  )

  # Generate project GUID
  find_program(guid_gen NAMES ${CMAKE_RUNTIME_OUTPUT_DIR}/guid.exe)
  if( NOT guid_gen )
    set( guid_src "${CMAKE_RUNTIME_OUTPUT_DIR}/guid.cs" )
    set( guid_gen "${CMAKE_RUNTIME_OUTPUT_DIR}/guid.exe" )
    file(TO_NATIVE_PATH "${guid_src}" guid_src)
    file(TO_NATIVE_PATH "${guid_gen}" guid_gen)
    file(WRITE ${guid_src} "class GUIDGen { static void Main() { System.Console.Write(System.Guid.NewGuid().ToString().ToUpper()); } }" )
    execute_process(
      COMMAND ${CSHARP_COMPILER} /t:exe /out:${guid_gen} /platform:anycpu ${guid_src}
    )
  endif ( )
  execute_process(COMMAND ${CSHARP_INTERPRETER} ${guid_gen} OUTPUT_VARIABLE proj_guid )

  # Save project info in global properties
  set_property(GLOBAL APPEND PROPERTY target_name_property "${name}")
  set_property(GLOBAL APPEND PROPERTY target_type_property "${type}")
  set_property(GLOBAL APPEND PROPERTY target_output_type_property "${output_type}")
  set_property(GLOBAL APPEND PROPERTY target_guid_property "${proj_guid}")
  string(REPLACE ";" "#" r "${depRefs}")
  set_property(GLOBAL APPEND PROPERTY target_refs_property "#${r}")
  string(REPLACE ";" "#" sd "${sources_dep}")
  set_property(GLOBAL APPEND PROPERTY target_sources_dep_property "#${sd}")
  set_property(GLOBAL APPEND PROPERTY target_src_dir_property "${CMAKE_CURRENT_SOURCE_DIR}")
  set_property(GLOBAL APPEND PROPERTY target_bin_dir_property "${CMAKE_CURRENT_BINARY_DIR}")
  set_property(GLOBAL APPEND PROPERTY target_proj_file_property "${CMAKE_CURRENT_BINARY_DIR}/${name}.csproj")

endmacro( CSHARP_ADD_PROJECT )

# Resolve dependencies
macro( CSHARP_RESOLVE_DEPENDENCIES )
  # Read global solution info lists
  get_property(target_name GLOBAL PROPERTY target_name_property)
  get_property(target_type GLOBAL PROPERTY target_type_property)
  get_property(target_refs GLOBAL PROPERTY target_refs_property)

  set( i 0 )
  foreach( name ${target_name} )
    list( GET target_type ${i} type )
    list( GET target_refs ${i} r )
    string(SUBSTRING "${r}" 1 -1 r)
    string(REPLACE "#" ";" refs "${r}")
    MESSAGE( STATUS "Resolving dependencies for ${type}: ${name}" )
    foreach( it ${refs} )
#      # Argument is a dll, add as dependency. csharp_add_dependency will decide if
#      # if it was a build target or not.
#      if( ${it} MATCHES "(.*)(dll)" )
        # Get the filename only (no slashes)
        get_filename_component(filename ${it} NAME)
        csharp_add_dependency(${name} ${filename})
#      endif( )
    endforeach( )

    math(EXPR i "${i}+1")
  endforeach( )

endmacro( CSHARP_RESOLVE_DEPENDENCIES )
