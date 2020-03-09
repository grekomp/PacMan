using Athanor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PacMan
{
	public class Player : Singleton<Player>
	{
		[Header("Components")]
		public GameObject playerSprite;
		public Animator animator;
		public MapMovingEntity mapMovingEntity;


		[Header("Options")]
		public MapGraphNode startingNode;


		[Header("Runtime variables")]
		public Direction movementDirection;
		public Direction nextMovementDirection;
		public Direction continuationDirection;


		InputConfig controls;


		#region Public properties
		public bool IsMoving => movementDirection != Direction.None;
		#endregion


		#region Initialization
		public void Awake()
		{
			mapMovingEntity.OnMovementFinished.RegisterListenerOnce(HandleMovementFinished);

			controls = new InputConfig();
			controls.Player.Movement.performed += ctx => MoveVector2(ctx.ReadValue<Vector2>());
		}

		private void OnEnable()
		{
			controls.Enable();
		}

		private void OnDisable()
		{
			controls.Disable();
		}
		public override void InitNewInstance() { }
		#endregion


		#region Input
		public void MoveVector2(Vector2 moveDirection)
		{
			if (moveDirection == Vector2.up) MoveUp();
			if (moveDirection == Vector2.right) MoveRight();
			if (moveDirection == Vector2.down) MoveDown();
			if (moveDirection == Vector2.left) MoveLeft();
		}
		#endregion


		#region Movement
		public void EnableControls()
		{
			controls.Enable();
		}
		public void DisableControls()
		{
			controls.Disable();
		}

		[ContextMenu("MoveUp")]
		public void MoveUp() => MoveDirection(Direction.Up);
		[ContextMenu("MoveRight")]
		public void MoveRight() => MoveDirection(Direction.Right);
		[ContextMenu("MoveDown")]
		public void MoveDown() => MoveDirection(Direction.Down);
		[ContextMenu("MoveLeft")]
		public void MoveLeft() => MoveDirection(Direction.Left);
		public void MoveDirection(Direction targetDirection)
		{
			if (targetDirection == Direction.None) return;
			if (targetDirection == movementDirection) return;

			MapGraphNode targetNode = null;
			if (IsMoving == false)
			{
				targetNode = mapMovingEntity.nodeFrom.NodeDirection(targetDirection);
			}
			else
			{
				if (movementDirection == targetDirection.OppositeDirection())
				{
					targetNode = mapMovingEntity.nodeFrom;
					mapMovingEntity.nodeFrom = mapMovingEntity.nodeTo;
				}
				else
				{
					nextMovementDirection = targetDirection;
					return;
				}
			}

			if (targetNode != null)
			{
				movementDirection = targetDirection;
				nextMovementDirection = targetDirection;
				continuationDirection = targetDirection;
				mapMovingEntity.MoveTo(targetNode);
				animator.SetBool("IsMoving", true);
				RotateDirection(targetDirection);
			}
		}

		private void RotateDirection(Direction targetDirection)
		{
			playerSprite.transform.rotation = Quaternion.AngleAxis(((int)targetDirection + 1) * 90, Vector3.back);
		}

		private void StopMovement()
		{
			movementDirection = Direction.None;
			nextMovementDirection = Direction.None;
			continuationDirection = Direction.None;

			mapMovingEntity.StopAllCoroutines();
			mapMovingEntity.nodeFrom = startingNode;
			mapMovingEntity.nodeTo = null;
		}

		public void HandleMovementFinished()
		{
			movementDirection = Direction.None;
			animator.SetBool("IsMoving", false);

			if (CanMoveInDirection(nextMovementDirection))
			{
				MoveDirection(nextMovementDirection);
			}
			else
			{
				MoveDirection(continuationDirection);
			}
		}

		protected bool CanMoveInDirection(Direction targetDirection)
		{
			return mapMovingEntity.nodeFrom.NodeDirection(targetDirection) != null;
		}
		#endregion


		#region Player death
		public IEnumerator PlayDeathAnimation()
		{
			animator.SetBool("Dead", true);
			yield return new WaitForSecondsRealtime(2f);
			animator.SetBool("Dead", false);
		}
		public void ResetPosition()
		{
			StopMovement();
			transform.position = startingNode.transform.position;
		}
		#endregion
	}
}
