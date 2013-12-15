/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

/// <summary>
/// Our model-view-controller implementation. It consists of three kinds of classes. The model is 
/// the data, the view is the (on screen) data presentation and the controller defines the way the
/// system reacts. MVC decouples these three concerns and provides a well-defined communication 
/// protocol between them by introducing a level of indirection.
/// </summary>
namespace SolidV.MVC
{
  /// <summary>
  /// A generic Model class that represents the data in SolidV.
  /// </summary>
  ///
  public class Model : IModel
  {
    private int updateLockCount;
    
    public event ModelChangedDelegate ModelChanged;
    
    private List<IModel> subModels;
    public List<IModel> SubModels {
      get {
        if (subModels == null)
          subModels = new List<IModel>();
        return subModels; 
      }
      set { subModels = value; }
    }
    
    public T GetSubModel<T>() where T: class, IModel
    {
      foreach (IModel item in SubModels){
        
        T result = item as T;
        if( result != null )
          return result;
      }
      return null;
    }
    
    public T UseSubModel<T>() where T: class, IModel, new()
    {
      foreach (IModel item in SubModels){
        
        T result = item as T;
        if( result != null )
          return result;
      }
      return RegisterSubModel<T>();
    }
    
    public T RegisterSubModel<T>() where T: class, IModel, new()
    {
      return this.RegisterSubModel<T>(new T());
    }
    
    public T RegisterSubModel<T>(T subModel) where T: class, IModel
    {
      if (GetSubModel<T>() == null){
        SubModels.Add(subModel);
        return subModel;
      }
      return null;
    }

    public Model()
    {
    }

    /// <summary>
    /// Begins the update of the Model. This method should be called when an update of the model
    /// starts.
    /// </summary>
    public void BeginUpdate()
    {
      updateLockCount++;
    }

    /// <summary>
    /// Ends the update of the Model.
    /// </summary>
    public void EndUpdate()
    {
      updateLockCount--;
      if (updateLockCount == 0)
        OnModelChanged();
    }

    protected void OnModelChanged() {
      if (ModelChanged != null) ModelChanged(this);
    }
  }
}
