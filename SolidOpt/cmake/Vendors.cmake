# $Id$
# It is part of the SolidOpt Copyright Policy (see Copyright.txt)
# For further details see the nearest License.txt

macro(ADD_VENDOR name vendor_path)
  if(NOT EXISTS ${name})
    ### TODO: Remove hard dependency to SolidOpt vendors
    get_filename_component(VENDOR_ABSOLUTE_PATH "${vendor_path}" ABSOLUTE)
    add_subdirectory("${VENDOR_ABSOLUTE_PATH}" "${name}")
  else()
    add_subdirectory("$name")
  endif()
endmacro(ADD_VENDOR)
