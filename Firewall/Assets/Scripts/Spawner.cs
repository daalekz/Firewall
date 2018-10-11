using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public float spawnRate = 0.5f;
	float spawnTimer = 0.0f;

	[Header("Spawnable Units")]
    public GameObject dummyUnit;

	float timer;

	// Use this for initialization
	void Start ()
	{
		//
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Spawner time logic
		spawnTimer += Time.deltaTime;
		if (spawnTimer >= spawnRate)
		{
		    Instantiate(dummyUnit, transform.position, transform.rotation);
			spawnTimer = 0.0f;
		}
	}
}
