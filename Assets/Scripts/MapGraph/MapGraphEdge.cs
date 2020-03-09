using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan
{
	[Serializable]
	public class MapGraphEdge : ScriptableObject
	{
		public MapGraphNode node1;
		public MapGraphNode node2;
	}
}
