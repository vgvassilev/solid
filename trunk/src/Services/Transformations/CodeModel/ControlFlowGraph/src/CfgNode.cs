/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;


using Mono.Cecil.Cil;

namespace SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph 
{

	public class CfgNode
	{
		#region Fields & Properties
		
		public static readonly List<CfgNode> NoSuccessors = new List<CfgNode>(0);
		public static readonly List<CfgNode> NoPredecessor = new List<CfgNode>(0);

		int index;
		public int Index {
			get { return index; }
			set { index = value; }
		}
		
		List<CfgNode> successors;
		public List<CfgNode> Successors {
			get { return successors;  }
			set { successors = value; }
		}		
		
		List<CfgNode> predecessor;		
		public List<CfgNode> Predecessor {
			get { return predecessor;  }
			set { predecessor = value; }
		}

		#endregion
		
		#region Constructors
		
		public CfgNode ()
		{
		}		
		
		#endregion
				
	}
}
