using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PacMan
{
	public class EnemyMovementAStar : EnemyMovementType
	{
		public class WeightedNode
		{
			public MapGraphNode node;
			public float parentDistance;
			public float pathDistance;
			public float targetDistance;
			public WeightedNode parentNode;

			public float TotalCost => parentDistance + targetDistance;

			public WeightedNode(MapGraphNode node, MapGraphNode targetNode)
			{
				this.node = node;
				this.parentDistance = 0f;
				this.targetDistance = (node.transform.position - targetNode.transform.position).sqrMagnitude;
				parentNode = null;
				pathDistance = 0f;
			}

			public WeightedNode(MapGraphNode node, MapGraphNode targetNode, WeightedNode parentNode)
			{
				this.node = node;
				this.parentDistance = (node.transform.position - parentNode.node.transform.position).sqrMagnitude;
				this.targetDistance = (node.transform.position - targetNode.transform.position).sqrMagnitude;
				this.parentNode = parentNode;
				this.pathDistance = parentNode.pathDistance + parentDistance;
			}
		}

		public MapGraphNode targetNode;

		public EnemyMovementAStar(MapMovingEntity mapMovingEntity, MapGraphNode targetNode) : base(mapMovingEntity)
		{
			this.targetNode = targetNode;
		}

		public override void Activate()
		{
			base.Activate();
			MoveNext();
		}

		public override void HandleMovementFinished()
		{
			base.HandleMovementFinished();

			MoveNext();
		}

		public void MoveNext()
		{
			mapMovingEntity.MoveTo(NextNode(targetNode));
		}
		public MapGraphNode NextNode(MapGraphNode targetNode)
		{
			List<WeightedNode> openNodes = new List<WeightedNode>();
			List<WeightedNode> closedNodes = new List<WeightedNode>();

			openNodes.Add(new WeightedNode(mapMovingEntity.nodeFrom, targetNode));

			while (openNodes.First().node != targetNode)
			{
				openNodes.OrderBy(n => n.TotalCost);

				closedNodes.Add(openNodes.First());
				openNodes.RemoveAt(0);

				for (int i = 0; i < 4; i++)
				{
					var neighbouringNode = closedNodes.Last().node.NodeDirection((Direction)i);
					if (neighbouringNode == null) continue;

					WeightedNode neighbourWeightedNode = closedNodes.Find(n => n.node == neighbouringNode);
					if (neighbourWeightedNode != null) continue;

					neighbourWeightedNode = openNodes.Find(n => n.node == neighbouringNode);
					if (neighbourWeightedNode == null)
					{
						openNodes.Add(new WeightedNode(neighbouringNode, targetNode, closedNodes.Last()));
					}
					else
					{
						float currentParentDistance = (neighbourWeightedNode.node.transform.position - closedNodes.Last().node.transform.position).sqrMagnitude;
						if (neighbourWeightedNode.pathDistance > closedNodes.Last().pathDistance + currentParentDistance)
						{
							neighbourWeightedNode.parentNode = closedNodes.Last();
							neighbourWeightedNode.parentDistance = currentParentDistance;
							neighbourWeightedNode.pathDistance = closedNodes.Last().pathDistance + currentParentDistance;
						}
					}
				}
			}

			WeightedNode currentNode = openNodes.First();
			while (currentNode.parentNode.parentNode != null)
			{
				currentNode = currentNode.parentNode;
			}

			return currentNode.node;
		}
	}
}
