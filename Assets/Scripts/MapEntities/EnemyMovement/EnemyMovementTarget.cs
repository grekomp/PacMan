using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
	public class EnemyMovementTarget : EnemyMovementType
	{
		private readonly MapGraphNode targetNode;
		private readonly Action movementFinishedAction;

		public EnemyMovementTarget(MapMovingEntity mapMovingEntity, MapGraphNode targetNode, Action movementFinishedAction) : base(mapMovingEntity)
		{
			this.targetNode = targetNode;
			this.movementFinishedAction = movementFinishedAction;
		}

		public override void Activate()
		{
			base.Activate();

			mapMovingEntity.MoveTo(targetNode);
		}

		public override void HandleMovementFinished()
		{
			base.HandleMovementFinished();

			movementFinishedAction?.Invoke();
		}
	}
}
