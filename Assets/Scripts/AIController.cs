using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
	GameController gc;

	int nodeIndex = 0;

	[Header("Movement")]
	public float speed = 0.5f;
	Vector3 direction;

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
		// Move the AI towards the current node
		transform.position += (direction * speed) * Time.deltaTime;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		// Check when we need to turn
		transform.position = col.transform.position;
		ChangeDirection();
	}

    // Handle changing the AI's direction when it hits a corner
	void ChangeDirection ()
	{
		nodeIndex++; // Change the node we will now move towards
		direction = gc.navPoints[nodeIndex + 1].transform.position - transform.position;
		direction = direction / direction.magnitude;
	}
}