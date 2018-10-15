﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class AIController : MonoBehaviour
{
	GameController gc;

	int nodeIndex = 0;

	public EnemyType Type;
	public Enemy data;
	private Vector3 direction;

	// Use this for initialization
	void Start ()
	{
		gc = GameController.instance;

		switch (Type)
		{
			case EnemyType.DDoS:
			    data = new DDoS();
				break;
			
			case EnemyType.Worm:
			    data = new Worm();
				break;

			case EnemyType.Spyware:
			    data = new Spyware();
				break;
		}

        System.Random rnd = new System.Random();
        data.PathNum = rnd.Next(0, gc.navPoints.Count);


        direction = gc.navPoints[data.PathNum][1].transform.position - gc.navPoints[data.PathNum][0].transform.position;

        //set that path number here


        // Set the initial direction the AI will move in
        direction = gc.navPoints[data.PathNum][1].transform.position - gc.navPoints[data.PathNum][0].transform.position;
        direction = direction / direction.magnitude;
        
    }

    // Update is called once per frame
    void Update ()
	{
        if (data != null)
        {
            // Move the AI towards the current node
            transform.position += (direction * data.Speed) * Time.deltaTime;
            //specifies z (so that it is rendered ontop)
            transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        }
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		// Check if we have hit the end of the path
		if (col.tag == "Finish")
		{
			Destroy(this.gameObject);
		}
		else
		{
		    // Check when we need to turn
		    transform.position = col.transform.position;
		    ChangeDirection();
		}
	}

    // Handle changing the AI's direction when it hits a corner
	void ChangeDirection ()
	{
		nodeIndex++; // Change the node we will now move towards
		direction = gc.navPoints[data.PathNum][nodeIndex + 1].transform.position - transform.position;
		direction = direction / direction.magnitude;
	}

    public int NavPointNum
    {
        get
        {
            return nodeIndex;
        }
    }

    public int AIPathNum
    {
        get
        {
            return data.PathNum;
        }

        set
        {
            data.PathNum = value;
        }
    }

    public Enemy EnemyObj
    {
        get
        {
            return data;
        }

        set
        {
            data = value;
        }
    }
}