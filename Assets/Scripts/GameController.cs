using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    public GameObject[] navPoints;

    // Used to initialise singleton
	void Awake ()
	{
		if (instance != null) throw new System.Exception();

		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		//
	}
	
	// Update is called once per frame
	void Update ()
	{
		//
	}
}
