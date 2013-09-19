##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

add_subdirectory(vendors)

csharp_add_gui_executable(
  DataMorphose.exe
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/gtk-gui/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/gtk-gui/*.stetic
  ${CMAKE_CURRENT_SOURCE_DIR}/Controller/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Controller/Deduce/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Controller/Map/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Controller/ReversibleActions/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Controller/Tools/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Controller/Transform/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Controller/Validate/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Model/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/Model/Meta/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/View/*.cs
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  DataMorphose.exe
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/MonoDevelop.Dock.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidV.dll
  Mono.Posix.dll
  # GAC
  "gtk-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f"
  "gdk-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f"
  "glib-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f"
  "glade-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f"
  "pango-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f"
  "atk-sharp, Version=2.12.0.0, Culture=neutral, PublicKeyToken=35e10195dab3c99f"
  )
csharp_add_project_meta(
  DataMorphose.exe
  "StartupProject" "true"
  )

# Configure application config
configure_file(
  ${CMAKE_CURRENT_SOURCE_DIR}/Properties/app.config
  ${CMAKE_RUNTIME_OUTPUT_DIR}/DataMorphose.exe.config
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
