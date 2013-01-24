##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

macro( CSHARP_SAVE_PROJECT name )
  # Generate csproj
  if ( (${CMAKE_GENERATOR} MATCHES "Visual Studio 10") OR FORCE_VISUAL_STUDIO_10_SLN)
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

    MESSAGE( STATUS "Generating ${name}.csproj" )

    # Set substitution variables
    set( VAR_Project_GUID ${proj_guid} )
    set( VAR_Project_OutputType ${output_type} )
    set( VAR_Project_DefaultNamespace "" )
    set( VAR_Project_AssemblyName "${name}" ) # Intentionally without extension
    set( VAR_Project_TargetFrameworkVersion "v${CSHARP_FRAMEWORK_VERSION}" )
    set( VAR_Project_TargetFrameworkProfile "${CSHARP_FRAMEWORK_PROFILE}" )
    set( VAR_Project_InternalReferences "" )
    set( VAR_Project_References "" )
    set( VAR_Project_RootNamespace "" )
    # Debug or Release target
    if (CMAKE_BUILD_TYPE MATCHES "Debug")
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
    file(RELATIVE_PATH VAR_Project_IntermediateOutputPath "${CMAKE_CURRENT_BINARY_DIR}" "${interm}obj")
    file(TO_NATIVE_PATH "${VAR_Project_IntermediateOutputPath}" VAR_Project_IntermediateOutputPath)
    # OutputPath (lib files)
    file(RELATIVE_PATH VAR_Project_OutputPath "${CMAKE_CURRENT_BINARY_DIR}" "${CMAKE_LIBRARY_OUTPUT_DIR}")
    file(TO_NATIVE_PATH "${VAR_Project_OutputPath}" VAR_Project_OutputPath)
    # Base path
    #set( VAR_Project_BaseDirectory ${CMAKE_CURRENT_SOURCE_DIR} )
    file(TO_NATIVE_PATH "${CMAKE_CURRENT_BINARY_DIR}" VAR_Project_BaseDirectory)

    if (refs)
      list( REMOVE_DUPLICATES refs )
    endif (refs)

    foreach ( it ${refs} )
      STRING( REGEX REPLACE "^/reference:" "" it ${it} )
      #file( RELATIVE_PATH rel_it ${CMAKE_CURRENT_BINARY_DIR} ${it} )

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
        file(TO_NATIVE_PATH "${rel_ref_proj}" rel_ref_proj)
        set( VAR_Project_InternalReferences "${VAR_Project_InternalReferences}    <ProjectReference Include=\"${rel_ref_proj}\">\n      <Project>{${ref_proj_guid}}</Project>\n      <Name>${filename_we}</Name>\n    </ProjectReference>\n" )
      else ( )
        if ( EXISTS ${it} )
          # External/vendor assembly
          file( RELATIVE_PATH rel_ref_proj ${CMAKE_CURRENT_BINARY_DIR} ${it} )
          file(TO_NATIVE_PATH "${rel_ref_proj}" rel_ref_proj)
          set( VAR_Project_References "${VAR_Project_References}    <Reference Include=\"${rel_ref_proj}\">\n      <Private>True</Private>\n      <HintPath>${rel_ref_proj}</HintPath>\n    </Reference>\n" )
        else ()
          # in GAC
          #  <Reference Include=\"Mono.Cecil, Version=0.9.4.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756\" />\n
          set( VAR_Project_References "${VAR_Project_References}    <Reference Include=\"${filename_we}\">\n      <Private>False</Private>\n    </Reference>\n" )
        endif ()
      endif ( TARGET ${filename_we} )

    endforeach(it)

    list(APPEND sources_dep "${CMAKE_CURRENT_SOURCE_DIR}/CMakeLists.txt")

    set( VAR_Project_CompileItems "" )
    foreach ( it ${sources_dep} )
      file(RELATIVE_PATH rel_it "${CMAKE_CURRENT_BINARY_DIR}" "${it}")
      file(RELATIVE_PATH link_it "${CMAKE_CURRENT_SOURCE_DIR}" "${it}")
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
      ${CMAKE_CURRENT_BINARY_DIR}/${name}.csproj
      @ONLY
    )

    # Add info for ptoject in global solution lists
    set_property(GLOBAL APPEND PROPERTY sln_projs_guid_property ${proj_guid})
    set_property(GLOBAL APPEND PROPERTY sln_projs_name_property ${name})
    set_property(GLOBAL APPEND PROPERTY sln_projs_file_property "${CMAKE_CURRENT_BINARY_DIR}/${name}.csproj")
  endif ()
endmacro( CSHARP_SAVE_PROJECT )

macro( CSHARP_SAVE_VS_SOLUTION name )
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
    set( VAR_Solution_NestedProjects "" )
    set( VAR_Solution_IdEscape "Id" )

    # Nested projects/folders
    set( fld_guids )
    set( fld_subpaths )
    set( i 0 )
    foreach ( it ${sln_projs_guid} )
      list( GET sln_projs_name ${i} project_name )
      list( GET sln_projs_file ${i} project_file )
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
      set( VAR_Solution_Platforms "${VAR_Solution_Platforms}    {${it}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU\n    {${it}}.Debug|Any CPU.Build.0 = Debug|Any CPU\n    {${it}}.Release|Any CPU.ActiveCfg = Release|Any CPU\n    {${it}}.Release|Any CPU.Build.0 = Release|Any CPU\n" )
      if (nested_in_guid)
        set( VAR_Solution_NestedProjects "${VAR_Solution_NestedProjects}    {${it}} = {${nested_in_guid}}\n" )
      endif()
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
