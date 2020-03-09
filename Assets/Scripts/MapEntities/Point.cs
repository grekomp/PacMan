using Athanor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PacMan
{
	public class Point : MonoBehaviour
	{
		[Header("Variables")]
		public IntReference pointValue;

		public void OnTriggerEnter2D(Collider2D collision)
		{
			if (IsPlayer(collision.gameObject))
			{
				GameController.Instance.IncrementScore(pointValue);
				GameSoundController.Instance.PlayPointPickupSound();
				Destroy(gameObject);
			}
		}

		public bool IsPlayer(GameObject gameObject)
		{
			return gameObject.CompareTag("Player");
		}
	}
}
