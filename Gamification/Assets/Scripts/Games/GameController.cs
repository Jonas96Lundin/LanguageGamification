using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	[SerializeField] private PointController pointController;

	public void OnContinue()
	{
		//Save points (pointController.CurrentPoints)
		SceneManager.LoadScene(1);
	}

	public void OnReset()
	{
		//Save points (pointController.CurrentPoints) to database
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void OnQuitGame()
	{
		SceneManager.LoadScene(1);
	}
}
