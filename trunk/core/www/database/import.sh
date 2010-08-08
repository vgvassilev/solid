#!/bin/bash
#########################################################################################################################
# This script is created by Vassil Vassilev and may contain errors.							#
# The purpose is to perform automatic import of the new database if possible while updating from subversion control	#
#########################################################################################################################

MyUSER="remoteroot"    		 	# USERNAME
MyPASS="remoteroot32"      	 	# PASSWORD
MyHOST="localhost"         	 	# Hostname
MyDB="sandbox-solidopt-core-db"  	# Database to dump
NewDB="sandbox-solidopt-core-db.mysql"	# New database file

# Extra variables for the dump if you want to add another use: MyExportVars="--add-drop-database=TRUE --another-var=(TRUE|FALSE)"
MyImportVars="--default-character-set=utf8"
 
# Linux bin paths, change this if it can not be autodetected via which command
MYSQL="$(which mysql)"

# Check all expected is there
if [ "$MYSQL" == "" ] ; then 
	echo "Error: MYSQL not detected. Please install it or edit the script." 
	exit -1 
fi
 
# Store list of databases
DBS=""

# Get all database list first

$MYSQL -u $MyUSER -h $MyHOST --password=$MyPASS -Bse "create database if not exists \`$MyDB\` ; "
$MYSQL -u $MyUSER -h $MyHOST -p$MyPASS $MyImportVars --database=$MyDB < $NewDB

