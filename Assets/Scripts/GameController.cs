using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }
	private WaveController wc;

    private GameObject[] navPointsArray = new GameObject[0]; // Array of empty game objects used as nodes to navigate between || FIX
	public Text WaveDisplay;

	void Awake ()
	{
		if (instance != null) throw new System.Exception();
		instance = this;
	}

	void Start ()
	{
		wc = WaveController.instance;
	}

	void Update ()
	{
		WaveDisplay.text = "Wave: " + wc.WaveCount.ToString();
	}

    public GameObject[] navPoints
    {
        get
        {
            return navPointsArray;
        }

        set
        {
            navPointsArray = value;
        }
    }
}
