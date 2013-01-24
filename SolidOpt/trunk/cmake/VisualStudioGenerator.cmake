##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

macro( CSHARP_SAVE_PROJECT proj_ix proj_guid proj_name proj_file )
  # Generate csproj
  if ( (${CMAKE_GENERATOR} MATCHES "Visual Studio 10") OR FORCE_VISUAL_STUDIO_10_SLN)

    get_property(target_name GLOBAL PROPERTY target_name_property)
    get_property(target_guid GLOBAL PROPERTY target_guid_property)

    MESSAGE( STATUS "Generating project ${proj_name}.csproj" )

    list( GET target_output_type ${proj_ix} output_type )
    list( GET target_bin_dir ${proj_ix} bin_dir )
    list( GET target_src_dir ${proj_ix} src_dir )
    list( GET target_src_dir ${proj_ix} src_dir )
    list( GET target_refs ${proj_ix} r )
    string(SUBSTRING "${r}" 1 -1 r)
    string(REPLACE "#" ";" refs "${r}")
    list( GET target_sources_dep ${proj_ix} sd )
    string(SUBSTRING "${sd}" 1 -1 sd)
    string(REPLACE "#" ";" sources_dep "${sd}")

    # Set substitution variables
    set( VAR_Project_GUID ${proj_guid} )
    set( VAR_Project_OutputType ${output_type} )
    set( VAR_Project_DefaultNamespace "" )
    set( VAR_Project_AssemblyName "${proj_name}" ) # Intentionally without extension
    set( VAR_Project_TargetFrameworkVersion "v${CSHARP_FRAMEWORK_VERSION}" )
    set( VAR_Project_TargetFrameworkProfile "${CSHARP_FRAMEWORK_PROFILE}" )
    set( VAR_Project_InternalReferences "" )
    set( VAR_Project_References "" )
    set( VAR_Project_RootNamespace "" )
    # Debug or Release target
    if (CMAKE_BUILD_TYPE STREQUAL "Debug")
      set( VAR_Project_DebugSymbols "True" )
      set( VAR_Project_DebugType "full" )
      set( VAR_Project_Optimize "False" )
      set( VAR_Project_DefineConstants "DEBUG;" )
      set( VAR_Project_Target "Debug" )
    elseif ()
      set( VAR_Project_DebugSymbols "False" )
      set( VAR_Project_DebugType "none" )
      set( VAR_Project_Optimize "True" )
      set( VAR_Project_DefineConstants "" )
      set( VAR_Project_Target "Release" )
    endif ()
    # IntermediateOutputPath (obj files)
    string(LENGTH "${CMAKE_RUNTIME_OUTPUT_DIR}" interm_len)
    math(EXPR interm_len "${interm_len}-3")
    string(SUBSTRING "${CMAKE_RUNTIME_OUTPUT_DIR}" 0 ${interm_len} interm)
    file(RELATIVE_PATH VAR_Project_IntermediateOutputPath "${bin_dir}" "${interm}obj")
    file(TO_NATIVE_PATH "${VAR_Project_IntermediateOutputPath}" VAR_Project_IntermediateOutputPath)
    # OutputPath (lib files)
    file(RELATIVE_PATH VAR_Project_OutputPath "${bin_dir}" "${CMAKE_LIBRARY_OUTPUT_DIR}")
    file(TO_NATIVE_PATH "${VAR_Project_OutputPath}" VAR_Project_OutputPath)
    # Base path
    #set( VAR_Project_BaseDirectory ${CMAKE_CURRENT_SOURCE_DIR} )
    file(TO_NATIVE_PATH "${bin_dir}" VAR_Project_BaseDirectory)

    if (refs)
      list( REMOVE_DUPLICATES refs )
    endif (refs)

    foreach ( it ${refs} )
      get_filename_component(filename ${it} NAME)
      STRING( REGEX REPLACE "(\\.dll)[^\\.dll]*$" "" name_we ${proj_name} )
      STRING( REGEX REPLACE "(\\.dll)[^\\.dll]*$" "" filename_we ${filename} )
      list(FIND target_name ${filename_we} idx)
      if (idx GREATER -1)
        # Internal project
        list( GET target_proj_file ${idx} ref_proj_file)
        list( GET target_guid ${idx} ref_proj_guid)
        file( RELATIVE_PATH rel_ref_proj_file ${bin_dir} ${ref_proj_file} )
        file( TO_NATIVE_PATH "${rel_ref_proj_file}" rel_ref_proj_file )
        set( VAR_Project_InternalReferences "${VAR_Project_InternalReferences}    <ProjectReference Include=\"${rel_ref_proj_file}\">\n      <Project>{${ref_proj_guid}}</Project>\n      <Name>${filename_we}</Name>\n    </ProjectReference>\n" )
      else ( )
        if ( TARGET ${filename_we} )
          # Vendor assembly
          file( RELATIVE_PATH rel_ref_proj_file ${bin_dir} ${it} )
          file( TO_NATIVE_PATH "${rel_ref_proj_file}" rel_ref_proj_file )
          set( VAR_Project_References "${VAR_Project_References}    <Reference Include=\"${filename_we}\">\n      <Private>True</Private>\n      <HintPath>${rel_ref_proj_file}</HintPath>\n    </Reference>\n" )
        else ()
          # GAC
          #  <Reference Include=\"Mono.Cecil, Version=0.9.4.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756\" />\n
          set( VAR_Project_References "${VAR_Project_References}    <Reference Include=\"${filename_we}\">\n      <Private>False</Private>\n    </Reference>\n" )
        endif ()
      endif ()

    endforeach(it)

    list(APPEND sources_dep "${src_dir}/CMakeLists.txt")

    set( VAR_Project_CompileItems "" )
    foreach ( it ${sources_dep} )
      file(RELATIVE_PATH rel_it "${bin_dir}" "${it}")
      file(RELATIVE_PATH link_it "${src_dir}" "${it}")
      file(TO_NATIVE_PATH "${rel_it}" rel_it)
      #TODO: Detect item type: Compile, EmbeddedResource, None, Folder, ...
      if (it MATCHES "CMakeLists\\.txt")
        set(item_type "None")
      else()
        set(item_type "Compile")
      endif()
      if (link_it MATCHES "^\\.\\.")
        set( VAR_Project_CompileItems "${VAR_Project_CompileItems}    <${item_type} Include=\"${rel_it}\" />\n" )
      else()
        set( VAR_Project_CompileItems "${VAR_Project_CompileItems}    <${item_type} Include=\"${rel_it}\">\n      <Link>${link_it}</Link>\n    </${item_type}>\n" )
      endif()
    endforeach(it)

    # Configure project
    configure_file(
      ${CMAKE_MODULE_PATH}/ProjectName-v11.csproj.in
      ${bin_dir}/${proj_name}.csproj
      @ONLY
    )
  endif ()
endmacro( CSHARP_SAVE_PROJECT )

macro( CSHARP_SAVE_VS_SOLUTION name )
  # Generate sln
  if ( (${CMAKE_GENERATOR} MATCHES "Visual Studio 10") OR FORCE_VISUAL_STUDIO_10_SLN)
    MESSAGE( STATUS "Generating solution ${name}.sln" )

    # Read global solution info lists
    get_property(target_name GLOBAL PROPERTY target_name_property)
    get_property(target_type GLOBAL PROPERTY target_type_property)
    get_property(target_output_type GLOBAL PROPERTY target_output_type_property)
    get_property(target_refs GLOBAL PROPERTY target_refs_property)
    get_property(target_sources_dep GLOBAL PROPERTY target_sources_dep_property)
    get_property(target_src_dir GLOBAL PROPERTY target_src_dir_property)
    get_property(target_bin_dir GLOBAL PROPERTY target_bin_dir_property)
    get_property(target_proj_file GLOBAL PROPERTY target_proj_file_property)
    get_property(target_guid GLOBAL PROPERTY target_guid_property)

    # Set substitution variables
    set( VAR_Solution_Projects "" )
    set( VAR_Solution_Platforms "" )
    set( VAR_Solution_NestedProjects "" )
    set( VAR_Solution_IdEscape "Id" )

    # Nested projects/folders
    set( fld_guids )
    set( fld_subpaths )
    set( i 0 )
    foreach ( it ${target_guid} )
      list( GET target_name ${i} project_name )
      list( GET target_proj_file ${i} project_file )
      file( RELATIVE_PATH project_file ${CMAKE_CURRENT_BINARY_DIR} ${project_file} )

      # SLN GUIDs
      # Project(.csproj) GUID = FAE04EC0-301F-11D3-BF4B-00C04F79EFBC
      # Project(.ilproj) GUID = B4EC64DC-6D44-11DD-AAB0-C9A155D89593
      # Project(Solution folder) GUID = 2150E333-8FDC-42A3-9474-1A3956D46DE8

      # Folders
      string(REGEX MATCHALL "[^/]+" li "${project_file}")
      list(REMOVE_AT li -1)
      set( current_subpath "" )
      set( nested_in_guid "" )
      foreach ( fld_it ${li} )
        set(current_subpath "${current_subpath}/${fld_it}")
        list(FIND fld_subpaths "${current_subpath}" idx)
        if (idx EQUAL -1)
          execute_process(COMMAND ${CSHARP_INTERPRETER} ${guid_gen} OUTPUT_VARIABLE fld_guid )
          #
          list(APPEND fld_subpaths "${current_subpath}")
          list(APPEND fld_guids "${fld_guid}")
          #
          set( VAR_Solution_Projects "${VAR_Solution_Projects}Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"${fld_it}\", \"${fld_it}\", \"{${fld_guid}}\"\nEndProject\n" )
          if (nested_in_guid)
            set( VAR_Solution_NestedProjects "${VAR_Solution_NestedProjects}    {${fld_guid}} = {${nested_in_guid}}\n" )
          endif()
        else ()
          list(GET fld_subpaths ${idx} current_subpath)
          list(GET fld_guids ${idx} fld_guid)
        endif()
        set( nested_in_guid "${fld_guid}" )
      endforeach()

      # Project
      file(TO_NATIVE_PATH "${project_file}" project_file)
      set( VAR_Solution_Projects "${VAR_Solution_Projects}Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"${project_name}\", \"${project_file}\", \"{${it}}\"\nEndProject\n" )
      set( VAR_Solution_Platforms "${VAR_Solution_Platforms}    {${it}}.${CMAKE_BUILD_TYPE}|Any CPU.ActiveCfg = ${CMAKE_BUILD_TYPE}|Any CPU\n    {${it}}.${CMAKE_BUILD_TYPE}|Any CPU.Build.0 = ${CMAKE_BUILD_TYPE}|Any CPU\n" )
      if (nested_in_guid)
        set( VAR_Solution_NestedProjects "${VAR_Solution_NestedProjects}    {${it}} = {${nested_in_guid}}\n" )
      endif()
      # Save project
      csharp_save_project(${i} ${it} ${project_name} ${project_file})

      math(EXPR i "${i}+1")
    endforeach(it)

    # Configure solution
    configure_file(
      ${CMAKE_MODULE_PATH}/SolutionName-v11.sln.in
      ${CMAKE_CURRENT_BINARY_DIR}/${name}.sln
      @ONLY
    )
  endif ()
endmacro( CSHARP_SAVE_VS_SOLUTION )
