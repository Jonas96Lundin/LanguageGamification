using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    public void OnColorWheel()
	{
		SceneManager.LoadScene(2);
	}

	public void LogOut()
	{
		SceneManager.LoadScene(0);
	}
}
