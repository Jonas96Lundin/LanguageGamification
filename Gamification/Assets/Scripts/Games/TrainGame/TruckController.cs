using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    [SerializeField] List<Transform> cargoPositions;
    [SerializeField] float xPosOffset;

    public List<Transform> CargoPositions { get { return cargoPositions; } }
    public float PosOffsetX { get { return xPosOffset; } }

    public void ClearCargo()
	{
        foreach(CargoPosition cargoPosition in GetComponentsInChildren<CargoPosition>())
		{
            cargoPosition.DestroyCargo();
        }
	}
}
