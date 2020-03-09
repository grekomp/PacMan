using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PacMan
{
	public enum GameState
	{
		NotStarted,
		Starting,
		Playing,
		PlayerDying,
		VictoryAnimation,
	}

	public class GameController : DontDestroySingleton<GameController>
	{
		[Header("Global variables")]
		public IntReference score;
		public IntReference highScore;
		public FloatReference enemyDispatchInterval;
		public FloatReference enemyDispatchInitialDelay;


		[Header("Runtime variables")]
		public GameState state;


		Coroutine currentCoroutine;
		Coroutine powerUpCoroutine;


		#region Game flow control
		protected override void Start()
		{
			base.Start();

			currentCoroutine = StartCoroutine(StartGame());
			StartCoroutine(EnemyDispatchCoroutine());
		}

		IEnumerator StartGame()
		{
			state = GameState.Starting;
			DisableControls();
			StopGame();
			yield return PlayGameIntro();
			state = GameState.Playing;
			ResumeGame();
			EnableControls();
		}

		private IEnumerator PlayGameIntro()
		{
			// TODO: Create game intro sequence
			GameSoundController.Instance.PlayIntroMusic();
			yield return new WaitForSecondsRealtime(4.41f);
			GameSoundController.Instance.PlayBgLoop();
		}
		#endregion


		#region Score
		public void ResetScore()
		{
			score.Value = 0;
		}
		public void IncrementScore(int increment)
		{
			score.Value += increment;

			if (highScore.Value < score.Value) highScore.Value = score.Value;
		}
		#endregion


		#region Player death
		[ContextMenu("Player Died")]
		public void PlayerDied()
		{
			if (state == GameState.Playing)
			{
				state = GameState.PlayerDying;
				StopCoroutine(currentCoroutine);
				currentCoroutine = StartCoroutine(PlayerDeathCoroutine());
			}
		}
		IEnumerator PlayerDeathCoroutine()
		{
			Player.instance.DisableControls();
			StopGame();
			GameSoundController.Instance.StopLoops();
			GameSoundController.Instance.PlayGameOverSound();
			yield return Player.instance.PlayDeathAnimation();

			Player.instance.ResetPosition();
			// Reset enemy positions

			GameSoundController.Instance.PlayBgLoop();
			Player.instance.EnableControls();
			ResumeGame();
		}
		#endregion


		#region PowerUp
		public void EnablePowerUp()
		{
			if (powerUpCoroutine != null) StopCoroutine(powerUpCoroutine);
			powerUpCoroutine = StartCoroutine(PowerUpCoroutine());
		}
		IEnumerator PowerUpCoroutine()
		{
			EnemyManager.Instance.StopFlashing();
			EnemyManager.Instance.EnableFleeMode();
			GameSoundController.Instance.PlayPowerUpLoop01();
			yield return new WaitForSeconds(6f);
			GameSoundController.Instance.PlayPowerUpLoop02();
			EnemyManager.Instance.FlashFleeingEnemies();
			yield return new WaitForSeconds(0.272f * 12);
			DisablePowerUp();
		}

		public void DisablePowerUp()
		{
			EnemyManager.Instance.DisableFleeMode();
			GameSoundController.Instance.PlayBgLoop();
		}
		#endregion


		#region Time control
		public void ResumeGame()
		{
			Time.timeScale = 1;
			state = GameState.Playing;
		}
		public void StopGame()
		{
			Time.timeScale = 0;
		}
		#endregion


		#region Player controls
		public void EnableControls()
		{
			Player.instance.EnableControls();
		}
		public void DisableControls()
		{
			Player.instance.DisableControls();
		}
		#endregion


		#region Enemy dispatch
		IEnumerator EnemyDispatchCoroutine()
		{
			yield return new WaitForSeconds(enemyDispatchInterval + enemyDispatchInitialDelay);
			EnemyManager.Instance.DispatchNext();
			GameSoundController.Instance.IncreaseLoopPitch();
			yield return new WaitForSeconds(enemyDispatchInterval);
			EnemyManager.Instance.DispatchNext();
			GameSoundController.Instance.IncreaseLoopPitch();
			yield return new WaitForSeconds(enemyDispatchInterval);
			EnemyManager.Instance.DispatchNext();
			GameSoundController.Instance.IncreaseLoopPitch();
		}
		#endregion
	}
}
