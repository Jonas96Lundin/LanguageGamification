using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainPlayerController : MonoBehaviour
{
	private bool isGrabbingCargo;

	public bool IsGrabbingCargo { get { return isGrabbingCargo; } set { isGrabbingCargo = value; } }
}
