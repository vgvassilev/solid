##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

# Those two are relevant when configuring just docs standalone
project(SolidIDE NONE)
cmake_minimum_required(VERSION 2.6.2)
find_package(Doxygen)
if (DOXYGEN_FOUND)
  # This relevant when configuring just docs standalone. This is useful for 
  # doxygen to be able to display Generated on `date` for rXXX by Doxygen
  if (NOT SolidOpt_Revision)
    include (../Version.cmake)
  endif()

  configure_file(
    ${CMAKE_CURRENT_SOURCE_DIR}/doxygen/conf/doxygen.conf.in 
    ${PROJECT_BINARY_DIR}/doxygen/doxygen.conf
    @ONLY
    )
  # Copy the css next to the index.html so that it will be at the document root
  # scope of a regular server configuration.
  configure_file(
    ${CMAKE_CURRENT_SOURCE_DIR}/doxygen/conf/custom.css 
    ${PROJECT_BINARY_DIR}/doxygen/html-new/custom.css
    COPYONLY
    )
  configure_file(
    ${CMAKE_CURRENT_SOURCE_DIR}/doxygen/conf/doxygen.intro.in 
    ${PROJECT_BINARY_DIR}/doxygen/doxygen.intro
    @ONLY
    )
  configure_file(
    ${CMAKE_CURRENT_SOURCE_DIR}/doxygen/conf/header.html.in 
    ${PROJECT_BINARY_DIR}/doxygen/header.html
    @ONLY
    )
  configure_file(
    ${CMAKE_CURRENT_SOURCE_DIR}/doxygen/conf/footer.html.in 
    ${PROJECT_BINARY_DIR}/doxygen/footer.html
    @ONLY
    )
  add_custom_target(
    doc
    COMMAND ${DOXYGEN_EXECUTABLE}
    "${PROJECT_BINARY_DIR}/doxygen/doxygen.conf"
    WORKING_DIRECTORY ${CMAKE_CURRENT_BINARY_DIR}
    SOURCES "${DOXYGEN_INPUT_DIR}"
    COMMENT "Generating API documentation with Doxygen" VERBATIM
    )
#  install(DIRECTORY "${PROJECT_BINARY_DIR}/docs/"
#    DESTINATION "docs")
endif()
