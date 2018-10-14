using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }
	private WaveController wc;

    public GameObject[] navPoints; // Array of empty game objects used as nodes to navigate between
	public Text WaveDisplay;

	void Awake ()
	{
		if (instance != null) throw new System.Exception();
		instance = this;
	}

    public WaveController WaveController
    {
        get
        {
            return wc;
        }

        set
        {
            wc = value;
        }
    }

	void Start ()
	{
		wc = WaveController.instance;
	}

	void Update ()
	{
		WaveDisplay.text = "Wave: " + wc.WaveCount.ToString();
	}
}
