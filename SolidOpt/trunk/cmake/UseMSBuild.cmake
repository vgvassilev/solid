#
# A CMake Module for using MSBuild.
#
# The following macros are set:
#   CSHARP_ADD_MSBUILD_PROJECT
#
# Copyright (c) SolidOpt Team
#

macro( CSHARP_ADD_MSBUILD_PROJECT filename )
  # TODO: Check if it was executable and set it properly
  set( TYPE_UPCASE "LIBRARY" )
  set( output "dll" )

  get_filename_component(name ${filename} NAME)
  STRING( REGEX REPLACE "(\\.csproj)[^\\.csproj]*$" "" name_we ${name} )
  STRING( REGEX REPLACE "(\\.dll)[^\\.dll]*$" "" name_we ${name_we} )
  STRING( REGEX REPLACE "(\\.sln)[^\\.sln]*$" "" name_we ${name_we} )

  if ( "${name}" MATCHES "^.*\\.dll$" )
    # Add custom target and command
    MESSAGE( STATUS "Adding binary library project ${name} for Coping." )

    add_custom_command(
      COMMENT "Coping library ${name}."
      OUTPUT ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name}
      COMMAND ${CMAKE_COMMAND} -E copy ${filename} ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name}
      MAIN_DEPENDENCY ${filename}
      WORKING_DIRECTORY ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}
    )
    add_custom_target(
      ${name} ALL
      DEPENDS ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name}
      SOURCES ${filename}
    )
  else()
    # Add custom target and command
    MESSAGE( STATUS "Adding project ${filename} for MSBuild-ing." )

    add_custom_command(
      COMMENT "MSBuilding ${filename}."
      OUTPUT ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name_we}.${output}
      COMMAND ${MSBUILD}
      ARGS /p:OutputPath=${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR} ${filename}
      WORKING_DIRECTORY ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}
    )
    add_custom_target(
      "${name_we}.${output}" ALL
      DEPENDS ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name_we}.${output}
      SOURCES ${filename}
    )
  endif()

  # TODO: For now build vendors if SLN generation is enabled. We nust rethink it!
  if ( (${CMAKE_GENERATOR} MATCHES "Visual Studio 10") OR VS10SLN)
    if ( "${filename}" MATCHES "^.*\\.dll$" )
      MESSAGE(STATUS "Coping binary library ${filename}...")
      file(COPY ${filename} DESTINATION ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/)
    else()
      MESSAGE(STATUS "MSBuilding ${filename}...")
      execute_process(
        COMMAND ${MSBUILD} /p:OutputPath=${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR} ${filename}
        WORKING_DIRECTORY ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}
      )
      #OUTPUT ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name_we}.${output}
    endif()
  endif()
endmacro( CSHARP_ADD_MSBUILD_PROJECT )
