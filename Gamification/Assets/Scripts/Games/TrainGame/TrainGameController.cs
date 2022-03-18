    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TrainGameController : MonoBehaviour
{
    QuestionController_TrainGame questionController;
    AnswerController_TrainGame answerController;


    [Header("Train")]
    private Vector2 trainStartPos = new Vector2(2500, 121);
    [SerializeField] private TrainController train;
    [SerializeField] private GameObject locomotivePrefab;
    [SerializeField] private GameObject wagonWithoutCargoPrefab;
    [SerializeField] private GameObject wagonWithCargoPrefab;

    private GameObject locomitive;
    private float locomotiveOffsetX;
    private int trainWagonOffsetX = 400;

    private int wagonCounter;
    private int numberOfWagons;
    private string currentWagonAnswer;


    [Header("Trucks")]
    [SerializeField] private List<GameObject> truckPrefabs;
    [SerializeField] private List<Transform> truckStartPositions;
    private List<TruckController> trucks = new List<TruckController>();


    [Header("Cargo")]
    [SerializeField] private GameObject cargoPrefab;
    private int truckCargoCounter;


    private float centerMovePosX = 960;
    private float trainEndMovePosX = -1000;
    private float truckEndMovePosX = 3000;
    private float enterTime = 3;
    private float exitTime = 5;


	private void OnEnable()
	{
        questionController = GetComponent<QuestionController_TrainGame>();
        answerController = GetComponent<AnswerController_TrainGame>();
        train = GetComponentInChildren<TrainController>();
    }

	void Start()
    {
        questionController.LoadQuestionData();
        questionController. LoadCurrentQuestion();
        CreateTrucks();
        CreateTrain();
    }
    public void EndCurrentGame()
	{
        MoveX(train.transform, trainEndMovePosX, exitTime, Ease.InSine);
        foreach (TruckController truck in trucks)
        {
            MoveX(truck.transform, truckEndMovePosX, exitTime, Ease.InSine);
        }
        StartCoroutine(NextGame());
    }
    private void StartNextGame()
	{
        questionController.LoadCurrentQuestion();
        ReloadTrucks();
        ReloadTrain();
    }

    #region Train
    private void CreateTrain()
	{
        train.transform.localPosition = trainStartPos;

        numberOfWagons = questionController.CurrentAnswerphrase.Length;
        locomotiveOffsetX = -(trainWagonOffsetX * (numberOfWagons + 1) / 2);

        CreateLocomotive(Instantiate(locomotivePrefab, train.transform));
        
        wagonCounter = 1;
        CreateWagonWithCargo(Instantiate(wagonWithCargoPrefab, locomitive.transform), 0);
        wagonCounter++;

		for (; wagonCounter < numberOfWagons; wagonCounter++)
		{
            CreateWagonWithoutCargo(Instantiate(wagonWithoutCargoPrefab, locomitive.transform));
        }

        CreateWagonWithCargo(Instantiate(wagonWithCargoPrefab, locomitive.transform), numberOfWagons - 1);

        MoveX(train.transform, centerMovePosX, enterTime, Ease.OutSine);
    }
    
    private void CreateLocomotive(GameObject newlocomotive)
	{
        newlocomotive.transform.localPosition = new Vector2(locomotiveOffsetX, 0);
        locomitive = newlocomotive;
    }
    private void CreateWagonWithCargo(GameObject wagon, int cargoPhraseIndex)
	{
        CreateWagonWithoutCargo(wagon);
        wagon.GetComponentInChildren<TMP_Text>().text = questionController.CurrentAnswerphrase[cargoPhraseIndex];
    }
    private void CreateWagonWithoutCargo(GameObject wagon)
	{
        wagon.transform.localPosition = new Vector2(wagonCounter * trainWagonOffsetX, wagon.transform.localPosition.y);
        train.CargoPositions.Add(wagon.GetComponentInChildren<CargoPosition>());
    }

	private void ResetTrain()
	{
        train.CargoPositions.Clear();
        Destroy(locomitive);
        train.transform.localPosition = trainStartPos;
    }
    private void ReloadTrain()
	{
        ResetTrain();
        CreateTrain();
	}
	#endregion

	#region Trucks
	private void CreateTrucks()
	{
		for (int i = 0; i < truckPrefabs.Count; i++)
		{
            trucks.Add(Instantiate(truckPrefabs[i].GetComponent<TruckController>(), truckStartPositions[i]));
        }
        LoadTrucks();
    }
    private void LoadTrucks()
	{
        truckCargoCounter = 0;
        foreach (TruckController truck in trucks)
		{
            foreach(Transform cargoPos in truck.CargoPositions)
			{
                GameObject cargo = Instantiate(cargoPrefab, cargoPos);
                cargo.GetComponentInChildren<TMP_Text>().text = answerController.AnswerWords[truckCargoCounter];
                truckCargoCounter++;
            }
            MoveX(truck.transform, centerMovePosX + truck.PosOffsetX, enterTime, Ease.OutSine);
		}
	}

    private void ResetTrucks()
    {
        for (int i = 0; i < trucks.Count; i++)
        {
            trucks[i].transform.position = truckStartPositions[i].position;
            trucks[i].ClearCargo();
        }
    }
    private void ReloadTrucks()
    {
        ResetTrucks();
        LoadTrucks();
    }
    #endregion

	private void MoveX(Transform trans, float distanceX, float moveTime, Ease ease = Ease.Unset)
	{
        trans.DOMoveX(distanceX, moveTime).SetEase(ease);
	}

    public string GetAnswer()
    {
        currentWagonAnswer = string.Empty;
        foreach (CargoPosition cargoPosition in train.CargoPositions)
        {
            if (!cargoPosition.GetComponentInChildren<TMP_Text>())
                return string.Empty;
            currentWagonAnswer += cargoPosition.GetComponentInChildren<TMP_Text>().text + " ";
        }
        return currentWagonAnswer.Trim(' ');
    }

    IEnumerator NextGame()
	{
        yield return new WaitForSeconds(exitTime);

        answerController.NextGame();
        StartNextGame();
    }
}
