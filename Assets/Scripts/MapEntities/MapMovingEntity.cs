using Athanor;
using System.Collections;
using UnityEngine;

namespace PacMan
{
	public class MapMovingEntity : MonoBehaviour
	{
		[Header("Events")]
		public GameEventHandler OnMovementFinished;

		[Header("Runtime variables")]
		public MapGraphNode nodeFrom;
		public MapGraphNode nodeTo;
		[Space]
		public FloatReference movementSpeed;

		Coroutine movementCoroutine;

		#region Movement
		public void MoveTo(MapGraphNode targetNode)
		{
			nodeTo = targetNode;

			if (movementCoroutine != null) StopCoroutine(movementCoroutine);

			movementCoroutine = StartCoroutine(Move());
		}
		IEnumerator Move()
		{
			while (true)
			{
				float moveDistance = Time.deltaTime * movementSpeed;
				float remainingDistance = Vector3.Distance(transform.position, nodeTo.transform.position);

				if (moveDistance >= remainingDistance)
				{
					transform.position = nodeTo.transform.position;
					nodeFrom = nodeTo;
					nodeTo = null;
					MovementFinished();
					yield break;
				}
				else
				{
					transform.position += (nodeTo.transform.position - transform.position).normalized * moveDistance;
					yield return null;
				}
			}
		}

		protected virtual void MovementFinished()
		{
			OnMovementFinished?.Raise();
		}
		#endregion
	}
}
