using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    public GameObject[] navPoints; // Array of empty game objects used as nodes to navigate between

	void Awake ()
	{
		if (instance != null) throw new System.Exception();
		instance = this;
	}
}
