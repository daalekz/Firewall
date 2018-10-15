using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
	public Slider LoadingBar;
	
	public void StartGame()
	{
		LoadingBar.gameObject.SetActive(true);
		StartCoroutine(StartLoadLevel());
	}

	private IEnumerator StartLoadLevel()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync("AITest 1");
		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / 0.9f);
			LoadingBar.value = progress;

			yield return null;
		}
	}
}
