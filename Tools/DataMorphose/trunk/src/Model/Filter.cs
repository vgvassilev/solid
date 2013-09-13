// /*
//  * $Id$
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
using System;

namespace DataMorphose
{
  public class Filter
  {
    public enum ConditionRelations {
      GreaterThan, // >
      LessThan // <
    }

    private ConditionRelations conditionRelation;

    private int condition;
    public int Condition {
      get { return condition; }
      set { condition = value; }
    }   

    public Filter(int condition, ConditionRelations conditionRelation) {
      this.condition = condition;
      this.conditionRelation = conditionRelation;
    }
  }
}

