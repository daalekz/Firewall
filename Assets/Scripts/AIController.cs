using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
	GameController gc;

	int nodeIndex = 0;

	[Header("Movement")]
	public float speed = 0.5f;
	public float turnDeadzone = 0.3f;
	Vector3 direction, heading;

	// Use this for initialization
	void Start ()
	{
		gc = GameController.instance;

		// Set the initial direction the AI will move in
		direction = gc.navPoints[1].transform.position - gc.navPoints[0].transform.position;
		direction = direction / direction.magnitude;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position += (direction * speed) * Time.deltaTime;
	}

    // Handle changing the AI's direction when it hits a corner
	void ChangeDirection ()
	{
		nodeIndex++;
		direction = gc.navPoints[nodeIndex + 1].transform.position - transform.position;
		direction = direction / direction.magnitude;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		transform.position = col.transform.position;
		ChangeDirection();
	}
}
