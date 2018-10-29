using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	public AudioSource spawnSource;
	public AudioSource playerDamageSource;
	public AudioSource shootSource;

	public void ShootSoundEffect()
	{
		shootSource.Play();
	}

	public void PlayerDamageSoundEffect()
	{
		playerDamageSource.Play();
	}

	public void SpawnSoundEffect()
	{
		spawnSource.Play();
	}
}
