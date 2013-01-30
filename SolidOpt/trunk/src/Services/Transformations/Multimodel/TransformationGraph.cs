/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Transformations.Multimodel
{
  /// <summary>
  /// A graph describing the relationships between the different code transformations.
  /// </summary>
  /// In SolidOpt to build an representation one usually needs transformator from another code
  /// representation. For example, in order to build ControlFlowGraph we need to have IL code and
  /// so on. The original intent is these transformers and code representations keep to piling up.
  /// However we should be able to give any two representations of them and we should get a N step
  /// path between them.
  /// The generic template mechanism, which is used in the implementation of the transformers, 
  /// namely <c>ITransfor<Source, Target></c> implicitly gives us the edges of the nodes, and the 
  /// nodes themselves.
  public class TransformationGraph
  {
    private IServiceProvider serviceProvider;
    public TransformationGraph(IServiceProvider serviceProvider) {
      this.serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Finds the path through given set of transformation services.
    /// </summary>
		/// This is A* based algorithm, see http://en.wikipedia.org/wiki/A*_search_algorithm.
    /// <param name='serviceProvider'>
    /// The set of services.
    /// </param>
    /// <param name='from'>
    /// From which starting node (code representation).
    /// </param>
    /// <param name='to'>
    /// To which end node (code representation).
    /// </param>
    public List<IService> FindPath(IService @from, IService to) {
      List<IService> closedSet = new List<IService>();
      List<IService> openSet = new List<IService>();
      Dictionary<IService, IService> cameFrom = new Dictionary<IService, IService>();
      openSet.Add(@from);
      IService current = null;
      while (openSet.Count > 0) {
        current = openSet[0];
        if (current == to)
          return ReconstructPath(cameFrom, to);
        openSet.Remove(current);
        closedSet.Add(current);
        foreach(IService neighbour in GetNeighbours(current)) {
          if (closedSet.Contains(neighbour))
            continue;
          cameFrom[neighbour] = current;
          if (!openSet.Contains(neighbour))
            openSet.Add(neighbour);
        }
      }
      return null;
    }

    private List<IService> ReconstructPath(Dictionary<IService, IService> cameFrom, IService node){
      List<IService> result = new List<IService>();
      if (cameFrom[node] != null) {
        result = ReconstructPath(cameFrom, cameFrom[node]);
        result.Add(node);
        return result;
      }
      result.Add(node);
      return result;
    }

    private List<IService> GetNeighbours(IService node) {
      // It comes in the form ITransform<Source, Target>, where all services have
      // source === node.
      List<IService> result = new List<IService>();
      foreach (var service in serviceProvider.GetServices(typeof(ITransform<,>))) {
        if (service.GetType().GetGenericArguments()[0] == node.GetType().GetGenericArguments()[0]){
          result.Add(service);
        }
      }
      return result;
    }
  }
}
