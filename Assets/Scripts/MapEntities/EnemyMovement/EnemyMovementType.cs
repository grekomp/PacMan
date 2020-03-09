using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
	[Serializable]
	public class EnemyMovementType
	{
		// Components
		protected MapMovingEntity mapMovingEntity;

		// Runtime variables
		public bool isActive;
		public Direction currentDirection;


		#region Constructors
		public EnemyMovementType(MapMovingEntity mapMovingEntity)
		{
			this.mapMovingEntity = mapMovingEntity;
		}
		#endregion


		#region State management
		public virtual void Activate()
		{
			isActive = true;
			mapMovingEntity.OnMovementFinished.RegisterListenerOnce(HandleMovementFinished);
		}
		public virtual void Deactivate()
		{
			isActive = false;
			mapMovingEntity.OnMovementFinished.DeregisterListener(HandleMovementFinished);
		}
		#endregion


		#region Movement events 
		public virtual void HandleMovementFinished() { }
		#endregion
	}
}
