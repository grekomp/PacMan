using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundController : DontDestroySingleton<GameSoundController>
{
	[Header("Components")]
	public AudioSource introMusic;
	public AudioSource gameOverMusic;

	public AudioSource pointPickup01;
	public AudioSource pointPickup02;
	public bool playSecondPickupSound;

	public AudioSource bgLoop;
	public AudioSource powerUpLoop01;
	public AudioSource powerUpLoop02;


	#region Sfx
	public void PlayPointPickupSound()
	{
		playSecondPickupSound = !playSecondPickupSound;

		if (playSecondPickupSound)
		{
			pointPickup02.PlayDelayed(-0.1f);
		}
		else
		{
			pointPickup01.PlayDelayed(-0.1f);
		}
	}
	#endregion


	#region Loops
	public void PlayBgLoop()
	{
		powerUpLoop01.Stop();
		powerUpLoop02.Stop();
		bgLoop.Play();
	}
	public void PlayPowerUpLoop01()
	{
		powerUpLoop01.Play();
		powerUpLoop02.Stop();
		bgLoop.Stop();
	}
	public void PlayPowerUpLoop02()
	{
		powerUpLoop01.Stop();
		powerUpLoop02.Play();
		bgLoop.Stop();
	}
	public void StopLoops()
	{
		powerUpLoop01.Stop();
		powerUpLoop02.Stop();
		bgLoop.Stop();
	}
	#endregion


	#region Music
	public void PlayIntroMusic()
	{
		introMusic.Play();
	}
	public void PlayGameOverSound()
	{
		gameOverMusic.Play();
	}

	public void IncreaseLoopPitch()
	{
		bgLoop.pitch += 0.1f;
	}
	#endregion
}
