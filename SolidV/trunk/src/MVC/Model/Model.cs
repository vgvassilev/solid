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
  /// A generic Model class that represents the data in SolidV. It is the master model, which can
  /// contain many submodels, which store extra information such as selection.
  /// </summary>
  ///
  public class Model : IModel
  {
    private int updateLockCount;
    
    public event ModelChangedDelegate ModelChanged;
    
    private Dictionary<Type, IModel> subModels = new Dictionary<Type, IModel>();

    /// <summary>
    /// The enumerator for the sub models.
    /// </summary>
    /// <returns>The sub models enumerator.</returns>
    /// 
    public IEnumerable<IModel> GetSubModelsEnumerator() {
      foreach (var item in subModels)
        yield return item.Value;
    }

    /// <summary>
    /// Gets a sub model if it was already registered.
    /// </summary>
    /// <returns>The sub model, null if it wasn't registered.</returns>
    /// <typeparam name="T">The type of the sub model to get.</typeparam>
    /// 
    public T GetSubModel<T>() where T: class, IModel
    {
      IModel result = null;
      subModels.TryGetValue(typeof(T), out result);

      return result as T;
    }

    /// <summary>
    /// Gets the or registers a sub model.
    /// </summary>
    /// <returns>The registered or already existant sub model.</returns>
    /// <typeparam name="T">The type of the sub module to get or register.</typeparam>
    ///
    public T GetOrRegisterSubModel<T>() where T: class, IModel, new()
    {
      T result = GetSubModel<T>();
      if (result != null)
        return result;
      return RegisterSubModel<T>();
    }

    /// <summary>
    /// Registers a sub model.
    /// </summary>
    /// <returns>The registered sub model, null if it already exists.</returns>
    /// <typeparam name="T">The type of the submodel to register.</typeparam>
    /// 
    private T RegisterSubModel<T>() where T: class, IModel, new() {
      return this.RegisterSubModel<T>(new T());
    }

    /// <summary>
    /// Registers a sub model.
    /// </summary>
    /// <returns>The registered model, null if it already existes.</returns>
    /// <param name="subModel">The externally created sub model.</param>
    /// <typeparam name="T">The the type of the sub model.</typeparam>
    /// 
    private T RegisterSubModel<T>(T subModel) where T: class, IModel
    {
      if (GetSubModel<T>() == null){
        subModels.Add(typeof(T), subModel);
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
