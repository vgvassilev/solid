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
#   CSHARP_ADD_EXECUTABLE( name source_files* ) - Define C# executable with the given name
#   CSHARP_ADD_GUI_EXECUTABLE( name source_files* ) - Define C# gui executable with the given name
#   CSHARP_ADD_LIBRARY( name source_files* ) - Define C# library with the given name
#
#   CSHARP_ADD_DEPENDENCIES( name references* ) - Define C# dependencies for project with the given name. For now dependencies must be defined after library.
#
#   CSHARP_ADD_PROJECT_META( name (key value)* ) - Define C# project meta data. For now dependencies must be defined after library.
#
# Examples:
#   CSHARP_ADD_EXECUTABLE( MyExecutable.exe "Program.cs" )
#   CSHARP_ADD_DEPENDENCIES( MyExecutable.exe "ref1.dll" "ref2.dll" )
#   CSHARP_ADD_PROJECT_META( MyExecutable.exe "TargetFrameworkVersion" "4.0" "Meta2" "Meta2Value" )
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
set_property(GLOBAL PROPERTY target_out_property)
set_property(GLOBAL PROPERTY target_refs_property)
set_property(GLOBAL PROPERTY target_metas_key_property)
set_property(GLOBAL PROPERTY target_metas_value_property)
set_property(GLOBAL PROPERTY target_sources_property)
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
  if ( TARGET ${depends_on} )
    list(FIND target_name ${depends_on} idx)
    if (idx GREATER -1)
      MESSAGE(STATUS "  ->Depends on[Target/Project]: ${depends_on}")
    else ()
      MESSAGE(STATUS "  ->Depends on[Target/Vendor]: ${depends_on}")
    endif ()
    add_dependencies( ${cur_target} ${depends_on} )
  else ( )
    MESSAGE(STATUS "  ->Depends on[External]: ${depends_on}")
  endif ( TARGET ${depends_on} )
endmacro( CSHARP_ADD_DEPENDENCY )

# Private macro
macro( CSHARP_ADD_PROJECT type name )
  set( sources )
  set( sources_dep )

  if( ${type} MATCHES "library" )
    set( output_type "library" )
    set( TYPE_UPCASE "LIBRARY" )
  elseif( ${type} MATCHES "exe" )
    set( output_type "exe" )
    set( TYPE_UPCASE "RUNTIME" )
  elseif( ${type} MATCHES "gui" )
    set( output_type "winexe" )
    set( TYPE_UPCASE "RUNTIME" )
  elseif( ${type} MATCHES "test_library" )
    set( output_type "library" )
    set( TYPE_UPCASE "LIBRARY" )
  endif( ${type} MATCHES "library" )

  # Generate AssemblyInfo.cs
  MESSAGE( STATUS "Generating AssemblyInfo.cs for ${name}" )
  string(SUBSTRING "${SolidOpt_LastDate}" 0 4 SolidOpt_LastYear)
  configure_file(
    ${CMAKE_MODULE_PATH}/AssemblyInfo.cs.in
    ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
    @ONLY
  )

  # Step through each argument
  foreach( it ${ARGN} )
    # Argument is a source file
    if( EXISTS ${it} )
      list( APPEND sources ${it} )
      list( APPEND sources_dep ${it} )
###    elseif( EXISTS ${CMAKE_CURRENT_BINARY_DIR}/${it} )
###      list( APPEND sources ${it} )
###      list( APPEND sources_dep ${it} )
    elseif( ${it} MATCHES "[*]" )
      # We try to expand wildcards
      FILE( GLOB it_glob ${it} )
      list( APPEND sources ${it} )
      list( APPEND sources_dep ${it_glob} )
    endif( )
  endforeach( )
  # Check we have at least one source
  list( LENGTH sources_dep sources_length )
  if ( ${sources_length} LESS 1 )
    MESSAGE( SEND_ERROR "No C# sources were specified for ${type} ${name}" )
  endif ()
  list( SORT sources_dep )

  # Generate project GUID
  find_program(guid_gen NAMES ${CMAKE_RUNTIME_OUTPUT_DIR}/guid.exe)
  if( NOT guid_gen )
    set( guid_src "${CMAKE_RUNTIME_OUTPUT_DIR}/guid.cs" )
    set( guid_gen "${CMAKE_RUNTIME_OUTPUT_DIR}/guid.exe" )
    file(TO_NATIVE_PATH "${guid_src}" guid_src)
    file(TO_NATIVE_PATH "${guid_gen}" guid_gen)
    file(WRITE ${guid_src} "class GUIDGen { static void Main() { System.Console.Write(System.Guid.NewGuid().ToString().ToUpper()); } }" )
    execute_process(
      WORKING_DIRECTORY "${CMAKE_RUNTIME_OUTPUT_DIR}"
      COMMAND ${CSHARP_COMPILER} /t:exe /out:guid.exe /platform:anycpu ${guid_src}
    )
  endif ( )
  execute_process(COMMAND ${CSHARP_INTERPRETER} ${guid_gen} OUTPUT_VARIABLE proj_guid )

  # Save project info in global properties
  set_property(GLOBAL APPEND PROPERTY target_name_property "${name}")
  set_property(GLOBAL APPEND PROPERTY target_type_property "${type}")
  set_property(GLOBAL APPEND PROPERTY target_output_type_property "${output_type}")
  set_property(GLOBAL APPEND PROPERTY target_out_property "${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name}")
  set_property(GLOBAL APPEND PROPERTY target_guid_property "${proj_guid}")
  # The implementation relies on fixed numbering. I.e. every property should be
  # set for each target in order CMAKE and VS sln generation to work. In the
  # case of references and test cases we have to insert sort-of empty property
  # for each target. In the case where the current target has test cases or more
  # references we have to edit the string at that position.
  set_property(GLOBAL APPEND PROPERTY target_refs_property "#System.dll")
  set_property(GLOBAL APPEND PROPERTY target_metas_key_property "#")
  set_property(GLOBAL APPEND PROPERTY target_metas_value_property "#")
  set_property(GLOBAL APPEND PROPERTY target_tests_property "#")
  set_property(GLOBAL APPEND PROPERTY target_test_results_property "#")
  # Replace the ; with # in the list of sources. Thus the list becomes
  # "flattened". This is useful when we append the sources for another target
  # and then it will become a list of target sources.
  # Eg. #target1_src1.cs#target1_src2#...;#target2_src1.cs#target2_src2#...
  string(REPLACE ";" "#" s "${sources}")
  set_property(GLOBAL APPEND PROPERTY target_sources_property "#${s}")
  string(REPLACE ";" "#" sd "${sources_dep}")
  set_property(GLOBAL APPEND PROPERTY target_sources_dep_property "#${sd}")
  set_property(GLOBAL APPEND PROPERTY target_src_dir_property "${CMAKE_CURRENT_SOURCE_DIR}")
  set_property(GLOBAL APPEND PROPERTY target_bin_dir_property "${CMAKE_CURRENT_BINARY_DIR}")
  STRING( REGEX REPLACE "(\\.dll)[^\\.dll]*$" "" name_we ${name} )
  STRING( REGEX REPLACE "(\\.exe)[^\\.exe]*$" "" name_we ${name_we} )
  set_property(GLOBAL APPEND PROPERTY target_proj_file_property "${CMAKE_CURRENT_BINARY_DIR}/${name_we}.csproj")

endmacro( CSHARP_ADD_PROJECT )

# Define dependencies macro
macro( CSHARP_ADD_DEPENDENCIES name )

  set(refs)

  # Step through each argument
  foreach( it ${ARGN} )
    list( APPEND refs ${it} )
  endforeach( )

  # Save project references info in global properties
  get_property(target_name GLOBAL PROPERTY target_name_property)
  get_property(target_refs GLOBAL PROPERTY target_refs_property)
  list(FIND target_name ${name} idx)
  if (idx GREATER -1)
    if (NOT ("${refs}" STREQUAL ""))
      string(REPLACE ";" "#" r "${refs}")
      get_property(target_refs GLOBAL PROPERTY target_refs_property)
      list(GET target_refs ${idx} old_refs)
      list(INSERT target_refs ${idx} "${old_refs}#${r}")
      math(EXPR idx "${idx}+1")
      list(REMOVE_AT target_refs ${idx})
      set_property(GLOBAL PROPERTY target_refs_property "${target_refs}")
    endif()
  else ()
    message(WARNING "Project ${name} was not defined!?")
  endif ()

endmacro( CSHARP_ADD_DEPENDENCIES )

# Resolve dependencies
macro( CSHARP_RESOLVE_DEPENDENCIES )
  # Read global solution info lists
  get_property(target_name GLOBAL PROPERTY target_name_property)
  get_property(target_type GLOBAL PROPERTY target_type_property)
  get_property(target_output_type GLOBAL PROPERTY target_output_type_property)
  get_property(target_out GLOBAL PROPERTY target_out_property)
  get_property(target_sources GLOBAL PROPERTY target_sources_property)
  get_property(target_sources_dep GLOBAL PROPERTY target_sources_dep_property)
  get_property(target_refs GLOBAL PROPERTY target_refs_property)

  # Set up the compiler flag for Debug/Release mode.
  set(BUILD_TYPE "optimize")
  if (CMAKE_BUILD_TYPE STREQUAL "Debug")
    set(BUILD_TYPE "debug")
  endif()

  # Define custom targets and commands
  set( i 0 )
  foreach( name ${target_name} )
    if (TARGET "${name}")
      # Target is already defined
    else()
      list( GET target_type ${i} type )
      list( GET target_output_type ${i} output_type )
      list( GET target_out ${i} out )

      list( GET target_sources ${i} s )
      string(SUBSTRING "${s}" 1 -1 s)
      string(REPLACE "#" ";" sources "${s}")

      list( GET target_sources_dep ${i} sd )
      string(SUBSTRING "${sd}" 1 -1 sd)
      string(REPLACE "#" ";" sources_dep "${sd}")

      list( GET target_refs ${i} r )
      string(REPLACE "#" "#/reference:" r "${r}")
      string(SUBSTRING "${r}" 1 -1 r)
      string(REPLACE "#" ";" refs "${r}")

      # Separate resources
      set(separated_sources)
      set(separated_embd_resources)
      set(separated_copy_resources)
      foreach(s ${sources})
        if ("${s}" MATCHES "\\.(stetic)$")
          # Expand wildcards
          FILE( GLOB s_glob ${s} )
          foreach (res ${s_glob})
            get_filename_component(res_name "${res}" NAME)
            list(APPEND separated_embd_resources "/resource:${res},${res_name}")
          endforeach()
        #elseif ("${s}" MATCHES "\\.(config)$")
        #  list(APPEND separated_copy_resources "${s}")
        else()
          list(APPEND separated_sources "${s}")
        endif()
      endforeach()

      # Add custom target and command
      get_filename_component(out_name "${out}" NAME)
      get_filename_component(out_dir "${out}" PATH)
      MESSAGE( STATUS "Adding C# ${type} ${name}: '${CSHARP_COMPILER} /t:${output_type} /out:${out_name} /platform:${CSHARP_PLATFORM} /${BUILD_TYPE} ${CSHARP_SDK_COMPILER} ${separated_embd_resources} ${separated_sources} ${refs}'" )
      add_custom_command(
        COMMENT "Compiling C# ${type} ${name}: '${CSHARP_COMPILER} /t:${output_type} /out:${out_name} /platform:${CSHARP_PLATFORM} /${BUILD_TYPE} ${CSHARP_SDK_COMPILER} ${separated_embd_resources} ${separated_sources} ${refs}'"
        OUTPUT ${out}
        COMMAND ${CSHARP_COMPILER}
        ARGS /t:${output_type} /out:${out} /platform:${CSHARP_PLATFORM} /${BUILD_TYPE} ${CSHARP_SDK_COMPILER} ${separated_embd_resources} ${separated_sources} ${refs}
        WORKING_DIRECTORY ${out_dir}
        DEPENDS ${sources_dep}
      )
      add_custom_target(
        ${name} ALL
        DEPENDS ${out}
        SOURCES ${sources_dep}
      )
    endif()

    # Add a test if target is Test Library - nunit console test runner
    if (type STREQUAL "test_library")
      MESSAGE(STATUS "Add test case runner for ${name}")
      ADD_TEST("${name}" ${CSHARP_INTERPRETER} "${NUNIT_CONSOLE}" "${out}")
    endif()

    math(EXPR i "${i}+1")
  endforeach( )

  # Resolving all target dependencies
  set( i 0 )
  foreach( name ${target_name} )
    list( GET target_type ${i} type )
    list( GET target_refs ${i} r )
    string(SUBSTRING "${r}" 1 -1 r)
    string(REPLACE "#" ";" refs "${r}")
    MESSAGE( STATUS "Resolving dependencies for ${type}: ${name}" )
    foreach( it ${refs} )
      # Get the filename only (no slashes)
      get_filename_component(filename ${it} NAME)
      csharp_add_dependency(${name} ${filename})
    endforeach( )

    math(EXPR i "${i}+1")
  endforeach( )

endmacro( CSHARP_RESOLVE_DEPENDENCIES )

# Define project meta data macro
macro( CSHARP_ADD_PROJECT_META name )
  set(metas_key)
  set(metas_value)

  set(flag TRUE)

  # Step through each argument
  foreach( it ${ARGN} )
    if (flag)
      list( APPEND metas_key ${it} )
      set(flag FALSE)
    else()
      list( APPEND metas_value ${it} )
      set(flag TRUE)
    endif()
  endforeach( )

  # Save project meta data in global properties
  get_property(target_name GLOBAL PROPERTY target_name_property)
  get_property(target_metas_key GLOBAL PROPERTY target_metas_key_property)
  get_property(target_metas_value GLOBAL PROPERTY target_metas_value_property)
  list(FIND target_name ${name} idx)
  if (idx GREATER -1)
    if (NOT ("${metas_key}" STREQUAL ""))
      string(REPLACE ";" "#" mk "${metas_key}")
      get_property(target_metas_key GLOBAL PROPERTY target_metas_key_property)
      list(GET target_metas_key ${idx} old_metas_key)
      list(INSERT target_metas_key ${idx} "${old_metas_key}#${mk}")
      string(REPLACE ";" "#" mv "${metas_value}")
      get_property(target_metas_value GLOBAL PROPERTY target_metas_value_property)
      list(GET target_metas_value ${idx} old_metas_value)
      list(INSERT target_metas_value ${idx} "${old_metas_value}#${mv}")
      math(EXPR idx "${idx}+1")
      list(REMOVE_AT target_metas_key ${idx})
      set_property(GLOBAL PROPERTY target_metas_key_property "${target_metas_key}")
      list(REMOVE_AT target_metas_value ${idx})
      set_property(GLOBAL PROPERTY target_metas_value_property "${target_metas_value}")
    endif()
  else ()
    message(WARNING "Project ${name} was not defined!?")
  endif ()

endmacro( CSHARP_ADD_PROJECT_META )
