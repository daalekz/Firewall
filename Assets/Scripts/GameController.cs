﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }
    private WaveController wc;

	public Text WaveDisplay, HealthDisplay;
	public Player PlayerBoi { get; private set; }

    private List<GameObject[]> navPointsArray = new List<GameObject[]>(); // Array of empty game objects used as nodes to navigate between || FIX


	void Start ()
	{
		wc = WaveController.instance;
		PlayerBoi = new Player(100);
	}

  void Awake()
  {
      if (instance != null) throw new System.Exception();
      instance = this;
  }

	void Update ()
	{
		WaveDisplay.text = "Wave: " + wc.WaveCount.ToString();
        


        foreach (GameObject enemy in wc.SpawnedObjects)
        {
            float current_point = enemy.transform.position.x;
            int selected_path = enemy.GetComponent<AIController>().data.PathNum;

            if (current_point >= navPoints[selected_path][navPoints[selected_path].Length - 1].transform.position.x)
            {
                wc.SpawnedObjects.Remove(enemy);
                TowerTools.Destroy(enemy);
                PlayerBoi.ApplyDamage(50);
                break;
            }

        }


        if (PlayerBoi != null)
        {
            HealthDisplay.text = "Health: " + PlayerBoi.Health.ToString();
        }

        Debug.Log(PlayerBoi.Health);
	}

	void OnTriggerExit2D(Collider2D col)
	{
		PlayerBoi.ApplyDamage(col.gameObject.GetComponent<AIController>().data.Damage);
		Destroy(col.gameObject);
	}


    public List<GameObject[]> navPoints
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
}
