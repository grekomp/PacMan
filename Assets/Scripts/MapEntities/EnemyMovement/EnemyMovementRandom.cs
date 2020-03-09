using Athanor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PacMan
{
	[Serializable]
	public class EnemyMovementRandom : EnemyMovementType
	{
		public EnemyMovementRandom(MapMovingEntity mapMovingEntity) : base(mapMovingEntity) { }

		public override void Activate()
		{
			base.Activate();

			currentDirection = Direction.None;
			if (mapMovingEntity.nodeTo == null)
			{
				MoveToRandomNode();
			}
		}

		public override void HandleMovementFinished()
		{
			base.HandleMovementFinished();

			MoveToRandomNode();
		}

		public void MoveToRandomNode()
		{
			MapGraphNode targetNode = null;
			Direction selectedDirection = (Direction)UnityEngine.Random.Range(1, 4);

			// Try to avoid coming back the same way
			if (selectedDirection == currentDirection.OppositeDirection()) selectedDirection = selectedDirection.NextDirection();

			// Find a valid movement direction
			for (int i = 0; i < 4; i++)
			{
				targetNode = mapMovingEntity.nodeFrom.NodeDirection(selectedDirection);
				if (targetNode != null) break;
				selectedDirection = selectedDirection.NextDirection();
			}

			if (targetNode == null)
			{
				Log.Error(this, "Cannot find a valid target node!");
				return;
			}

			currentDirection = selectedDirection;
			mapMovingEntity.MoveTo(targetNode);
		}
	}
}
