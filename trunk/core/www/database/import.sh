#!/bin/bash
#########################################################################################################################
# This script is created by Vassil Vassilev and may contain errors.							#
# The purpose is to perform automatic import of the database if possible while updating from subversion control		#
# 															#
# It:															#
# 1. Dumps olddatabase													#
# 2. If there are differences												#
#	2.1 Do you want to replace the database? R -> Replace D -> Show diff G -> Show GUI Diff if exists C -> exit	#
#	2.2. Then zip old database to /olddumps with unique name (date-time-value)					#
# 3. Compares if there are no differences tell nothing to do database is up to date and exit				#
#########################################################################################################################

MyUSER="root"     		 	# USERNAME
MyPASS="mysqlroot32"      	 	# PASSWORD
MyHOST="localhost"         	 	# Hostname
MyDB="sandbox-solidopt-core-db"  	# Database to dump
NewDB="sandbox-solidopt-core-db.sql"	# New database file

# Extra variables for the dump if you want to add another use: MyExportVars="--add-drop-database=TRUE --another-var=(TRUE|FALSE)"
MyExportVars="--compact=TRUE --add-drop-database=TRUE --set-charset=TRUE"
MyImportVars="--default-character-set=utf8"
 
# Linux bin paths, change this if it can not be autodetected via which command
MYSQL="$(which mysql)"
MYSQLDUMP="$(which mysqldump)"
#Define tables to omit
TABLESTOSKIP="--ignore-table $MyDB.cache_block --ignore-table $MyDB.cache_filter --ignore-table $MyDB.cache_form --ignore-table $MyDB.cache_menu --ignore-table $MyDB.cache_page --ignore-table $MyDB.cache_update --ignore-table $MyDB.sessions --ignore-table $MyDB.watchdog"

GZIP="$(which gzip)"
GREP="$(which grep)"

#Standard diff tool
DIFF="$(which diff)"
#GUI diff tool
MELD="$(which meld)"

# Check all expected is there
if [ "$MYSQL" == "" ] ; then 
	echo "Error: MYSQL not detected. Please install it or edit the script." 
	exit -1 
fi
if [ "$MYSQLDUMP" == "" ] ; then 
	echo "Error: MYSQLDUMP not detected. Please install it or edit the script." 
	exit -1 
fi
if [ "$GREP" == "" ] ; then
	echo "Error: GREP not detected. Please install it or edit the script." 
	exit -1 
fi
if [ "$DIFF" == "" ] ; then 
	echo "Error: DIFF not detected. Please install it or edit the script." 
	exit -1 
fi

# Check optional tools
if [ "$GZIP" == "" ] ; then 
	echo "Warning: GZIP not detected. Please install it or edit the script."
fi
if [ "$MELD" == "" ] ; then 
	echo "Warning: MELD (GUI DIFF) not detected. Please install it or edit the script." 
fi

 
# Backup Dest directory, change this if you have someother location
DEST="olddumps"
 
# Main directory where backup will be stored
MBD="$DEST"
 
# Get hostname
HOST="$(hostname)"
 
# Get data in dd-mm-yyyy format
NOW="$(date -u '+%F_%H-%M-%S_%Z')"
 
# File to store current backup file
FILE="$MBD/$MyDB.$HOST.$NOW"

# Store list of databases
DBS=""

# Variable containing diff between the two files
DIFFERENCE=""

SEL=""
# Get all database list first

DBS="$(($MYSQL -u $MyUSER -h $MyHOST --password=$MyPASS -Bse 'show databases') | $GREP $MyDB) "

# Check if the database exists
if [ "$?" == 0 ] && [ -n "$DBS" ] ; then
	# Dump the database into sql file
	echo "Dumping the file. This can take a while..."

	echo 'SET FOREIGN_KEY_CHECKS=0;' > $FILE.sql
	$MYSQLDUMP -u $MyUSER -h $MyHOST -p$MyPASS $MyExportVars --databases --no-data $MyDB  >> $FILE.sql
	$MYSQLDUMP -u $MyUSER -h $MyHOST -p$MyPASS $MyExportVars $TABLESTOSKIP --databases $MyDB  >> $FILE.sql
	echo 'SET FOREIGN_KEY_CHECKS=1;' >> $FILE.sql
	
	# Handling the case when the dump is unsuccessful. For example if the database doesn't exist
	if [ "$?" == 0 ] ; then
		DIFFERENCE="$($DIFF $MyDB.sql $FILE.sql) "
	else
		echo "Unknown error occured"
		rm -f $FILE.sql
		exit -1
	fi
#	echo "Backup of the old database in $FILE.sql created"

	if [ "$DIFFERENCE" != "" ] && [ "$DIFFERENCE" != " " ] ; then
		while [ 1 ]
		do
			if [ "$MELD" != "" ] ; then
				echo -n "There are differences in the current database and the new one. (R)eplace, (D)iff, (G)UI Diff, (C)ancel: "
			else
				echo -n "There are differences in the current database and the new one. (R)eplace, (D)iff, (C)ancel: "
			fi
			read SEL
			if [ $SEL == 'R' ] || [ $SEL == 'r' ] ; then
				echo "Importing the new database. This can taka a while..."				
				$MYSQL -u $MyUSER -h $MyHOST -p$MyPASS $MyImportVars < $NewDB
				break
			elif [ $SEL == 'D' ] || [ $SEL == 'd' ] ; then
				echo $DIFFERENCE
			elif [ $SEL == 'G' ] || [ $SEL == 'g' ] ; then
				$MELD $MyDB.sql $FILE.sql
			elif [ $SEL == 'C' ] || [ $SEL == 'c' ] ; then
				echo "User aborted."
				rm -f $FILE.sql
				exit 0
			fi
		done
	else
		echo "The databases are synchronized. Nothing to do. Exitting"	
		rm -f $FILE.sql
		exit 0
	fi
else
	echo -n "The database does not exists. Do you want to create and import? [n/y]: "
	read SEL
	if [ $SEL == 'Y' ] || [ $SEL == 'y' ] ; then
		echo "Creating and importing the new database. This can taka a while..."				
		$MYSQL -u $MyUSER -h $MyHOST -p$MyPASS $MyImportVars < $NewDB
	else
		echo "User aborted"
		exit 0
	fi
fi
if [ "$GZIP" != "" ] && [ -f $FILE.sql ]; then
	echo "Archiving backup ...into $FILE.gz"	
	$GZIP -9 $FILE.sql
	echo "$FILE.gz created"
fi

