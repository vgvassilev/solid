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
  public class DataModel
  {
    int updateLockCount = 0;

    private List<IObserver> observers;
    public List<IObserver> Observers {
      get { 
        if (observers == null)
          observers = new List<IObserver>(); 
        return observers; 
      }
      set { observers = value; }
    }

    public Database db;
    public Database DB {
      get { return db; }
      set { db = value; }
    }

    public DataModel(Database db) {
      this.db = db;
    }

    public void BeginUpdate()
    {
      updateLockCount++;
    }

    public void EndUpdate()
    {
      updateLockCount--;
      if (updateLockCount == 0)
        Notify();
    }

    public void Attach(IObserver observer) {
      Observers.Add(observer);
    }

    public void Detach(IObserver observer) {
      observers.Remove(observer);
    }
    
    public void Notify()
    {
      foreach (IObserver observer in Observers)
        observer.Update(this);
    }
  }
}

