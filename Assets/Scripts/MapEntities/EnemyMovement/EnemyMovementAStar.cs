using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
	public class EnemyMovementAStar : EnemyMovementType
	{
		public EnemyMovementAStar(MapMovingEntity mapMovingEntity) : base(mapMovingEntity)
		{ }

		public override void Activate()
		{
			base.Activate();
		}

		public override void HandleMovementFinished()
		{
			base.HandleMovementFinished();

		}


	}
}
