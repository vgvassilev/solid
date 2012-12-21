#build our vendor drops using msbuild
find_program(MSBUILD NAMES msbuild xbuild)
if( NOT MSBUILD )
  message(FATAL_ERROR "MSBUILD/XBUILD not found")
endif( NOT MSBUILD )
message( STATUS "Compiling externals using MSBUILD")

execute_process(
  COMMAND ${MSBUILD} /p:OutputPath=${CMAKE_LIBRARY_OUTPUT_DIR} ${CMAKE_CURRENT_SOURCE_DIR}/vendors/Mono.Cecil/Mono.Cecil.csproj
  COMMAND ${MSBUILD} /p:OutputPath=${CMAKE_LIBRARY_OUTPUT_DIR} ${CMAKE_CURRENT_SOURCE_DIR}/vendors/Mono.Cecil/rocks/Mono.Cecil.Rocks.csproj
  COMMAND ${MSBUILD} /p:OutputPath=${CMAKE_LIBRARY_OUTPUT_DIR} ${CMAKE_CURRENT_SOURCE_DIR}/vendors/Cecil.Decompiler/Cecil.Decompiler/Cecil.Decompiler.csproj
  COMMAND ${MSBUILD} /p:OutputPath=${CMAKE_LIBRARY_OUTPUT_DIR} ${CMAKE_CURRENT_SOURCE_DIR}/vendors/NUnit/NUnitCore/core/nunit.core.dll.csproj
  COMMAND ${MSBUILD} /p:OutputPath=${CMAKE_LIBRARY_OUTPUT_DIR} ${CMAKE_CURRENT_SOURCE_DIR}/vendors/NUnit/NUnitFramework/framework/nunit.framework.dll.csproj
)

add_subdirectory(Services)

#find_package(Doxygen)
#if (DOXYGEN_FOUND)
#    configure_file("docs/Doxyfile.in" "${PROJECT_BINARY_DIR}/Doxyfile")
#    add_custom_target(docs
#                      COMMAND ${DOXYGEN_EXECUTABLE}
#                              "${PROJECT_BINARY_DIR}/Doxyfile"
#                      SOURCES "${PROJECT_BINARY_DIR}/Doxyfile")
#    install(DIRECTORY "${PROJECT_BINARY_DIR}/docs/"
#            DESTINATION "docs")
#endif()
