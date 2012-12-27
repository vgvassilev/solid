#build our vendor drops using msbuild

if( WIN32 )
  list( GET CSHARP_DOTNET_VERSIONS 0 newest_csharp_dotnet_version )
endif( )

find_program(msbuild NAMES xbuild ${csharp_dotnet_framework_dir}/${newest_csharp_dotnet_version}/msbuild.exe)

MESSAGE(STATUS "MSBUILD: ${msbuild}")

if( NOT msbuild )
  message(FATAL_ERROR "MSBUILD/XBUILD not found")
endif( NOT msbuild )
message( STATUS "Compiling externals using msbuild")

execute_process(
  COMMAND ${msbuild} /p:OutputPath=${CMAKE_LIBRARY_OUTPUT_DIR} ${CMAKE_CURRENT_SOURCE_DIR}/vendors/Mono.Cecil/Mono.Cecil.csproj
  COMMAND ${msbuild} /p:OutputPath=${CMAKE_LIBRARY_OUTPUT_DIR} ${CMAKE_CURRENT_SOURCE_DIR}/vendors/Mono.Cecil/rocks/Mono.Cecil.Rocks.csproj
  COMMAND ${msbuild} /p:OutputPath=${CMAKE_LIBRARY_OUTPUT_DIR} ${CMAKE_CURRENT_SOURCE_DIR}/vendors/Cecil.Decompiler/Cecil.Decompiler/Cecil.Decompiler.csproj
  COMMAND ${msbuild} /p:OutputPath=${CMAKE_LIBRARY_OUTPUT_DIR} ${CMAKE_CURRENT_SOURCE_DIR}/vendors/NUnit/NUnitCore/core/nunit.core.dll.csproj
  COMMAND ${msbuild} /p:OutputPath=${CMAKE_LIBRARY_OUTPUT_DIR} ${CMAKE_CURRENT_SOURCE_DIR}/vendors/NUnit/NUnitFramework/framework/nunit.framework.dll.csproj
)

add_subdirectory(Services)
add_subdirectory(Core) # Core depends on Services

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
