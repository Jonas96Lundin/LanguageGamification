using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    [SerializeField] List<CargoPosition> cargoPositions = new List<CargoPosition>();

    public List<CargoPosition> CargoPositions { get { return cargoPositions; } }
}
