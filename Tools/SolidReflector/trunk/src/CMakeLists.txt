##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

add_subdirectory(vendors)

csharp_add_gui_executable(
  SolidReflector.exe
  ${CMAKE_CURRENT_SOURCE_DIR}/*.cs
  ${CMAKE_CURRENT_SOURCE_DIR}/gtk-gui/*.cs
#  ${CMAKE_CURRENT_SOURCE_DIR}/gtk-gui/*.stetic
  ${CMAKE_CURRENT_SOURCE_DIR}/gtk-gui/gui.stetic
  ${CMAKE_CURRENT_SOURCE_DIR}/Properties/app.config
  ${CMAKE_CURRENT_BINARY_DIR}/AssemblyInfo.cs
  )
csharp_add_dependencies(
  SolidReflector.exe
  # External
  ${CMAKE_LIBRARY_OUTPUT_DIR}/SolidOpt.Services.dll
  ${CMAKE_LIBRARY_OUTPUT_DIR}/MonoDevelop.Dock.dll
  Mono.Posix.dll
  # GAC
  gtk-sharp.dll
  gdk-sharp.dll
  )

configure_file(
  ${CMAKE_CURRENT_SOURCE_DIR}/Properties/Plugins.env.in
  ${CMAKE_RUNTIME_OUTPUT_DIR}/Plugins.env
  @ONLY
)


#add_subdirectory(gtk-gui)
#add_subdirectory(BuildInformation)

