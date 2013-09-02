/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;

using DataMorphose.View;

namespace DataMorphose.Model
{
  /// <summary>
  /// DataModel is the class that represents the whole structure of the data. 
  /// The Observer pattern will be used for all of the classes that need the data.
  /// </summary>
  public class DataModel : SolidV.MVC.Model
  {
    public Database db;
    public Database DB {
      get { return db; }
      set { db = value; }
    }

    public DataModel(Database db) {
      this.db = db;
    }
  }
}

