#!/bin/bash
##
## $Id$
## It is part of the SolidOpt Copyright Policy (see Copyright.txt)
## For further details see the nearest License.txt
##

## This script helps creation of new subprojects with the structure:
## SolidOpt_SRC
## ...
##   ProjectName
##   +--CMakeLists.txt
##   +--src
##      +--CMakeLists.txt
##   +--test
##      +--CMakeLists.txt
##      +--TestCases
##         +--CMakeLists.txt

## The script should be called with createNewSolidOptProject.sh -d folder -n ProjectName
#Process args
PROJECT_FOLDER=""
PROJECT_NAME=""

#
### echo %%% Help function
#
printhelp() {
cat <<EOF
'configure' configures SolidOpt to adapt to many kind of systems.

Usage:     $0 [options]

   OPTIONS            DESCRIPTION                                  
   -d                 The directory where the subproject will be created.
   -n                 The name of the subproject.

Report bugs to <http://bugzilla.solidopt.org/describecomponents.cgi?product=SolidOpt>
EOF
}


while [[ $1 = -* ]]; do
    arg=$1; shift           # shift the found arg away.
    case $arg in
        -d)
            PROJECT_FOLDER="$1"
            shift               # takes an arg, needs an extra shift
            ;;
        -n)
            PROJECT_NAME="$1"
            shift               # takes an arg, needs an extra shift
            ;;
      "--help" | "-h")
          printhelp ; exit 0;
          ;;
      *)
          echo "Unrecognized argument: $arg_name. Exiting..."
          exit 1
          ;;
    esac
done

if [ "$PROJECT_FOLDER" == "" ]; then
    echo "Project folder not set. Please use -d to set it."
    exit 1
fi

if [ "$PROJECT_NAME" == "" ]; then
    echo "Project name not set. Please use -n to set it."
    exit 1
fi

if [ -d "$PROJECT_FOLDER" ]; then
    # Control will enter here if $PROJECT_FOLDER exists.
    echo "Folder $PROJECT_FOLDER already exists. Aborting."
    exit 1
fi

#Create the folder which our new project will reside in.
mkdir $PROJECT_FOLDER

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

cp -R $SCRIPT_DIR/sampleProjectSkeleton/* $PROJECT_FOLDER/

#Replace all relevant variables
find $PROJECT_FOLDER -type f -exec sed -i '' "s,@PROJECT_NAME@,$PROJECT_NAME,g" {} \;

if [ -f $PROJECT_FOLDER/../CMakeLists.txt ]; then
    #If there is parent CMakeLists.txt we want to patch the new project in
    echo "Patching $PROJECT_FOLDER/../CMakeLists.txt..."
    echo "add_subdirectory(`basename $PROJECT_FOLDER`)" >> $PROJECT_FOLDER/../CMakeLists.txt
fi
echo "Project $PROJECT_NAME successfully created in $PROJECT_FOLDER"
