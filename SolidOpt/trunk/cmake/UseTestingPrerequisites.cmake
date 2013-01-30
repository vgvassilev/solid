##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

#
# A CMake Module for finding and using the prerequisites needed for running unit
# tests.
#
# The following macros are set:
#   CSHARP_ADD_TEST_CASE - Adds a single test case.
#  

macro( CSHARP_ADD_TEST_CASE target)
  set(test_cases)
  set(test_results)
  # Step through each argument. Argument is a test source file
  foreach( it ${ARGN} )
    # We need to expand wildcards
    if( EXISTS ${it} )
      list( APPEND test_cases ${it} )
    else()
      FILE( GLOB it_glob ${it} )
      list( APPEND test_cases ${it_glob} )
    endif()
  endforeach( )

  foreach(it ${test_cases})
    get_filename_component(test_case ${it} NAME)
    MESSAGE( STATUS "Configuring test case ${test_case} for ${target}" )
    configure_file(
      ${it}
      ${CMAKE_CURRENT_BINARY_DIR}/${test_case}
      @ONLY
      )

    # Consider it.* as result files and copy them over. 
    # Expand wildcards first.
    FILE( GLOB test_case_results ${it}.* )
    foreach(result_it ${test_case_results})
      get_filename_component(test_case_result ${result_it} NAME)

      configure_file(
        ${test_case_result}
        ${CMAKE_CURRENT_BINARY_DIR}/${test_case_result}
        COPYONLY
        )
      # Add the result files to the list of result files.
      list( APPEND test_results ${result_it} )
    endforeach()
  endforeach()

endmacro( CSHARP_ADD_TEST_CASE )
