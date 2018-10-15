using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public static UIController instance;
	public GameObject WaveButton;

	void Awake ()
	{
		if (instance != null) throw new System.Exception();
		instance = this;
	}

	public void ToggleWaveButtonView()
	{
		if (!WaveButton.activeSelf)
		{
			WaveButton.SetActive(true);
		}
		else
		{
			WaveButton.SetActive(false);
		}
	}
}
