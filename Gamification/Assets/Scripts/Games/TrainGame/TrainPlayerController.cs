using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainPlayerController : MonoBehaviour
{
	private bool isPaused = true;

	public bool IsPaused { get { return isPaused; } set { isPaused = value; } }
}
