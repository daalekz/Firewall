using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuaController : MonoBehaviour {

	public Transform Active, Inactive;
	private Transform Target;

	void Start()
	{
		Target = Inactive;
	}

	void Update()
	{
		transform.position = Vector3.Lerp(transform.position, Target.position, 0.1f);
	}

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
