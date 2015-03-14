##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

# Build our vendor drops using MSBuild, CMake, Copy, ...

## SolidOpt
##TODO: Fix vendors
#add_vendor(SolidOpt ../../../../SolidOpt)

# SolidOpt.Serives
#TODO: Fix vendors
add_vendor(SolidOpt.Services ../../../../SolidOpt/src/Services/src)

# SolidV
#TODO: Fix vendors
add_vendor(SolidV ../../../../SolidV)