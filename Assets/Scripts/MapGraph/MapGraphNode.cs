using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace PacMan
{
	public class MapGraphNode : MonoBehaviour
	{
		[Header("Neighbouring nodes")]
		public MapGraphNode nodeUp;
		public MapGraphNode nodeRight;
		public MapGraphNode nodeDown;
		public MapGraphNode nodeLeft;


		#region Neighbour helper methods
		public MapGraphNode NodeDirection(Direction movementDirection)
		{
			switch (movementDirection)
			{
				case Direction.None:
					return null;
				case Direction.Up:
					return nodeUp;
				case Direction.Right:
					return nodeRight;
				case Direction.Down:
					return nodeDown;
				case Direction.Left:
					return nodeLeft;
				default:
					return null;
			}
		}
		#endregion


#if UNITY_EDITOR
		#region Generating graph
		[ContextMenu("AutoAssignNodeUp")]
		public void AutoAssignNeighbourUp()
		{
			AutoAssignNeighbour(ref nodeUp, (bestNode, currentNode) =>
			{
				if (currentNode == this) return false;

				if (currentNode.transform.position.x == transform.position.x && currentNode.transform.position.y > transform.position.y)
				{
					if (bestNode == null || (bestNode.transform.position - transform.position).sqrMagnitude > (currentNode.transform.position - transform.position).sqrMagnitude)
					{
						return true;
					}
				}

				return false;
			});
		}
		[ContextMenu("AutoAssignNodeRight")]
		public void AutoAssignNeighbourRight()
		{
			AutoAssignNeighbour(ref nodeRight, (bestNode, currentNode) =>
			{
				if (currentNode == this) return false;

				if (currentNode.transform.position.y == transform.position.y && currentNode.transform.position.x > transform.position.x)
				{
					if (bestNode == null || (bestNode.transform.position - transform.position).sqrMagnitude > (currentNode.transform.position - transform.position).sqrMagnitude)
					{
						return true;
					}
				}

				return false;
			});
		}

		[ContextMenu("AutoAssignNodeDown")]
		public void AutoAssignNeighbourDown()
		{
			AutoAssignNeighbour(ref nodeDown, (bestNode, currentNode) =>
			{
				if (currentNode == this) return false;

				if (currentNode.transform.position.x == transform.position.x && currentNode.transform.position.y < transform.position.y)
				{
					if (bestNode == null || (bestNode.transform.position - transform.position).sqrMagnitude > (currentNode.transform.position - transform.position).sqrMagnitude)
					{
						return true;
					}
				}

				return false;
			});
		}
		[ContextMenu("AutoAssignNodeLeft")]
		public void AutoAssignNeighbourLeft()
		{
			AutoAssignNeighbour(ref nodeLeft, (bestNode, currentNode) =>
			{
				if (currentNode == this) return false;

				if (currentNode.transform.position.y == transform.position.y && currentNode.transform.position.x < transform.position.x)
				{
					if (bestNode == null || (bestNode.transform.position - transform.position).sqrMagnitude > (currentNode.transform.position - transform.position).sqrMagnitude)
					{
						return true;
					}
				}

				return false;
			});
		}

		public void AutoAssignNeighbour(ref MapGraphNode neighbour, Func<MapGraphNode, MapGraphNode, bool> condition)
		{
			var nodes = GameObject.FindObjectsOfType<MapGraphNode>();
			MapGraphNode bestNode = null;

			foreach (var node in nodes)
			{
				if (condition(bestNode, node))
				{
					bestNode = node;
				}
			}

			neighbour = bestNode;

			EditorUtility.SetDirty(this);
		}
		#endregion
#endif
	}
}
