rem #########################################################################################################################
rem # This script is created by Vassil Vassilev and may contain errors.							    #
rem # The purpose is to perform automatic import of the new database if possible while updating from subversion control	    #
rem #########################################################################################################################

rem USERNAME
set MyUSER="remoteroot"
rem PASSWORD
set MyPASS="remoteroot32"
rem  Hostname
set MyHOST="localhost"
rem Database to dump
set MyDB="sandbox-solidopt-core-db"
rem New database file
set NewDB="sandbox-solidopt-core-db.sql"

rem Extra variables for the dump if you want to add another use: MyExportVars="--add-drop-database=TRUE --another-var=(TRUE|FALSE)"
set MyImportVars="--default-character-set=utf8"
 
rem Windows bin paths, change this if it can not be autodetected via which command
set MYSQL="C:\wamp\bin\mysql\mysql5.1.36\"

rem Check all expected is there
if "%MYSQL%" == "" (
	echo "Error: MYSQL not detected. Please install it or edit the script." 
	exit -1 
)

 
rem Store list of databases
set DBS=""

rem Get all database list first

%MYSQL% -u %MyUSER% -h %MyHOST% --password=%MyPASS% -Bse "create database if not exists \`%MyDB%\` ; "
%MYSQL% -u %MyUSER% -h %MyHOST% -p%MyPASS% %MyImportVars% --database=%MyDB% < %NewDB%
