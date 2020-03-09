using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
	public class EnemyReturnToBaseMovement : EnemyMovementType
	{
		Action movementFinishedAction;
		MapGraphNode baseNode;

		public EnemyReturnToBaseMovement(MapMovingEntity mapMovingEntity, MapGraphNode baseNode, Action movementFinishedAction) : base(mapMovingEntity)
		{
			this.movementFinishedAction = movementFinishedAction;
			this.baseNode = baseNode;
		}

		public override void Activate()
		{
			base.Activate();

			mapMovingEntity.MoveTo(baseNode);
		}

		public override void HandleMovementFinished()
		{
			base.HandleMovementFinished();

			movementFinishedAction?.Invoke();
		}
	}
}
