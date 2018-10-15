using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }
	private WaveController wc;

    private GameObject[] navPointsArray = new GameObject[0]; // Array of empty game objects used as nodes to navigate between || FIX
	public Text WaveDisplay, HealthDisplay;
	public Player PlayerBoi { get; private set; }

	void Awake ()
	{
		if (instance != null) throw new System.Exception();
		instance = this;
	}

	void Start ()
	{
		wc = WaveController.instance;
		PlayerBoi = new Player(100);
	}

	void Update ()
	{
		WaveDisplay.text = "Wave: " + wc.WaveCount.ToString();
		HealthDisplay.text = "Health: " + PlayerBoi.Health.ToString();
	}

	void OnTriggerExit2D(Collider2D col)
	{
		PlayerBoi.ApplyDamage(col.gameObject.GetComponent<AIController>().data.Damage);
		Destroy(col.gameObject);
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
