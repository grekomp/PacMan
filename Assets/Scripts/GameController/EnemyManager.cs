using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan
{
	public class EnemyManager : DontDestroySingleton<EnemyManager>
	{
		[Header("Runtime variables")]
		public List<Enemy> enemies = new List<Enemy>();


		#region Flee mode
		public void EnableFleeMode()
		{
			foreach (var enemy in enemies)
			{
				if (enemy.state == EnemyState.FollowingPlayer)
				{
					enemy.SetFleeState();
				}
			}
		}
		public void DisableFleeMode()
		{
			foreach (var enemy in enemies)
			{
				if (enemy.state == EnemyState.FleeMode)
				{
					enemy.SetPlayerFollowState();
				}
			}
		}
		public void FlashFleeingEnemies()
		{
			foreach (var enemy in enemies)
			{
				if (enemy.state == EnemyState.FleeMode)
				{
					enemy.StartFlashing();
				}
			}
		}

		public void StopFlashing()
		{
			foreach (var enemy in enemies)
			{
				enemy.StopFlashing();
			}
		}
		#endregion


		#region Dispatching more enemies
		public void DispatchNext()
		{
			foreach (var enemy in enemies)
			{
				if (enemy.state == EnemyState.Idle)
				{
					enemy.SetExitBaseState();
					break;
				}
			}
		}
		#endregion
	}
}
