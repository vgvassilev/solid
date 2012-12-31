# A CMake Module for finding and using C# (.NET and Mono).
#
# The following variables are set:
#   CSHARP_TYPE - the type of the C# compiler (eg. ".NET" or "Mono")
#   CSHARP_COMPILER - the path to the C# compiler executable (eg. "C:/Windows/Microsoft.NET/Framework/v4.0.30319/csc.exe")
#   CSHARP_VERSION - the version number of the C# compiler (eg. "v4.0.30319")
#
# The following macros are defined:
#   CSHARP_ADD_EXECUTABLE( name references [files] [output_dir] ) - Define C# executable with the given name
#   CSHARP_ADD_LIBRARY( name references [files] [output_dir] ) - Define C# library with the given name
#
# Examples:
#   CSHARP_ADD_EXECUTABLE( MyExecutable "" "Program.cs" )
#   CSHARP_ADD_EXECUTABLE( MyExecutable "ref1.dll ref2.dll" "Program.cs File1.cs" )
#   CSHARP_ADD_EXECUTABLE( MyExecutable "ref1.dll;ref2.dll" "Program.cs;File1.cs" )
#
# This file is based on the work of SimpleITK:
#   https://github.com/SimpleITK/SimpleITK/blob/master/CMake/UseCSharp.cmake
# Copyright (c) SolidOpt Team
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

# Init global solution lists
set_property(GLOBAL PROPERTY sln_projs_guid_property)
set_property(GLOBAL PROPERTY sln_projs_name_property)
set_property(GLOBAL PROPERTY sln_projs_file_property)

# Macroses

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
    MESSAGE(STATUS "  ->Depends on[Target]: ${depends_on_we}")
    add_dependencies( ${cur_target_we} ${depends_on_we} )
  else ( )
    MESSAGE(STATUS "  ->Depends on[External]: ${depends_on}")
  endif ( TARGET ${depends_on_we} )
endmacro( CSHARP_ADD_DEPENDENCY )

macro( CSHARP_SAVE_SOLUTION name )
  # Generate sln
  if ( (${CMAKE_GENERATOR} MATCHES "Visual Studio 10") OR FORCE_VISUAL_STUDIO_10_SLN)
    # Read global solution lists
    get_property(sln_projs_guid GLOBAL PROPERTY sln_projs_guid_property)
    get_property(sln_projs_name GLOBAL PROPERTY sln_projs_name_property)
    get_property(sln_projs_file GLOBAL PROPERTY sln_projs_file_property)

    MESSAGE( STATUS "Generating solution ${name}.sln" )

    # Set substitution variables
    set( VAR_Solution_Projects "" )
    set( VAR_Solution_Platforms "" )
    set( i 0 )
    foreach ( it ${sln_projs_guid} )
      list( GET sln_projs_name ${i} project_name )
      list( GET sln_projs_file ${i} project_file )
      file( RELATIVE_PATH project_file ${CMAKE_CURRENT_BINARY_DIR} ${project_file} )
      # Project(.csproj) GUID = FAE04EC0-301F-11D3-BF4B-00C04F79EFBC
      # Project(.ilproj) GUID = B4EC64DC-6D44-11DD-AAB0-C9A155D89593
      # Project folder GUID = 2150E333-8FDC-42A3-9474-1A3956D46DE8
      set( VAR_Solution_Projects "${VAR_Solution_Projects}Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"${project_name}\", \"${project_file}\", \"{${it}}\"\nEndProject\n" )
      set( VAR_Solution_Platforms "${VAR_Solution_Platforms}\t\t{${it}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU\n\t\t{${it}}.Debug|Any CPU.Build.0 = Debug|Any CPU\n\t\t{${it}}.Release|Any CPU.ActiveCfg = Release|Any CPU\n\t\t{${it}}.Release|Any CPU.Build.0 = Release|Any CPU\n" )
      math(EXPR i "${i}+1")
    endforeach(it)

    # Configure solution
    configure_file(
      ${CMAKE_MODULE_PATH}/SolutionName-v11.sln.in
      ${CMAKE_CURRENT_BINARY_DIR}/${name}.sln
      @ONLY
    )
  endif ()
endmacro( CSHARP_SAVE_SOLUTION )

# Private macro
macro( CSHARP_ADD_PROJECT type name )
  # Generate AssemblyInfo.cs
  MESSAGE( STATUS "Generating AssemblyInfo.cs for ${name}" )
  string(SUBSTRING ${SolidOpt_LastDate} 0 4 SolidOpt_LastYear)
  configure_file(
    ${CMAKE_MODULE_PATH}/AssemblyInfo.cs.in
    ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
    @ONLY
  )

  string(TOUPPER ${type} TYPE_UPCASE)

  set( refs /reference:System.dll )
  set( sources )
  set( sources_dep )

  if( ${type} MATCHES "library" )
    set( output "dll" )
    set( output_type "Library" )
  elseif( ${type} MATCHES "exe" )
    set( output "exe" )
    set( output_type "Exe" )
  elseif( ${type} MATCHES "gui" )
    set( output "exe" )
    set( output_type "WinExe" )
  endif( ${type} MATCHES "library" )

  # Step through each argument
  foreach( it ${ARGN} )
    if( ${it} MATCHES "(.*)(dll)" )
       # Argument is a dll, add reference
       list( APPEND refs /reference:${it} )
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
  MESSAGE( STATUS "Adding C# ${type} ${name}: '${CSHARP_COMPILER} /t:${type} /out:${name}.${output} /platform:${CSHARP_PLATFORM} /${BUILD_TYPE} ${CSHARP_SDK_COMPILER} ${refs} ${sources}'" )
  add_custom_command(
    COMMENT "Compiling C# ${type} ${name}: '${CSHARP_COMPILER} /t:${type} /out:${name}.${output} /platform:${CSHARP_PLATFORM} /${BUILD_TYPE} ${CSHARP_SDK_COMPILER} ${refs} ${sources}'"
    OUTPUT ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name}.${output}
    COMMAND ${CSHARP_COMPILER}
    ARGS /t:${type} /out:${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name}.${output} /platform:${CSHARP_PLATFORM} /${BUILD_TYPE} ${CSHARP_SDK_COMPILER} ${refs} ${sources}
    WORKING_DIRECTORY ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}
    DEPENDS ${sources_dep}
  )
  add_custom_target(
    ${name} ALL
    DEPENDS ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name}.${output}
    SOURCES ${sources_dep}
  )

  # Resolve dependencies
  MESSAGE( STATUS "Resolving dependencies for ${type}: ${name}" )
  foreach( it ${ARGN} )
    # Argument is a dll, add as dependency. csharp_add_dependency will decide if
    # if it was a build target or not.
    if( ${it} MATCHES "(.*)(dll)" )
       # Get the filename only (no slashes)
       get_filename_component(filename ${it} NAME)
       csharp_add_dependency( ${name} ${filename} )
     endif( )
   endforeach( )

  # Generate csproj
  if ( (${CMAKE_GENERATOR} MATCHES "Visual Studio 10") OR FORCE_VISUAL_STUDIO_10_SLN)
    find_program(guid_gen NAMES ${CMAKE_LIBRARY_OUTPUT_DIR}/guid.exe)
    if( NOT guid_gen )
      file(WRITE ${CMAKE_LIBRARY_OUTPUT_DIR}/guid.cs "class GUIDGen { static void Main() { System.Console.Write(System.Guid.NewGuid().ToString().ToUpper()); } }" )
      set( guid_gen "${CMAKE_LIBRARY_OUTPUT_DIR}/guid.exe" )
      execute_process(
        COMMAND ${CSHARP_COMPILER} /t:exe /out:${guid_gen} /platform:anycpu ${CMAKE_LIBRARY_OUTPUT_DIR}/guid.cs
      )
    endif ( )

    execute_process(COMMAND ${CSHARP_INTERPRETER} ${guid_gen} OUTPUT_VARIABLE proj_guid )

    MESSAGE( STATUS "Generating ${name}.csproj" )
    # Set substitution variables
    set( VAR_Project_GUID ${proj_guid} )
    set( VAR_Project_OutputType ${output_type} )
    set( VAR_Project_DefaultNamespace "" )
    set( VAR_Project_AssemblyName "${name}.${output}" )
    set( VAR_Project_TargetFrameworkVersion "v${CSHARP_FRAMEWORK_VERSION}" )
    set( VAR_Project_TargetFrameworkProfile "${CSHARP_FRAMEWORK_PROFILE}" )
    set( VAR_Project_InternalReferences "" )
    set( VAR_Project_References "" )
    if (refs)
      list( REMOVE_DUPLICATES refs )
    endif (refs)
    foreach ( it ${refs} )
      STRING( REGEX REPLACE "^/reference:" "" it ${it} )
      file( RELATIVE_PATH rel_it ${CMAKE_CURRENT_BINARY_DIR} ${it} )

      get_filename_component(filename ${it} NAME)
      STRING( REGEX REPLACE "(\\.dll)[^\\.dll]*$" "" name_we ${name} )
      STRING( REGEX REPLACE "(\\.dll)[^\\.dll]*$" "" filename_we ${filename} )
      if ( TARGET ${filename_we} )
        # Internal project
        get_property(sln_projs_guid GLOBAL PROPERTY sln_projs_guid_property)
        get_property(sln_projs_name GLOBAL PROPERTY sln_projs_name_property)
        get_property(sln_projs_file GLOBAL PROPERTY sln_projs_file_property)
        list( FIND sln_projs_name ${filename_we} index )
        #TODO: index=-1 ~ in GAC?
        list( GET sln_projs_file ${index} ref_proj)
        list( GET sln_projs_guid ${index} ref_proj_guid)
        file( RELATIVE_PATH rel_ref_proj ${CMAKE_CURRENT_BINARY_DIR} ${ref_proj} )
        set( VAR_Project_InternalReferences "${VAR_Project_InternalReferences}\t<ProjectReference Include=\"${rel_ref_proj}\">\n\t  <Project>{${ref_proj_guid}}</Project>\n\t  <Name>${filename_we}</Name>\n\t</ProjectReference>\n" )
      else ( )
        # in GAC
        #\t<Reference Include=\"Mono.Cecil, Version=0.9.4.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756\">\n
        #\t  <Private>False</Private>\n
        #\t</Reference>\n
        #set( VAR_Project_References "${VAR_Project_References}\t<Reference Include=\"${filename}\">\n\t  <Private>False</Private>\n\t</Reference>\n" )

        # External/vendor assembly
        file( RELATIVE_PATH rel_ref_proj ${CMAKE_CURRENT_BINARY_DIR} ${it} )
        set( VAR_Project_References "${VAR_Project_References}\t<Reference Include=\"${filename}\">\n\t  <Private>False</Private>\n\t  <HintPath>${rel_ref_proj}</HintPath>\n\t</Reference>\n" )
      endif ( TARGET ${filename_we} )

    endforeach(it)

    set( VAR_Project_CompileItems "" )
    foreach ( it ${sources_dep} )
      file( RELATIVE_PATH rel_it ${CMAKE_CURRENT_BINARY_DIR} ${it} )
      #TODO: Detect item type: Compile, EmbeddedResource, None, Folder, ...
      set( VAR_Project_CompileItems "${VAR_Project_CompileItems}\t<Compile Include=\"${rel_it}\" />\n" )
    endforeach(it)

    # Configure project
    configure_file(
      ${CMAKE_MODULE_PATH}/ProjectName-v11.csproj.in
      ${CMAKE_CURRENT_BINARY_DIR}/${name}.csproj
      @ONLY
    )

    # Add info for ptoject in global solution lists
    set_property(GLOBAL APPEND PROPERTY sln_projs_guid_property ${proj_guid})
    set_property(GLOBAL APPEND PROPERTY sln_projs_name_property ${name})
    set_property(GLOBAL APPEND PROPERTY sln_projs_file_property "${CMAKE_CURRENT_BINARY_DIR}/${name}.csproj")
  endif ()

endmacro( CSHARP_ADD_PROJECT )