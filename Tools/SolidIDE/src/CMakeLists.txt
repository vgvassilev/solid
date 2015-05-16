##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

add_subdirectory(vendors)

csharp_add_gui_executable(
  SolidIDE.exe
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/gtk-gui/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/gtk-gui/*.stetic
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  SolidIDE.exe
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidV.dll
  Mono.Posix.dll
  # GAC
  pkg:gtk-sharp-2.0
  System.Xml.dll
  )
csharp_add_project_meta(
  SolidIDE.exe
  "StartupProject" "true"
  )

# Configure application config
configure_file(
  ${CMAKE_CURRENT_SOURCE_DIR}/Properties/app.config
  ${CMAKE_RUNTIME_OUTPUT_DIR}/SolidIDE.exe.config
  @ONLY
)
configure_file(
  ${CMAKE_CURRENT_SOURCE_DIR}/SolidIDE.exe.sh.in
  ${CMAKE_RUNTIME_OUTPUT_DIR}/SolidIDE.exe.sh
  @ONLY
)

# Plugin config and dev symlink
configure_file(
  ${CMAKE_CURRENT_SOURCE_DIR}/Properties/Plugins.env.in
  ${CMAKE_RUNTIME_OUTPUT_DIR}/Plugins.env
  @ONLY
)
execute_process(
  COMMAND ${CMAKE_COMMAND} -E create_symlink ../lib ${CMAKE_RUNTIME_OUTPUT_DIR}/plugins
  WORKING_DIRECTORY ${CMAKE_RUNTIME_OUTPUT_DIR}
)


#add_subdirectory(gtk-gui)
#add_subdirectory(BuildInformation)

