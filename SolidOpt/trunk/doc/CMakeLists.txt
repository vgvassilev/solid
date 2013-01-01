find_package(Doxygen)
if (DOXYGEN_FOUND)
  configure_file(
    ${CMAKE_CURRENT_SOURCE_DIR}/doxygen/conf/doxygen.conf.in 
    ${PROJECT_BINARY_DIR}/doc/doxygen.conf
    @ONLY
    )
  configure_file(
    ${CMAKE_CURRENT_SOURCE_DIR}/doxygen/conf/custom.css 
    ${PROJECT_BINARY_DIR}/doc/custom.css
    COPYONLY
    )
  configure_file(
    ${CMAKE_CURRENT_SOURCE_DIR}/doxygen/conf/doxygen.intro.in 
    ${PROJECT_BINARY_DIR}/doc/doxygen.intro
    @ONLY
    )
  configure_file(
    ${CMAKE_CURRENT_SOURCE_DIR}/doxygen/conf/header.html.in 
    ${PROJECT_BINARY_DIR}/doc/header.html
    @ONLY
    )
  configure_file(
    ${CMAKE_CURRENT_SOURCE_DIR}/doxygen/conf/footer.html.in 
    ${PROJECT_BINARY_DIR}/doc/footer.html
    @ONLY
    )
  add_custom_target(
    doxygen
    COMMAND ${DOXYGEN_EXECUTABLE}
    "${PROJECT_BINARY_DIR}/doc/doxygen.conf"
    WORKING_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}
    SOURCES "${CMAKE_SOURCE_DIR}"
    COMMENT "Generating API documentation with Doxygen" VERBATIM
    )
#  install(DIRECTORY "${PROJECT_BINARY_DIR}/docs/"
#    DESTINATION "docs")
endif()
