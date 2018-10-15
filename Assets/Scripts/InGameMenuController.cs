using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuController : MonoBehaviour {

	//Active and inactive targets for the panel position
	public Transform Active, Inactive;

	//Panels for each structure
	public GameObject DefenderPanel, ScannerPanel, ExtractorPanel, IsolatorPanel, AnnihilatorPanel;

	//A dictionary holding each button with their associated panel
	private Dictionary<GameObject, GameObject> _buttons;

	//Current target of the panel
	private Transform Target;


	void Start()
	{
		//Target is set to the inactive position on startup
		Target = Inactive;

		//Deactivates all panels at startup
		DefenderPanel.SetActive(false);
		ScannerPanel.SetActive(false);
		ExtractorPanel.SetActive(false);
		IsolatorPanel.SetActive(false);
		AnnihilatorPanel.SetActive(false);
	}

	void Update()
	{
		//Moves the panel towards the target position
		transform.position = Vector3.Lerp(transform.position, Target.position, 0.1f);

		//
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

	public void CursorEnterButton(GameObject panel)
	{
		panel.SetActive(true);
	}

	public void CursorExitButton(GameObject panel)
	{
		panel.SetActive(false);
	}
}
