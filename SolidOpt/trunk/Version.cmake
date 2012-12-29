# $Id: AbstractService.cs 441 2011-03-24 14:56:14Z vvassilev $
# It is part of the SolidOpt Copyright Policy (see Copyright.txt)
# For further details see the nearest License.txt

# This defines SolidOpt versioning.
# The following variables are set:
#   SolidOpt_Major - Framework major version
#   SolidOpt_Minor - Framework minor version
#   SolidOptRevision - Svn working copy revision.

set(SolidOpt_Major 0)
set(SolidOpt_Minor 0)

# Get trunk rev number.
find_program(SVN_EXECUTABLE svn DOC "subversion command line client")

macro(Subversion_GET_REVISION dir variable)
  execute_process(
    COMMAND ${SVN_EXECUTABLE} info ${dir}
    OUTPUT_VARIABLE ${variable}
    OUTPUT_STRIP_TRAILING_WHITESPACE
    )
  string(REGEX REPLACE "^(.*\n)?Revision: ([^\n]+).*" "\\2" ${variable} "${${variable}}")
endmacro(Subversion_GET_REVISION)

Subversion_GET_REVISION(${CMAKE_SOURCE_DIR} SolidOptRevision)

