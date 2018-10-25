using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour {

	void Awake()
	{
		GameObject[] objs = GameObject.FindGameObjectsWithTag("BackgroundMusic");

		if (objs.Length > 1)
			Destroy(this.gameObject);

		DontDestroyOnLoad(this.gameObject);
	}
}
