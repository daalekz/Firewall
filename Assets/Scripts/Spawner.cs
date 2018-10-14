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

    //returns game object, so that active game objects can be accessed
	public GameObject Spawn(GameObject obj)
	{
		return Instantiate(obj, transform.position, transform.rotation);
	}
}
