using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoPosition : MonoBehaviour
{
    private bool hasCargo;

    public bool HasCargo { get { return hasCargo; } set { hasCargo = value; } }

    public void DestroyCargo()
	{
		if (GetComponentInChildren<CargoController>())
		{
			Destroy(GetComponentInChildren<CargoController>().gameObject);
		}
	}
}
