using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

	//Active and inactive targets for the panel position
	public Transform Active, Inactive;

	//Current target of the panel
	private Transform Target;


	void Start()
	{
		//Target is set to the inactive position on startup
		Target = Inactive;
	}

	void Update()
	{
		//Moves the panel towards the target position
		transform.position = Vector3.Lerp(transform.position, Target.position, 0.1f);
	}

	//Changes the target position of the pannel to active or inactive
	public void ToggleMenu()
	{
		if (Target == Active)
		{
			Target = Inactive;
		}
		else
		{
			Target = Active;
		}
	}
}
