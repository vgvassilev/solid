#
# A CMake Module for using MSBuild.
#
# The following macros are set:
#   CSHARP_ADD_MSBUILD_PROJECT
#
# Copyright (c) SolidOpt Team
#

macro( CSHARP_ADD_MSBUILD_PROJECT filename )
  # Add custom target and command
  MESSAGE( STATUS "Adding project ${filename} for MSBuild-ing." )
  # TODO: Check if it was executable and set it properly
  set( TYPE_UPCASE "LIBRARY" )
  set( output "dll" )

  get_filename_component(name ${filename} NAME)
  STRING( REGEX REPLACE "(\\.csproj)[^\\.csproj]*$" "" name_we ${name} )
  STRING( REGEX REPLACE "(\\.dll)[^\\.dll]*$" "" name_we ${name_we} )
  STRING( REGEX REPLACE "(\\.sln)[^\\.sln]*$" "" name_we ${name_we} )

  add_custom_command(
    COMMENT "MSBuilding ${filename}."
    OUTPUT ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name_we}.${output}
    COMMAND ${MSBUILD}
    ARGS /p:OutputPath=${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR} ${filename}
    WORKING_DIRECTORY ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}
  )
  add_custom_target(
    ${name_we} ALL
    DEPENDS ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name_we}.${output}
    SOURCES ${filename}
  )

  # TODO: For now build vendors if SLN generation is enabled. We nust rethink it!
  if ( (${CMAKE_GENERATOR} MATCHES "Visual Studio 10") OR VS10SLN)
    MESSAGE(STATUS "MSBuilding ${filename}...")
    execute_process(
      COMMAND ${MSBUILD} /p:OutputPath=${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR} ${filename}
      WORKING_DIRECTORY ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}
    )
    #OUTPUT ${CMAKE_${TYPE_UPCASE}_OUTPUT_DIR}/${name_we}.${output}
  endif()
endmacro( CSHARP_ADD_MSBUILD_PROJECT )
