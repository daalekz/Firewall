using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public static Spawner instance { get; private set; }

	void Awake ()
	{
		if (instance != null) throw new System.Exception();
		instance = this;
	}

	public void Spawn(GameObject obj)
	{
		Instantiate(obj, transform.position, transform.rotation);
	}
}
