using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan
{
	public enum EnemyState
	{
		Idle,
		ExitingBase,
		FollowingPlayer,
		FleeMode,
		ReturningToBase,
	}

	public class Enemy : MonoBehaviour
	{
		[Header("Components")]
		public MapMovingEntity mapMovingEntity;
		public GameObject eyeRetinas;

		public GameObject fleeingEnemy;
		public SpriteRenderer fleeingEnemyBody;
		public SpriteRenderer fleeingEnemyEyes;
		public GameObject body;

		public AudioSource enemyDeathAudio;

		[Header("Settings")]
		public Vector3[] eyeRetinaPositions;
		public FloatReference playerFollowMovementSpeed;
		public FloatReference fleeMovementSpeed;
		public FloatReference returnToBaseMovementSpeed;
		public FloatReference exitBaseMovementSpeed;
		public FloatReference idleMovementSpeed;
		[Space]
		public MapGraphNode baseNode;
		public MapGraphNode originNode;
		[Space]
		public Color fleeingEnemyColor01;
		public Color fleeingEnemyEyesColor01;
		public Color fleeingEnemyColor02;
		public Color fleeingEnemyEyesColor02;
		public FloatReference fleeingEnemyFlashingInterval;

		[Header("Runtime variables")]
		public EnemyState state = EnemyState.FollowingPlayer;
		public bool isActive;
		public bool canKillPlayer = true;

		EnemyMovementType currentMovementType;
		EnemyMovementRandom randomMovement;
		EnemyMovementRandom fleeMovement;
		EnemyReturnToBaseMovement returnToBaseMovement;
		EnemyMovementTarget exitBaseMovement;
		EnemyMovementRandom idleMovement;

		Coroutine flashingCoroutine;

		#region Initialization
		void Awake()
		{
			randomMovement = new EnemyMovementRandom(mapMovingEntity);
			fleeMovement = new EnemyMovementRandom(mapMovingEntity);
			idleMovement = new EnemyMovementRandom(mapMovingEntity);
			returnToBaseMovement = new EnemyReturnToBaseMovement(mapMovingEntity, baseNode, SetExitBaseState);
			exitBaseMovement = new EnemyMovementTarget(mapMovingEntity, originNode, SetPlayerFollowState);

			if (state == EnemyState.Idle)
			{
				SetIdleInBaseState();
			}
			else
			{
				SetPlayerFollowState();
			}
		}
		#endregion


		#region Movement
		void Update()
		{
			RotateEyes(currentMovementType.currentDirection);
		}

		private void ActivateMovementType(EnemyMovementType movementType)
		{
			movementType.Activate();
			currentMovementType = movementType;
		}
		private void RotateEyes(Direction currentDirection)
		{
			if (currentDirection == Direction.None)
			{
				eyeRetinas.transform.localPosition = eyeRetinaPositions[0];
			}
			else
			{
				eyeRetinas.transform.localPosition = eyeRetinaPositions[(int)currentDirection];
			}
		}
		#endregion


		#region States
		public void SetFleeState()
		{
			currentMovementType?.Deactivate();

			StopFlashing();
			state = EnemyState.FleeMode;
			fleeingEnemy.SetActive(true);
			body.SetActive(true);
			mapMovingEntity.movementSpeed.Value = fleeMovementSpeed;

			canKillPlayer = false;

			ActivateMovementType(fleeMovement);
		}
		public void SetPlayerFollowState()
		{
			currentMovementType?.Deactivate();

			StopFlashing();
			state = EnemyState.FollowingPlayer;
			fleeingEnemy.SetActive(false);
			body.SetActive(true);
			mapMovingEntity.movementSpeed.Value = playerFollowMovementSpeed;

			canKillPlayer = true;

			ActivateMovementType(randomMovement);
		}

		public void SetReturnToBaseState()
		{
			currentMovementType?.Deactivate();

			StopFlashing();
			state = EnemyState.ReturningToBase;
			canKillPlayer = false;
			fleeingEnemy.SetActive(false);
			body.SetActive(false);
			mapMovingEntity.movementSpeed.Value = returnToBaseMovementSpeed;

			ActivateMovementType(returnToBaseMovement);
		}
		public void SetExitBaseState()
		{
			currentMovementType?.Deactivate();
			state = EnemyState.ExitingBase;

			fleeingEnemy.SetActive(false);
			body.SetActive(true);
			mapMovingEntity.movementSpeed.Value = exitBaseMovementSpeed;

			ActivateMovementType(exitBaseMovement);
		}
		public void SetIdleInBaseState()
		{
			currentMovementType?.Deactivate();
			state = EnemyState.Idle;

			fleeingEnemy.SetActive(false);
			body.SetActive(true);
			mapMovingEntity.StopAllCoroutines();
			mapMovingEntity.movementSpeed.Value = idleMovementSpeed;
			mapMovingEntity.nodeFrom = baseNode;

			ActivateMovementType(idleMovement);
		}
		#endregion


		#region Flashing fleeing enemy
		public void StartFlashing()
		{
			StopFlashing();

			flashingCoroutine = StartCoroutine(FlashingCoroutine());
		}
		public void StopFlashing()
		{
			if (flashingCoroutine != null) StopCoroutine(flashingCoroutine);
			SetNormalColors();
		}
		IEnumerator FlashingCoroutine()
		{
			while (true)
			{
				SetAltColors();
				yield return new WaitForSeconds(fleeingEnemyFlashingInterval);
				SetNormalColors();
				yield return new WaitForSeconds(fleeingEnemyFlashingInterval);
			}
		}

		public void SetNormalColors()
		{
			fleeingEnemyBody.color = fleeingEnemyColor01;
			fleeingEnemyEyes.color = fleeingEnemyEyesColor01;
		}
		public void SetAltColors()
		{
			fleeingEnemyBody.color = fleeingEnemyColor02;
			fleeingEnemyEyes.color = fleeingEnemyEyesColor02;
		}
		#endregion


		#region Player collisions
		private void OnTriggerEnter2D(Collider2D collision)
		{

			if (collision.gameObject.CompareTag("Player"))
			{
				if (canKillPlayer)
				{
					GameController.Instance.PlayerDied();
				}
				if (state == EnemyState.FleeMode)
				{
					enemyDeathAudio.Play();
					GameController.Instance.IncrementScore(100);
					SetReturnToBaseState();
				}
			}
		}
		#endregion
	}
}
