using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PacMan
{
	public class MapGraphContainer : MonoBehaviour
	{
		[Header("Nodes")]
		public List<MapGraphNode> nodes = new List<MapGraphNode>();


		#region Finding nodes
		[ContextMenu("Find Nodes")]
		public void FindNodes()
		{
			nodes = GetComponentsInChildren<MapGraphNode>().ToList();
		}
		#endregion
	}
}
