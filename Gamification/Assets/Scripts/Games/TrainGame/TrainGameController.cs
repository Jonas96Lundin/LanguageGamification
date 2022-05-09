using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class TrainGameController : MonoBehaviour
{
	public static float badgeTime = 300;
	public static int badgePoints = 42;

	private GameController gameController;
	private QuestionController_TrainGame questionController;
	private AnswerController_TrainGame answerController;
	[SerializeField] private Timer timer;
	[SerializeField] private GameObject finalQuestionDisplay;
	[SerializeField] private GameObject finalFinalQuestionDisplay;
	[SerializeField] private GameObject victoryDisplay;
	[SerializeField] private Button quitButton;

	[Header("StartPanel")]
	[SerializeField] private GameObject startPanel;
	[SerializeField] private Button startButton;
	[SerializeField] private TMP_Text startCountDown;

	[Header("Train objects")]
	[SerializeField] private TrainController train;
	[SerializeField] private GameObject locomotivePrefab;
	[SerializeField] private GameObject wagonWithoutCargoPrefab;
	[SerializeField] private GameObject wagonWithCargoPrefab;
	private Vector2 trainStartPos = new Vector2(2500, 121);

	private GameObject locomitive;
	private float locomotiveOffsetX;
	private int trainWagonOffsetX = 400;

	private int wagonCounter;
	private int numberOfWagons;
	private string currentWagonAnswer;


	[Header("Truck objects")]
	[SerializeField] private List<GameObject> truckPrefabs;
	[SerializeField] private List<Transform> truckStartPositions;
	private List<TruckController> trucks = new List<TruckController>();


	[Header("Cargo")]
	[SerializeField] private GameObject cargoPrefab;
	private int truckCargoCounter;


	private float centerMovePosX = 0;
	private float endMovePosX = 20;
	private float enterDelay = 3;
	private float exitDelay = 5;
	private float endGameDelay = 1.5f;
	private float victoryScreenDelay = 0.5f;


	private void OnEnable()
	{
		gameController = GetComponent<GameController>();
		questionController = GetComponent<QuestionController_TrainGame>();
		answerController = GetComponent<AnswerController_TrainGame>();
		train = GetComponentInChildren<TrainController>();
	}

	void Start()
	{
		gameController.SetGame(Games.TRAINGAME);
		questionController.LoadQuestionData();
		BadgeManager.questionSkipped = false;
	}
	public void OnStartGame()
	{
		StartCoroutine(StartGameCountdown());
	}
	//public void OnEndGame()
	//{

	//	StartCoroutine(EndGame());
	//}
	
	private void StartGame()
	{
		questionController.LoadCurrentQuestion();
		//answerController.StartGame();
		CreateTrucks();
		CreateTrain();
	}
	public void EndCurrentGame()
	{
		MoveX(train.transform, -endMovePosX, exitDelay, Ease.InSine);
		foreach (TruckController truck in trucks)
		{
			MoveX(truck.transform, endMovePosX, exitDelay, Ease.InSine);
		}		
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

		MoveX(train.transform, centerMovePosX, enterDelay, Ease.OutSine);
		answerController.NewTrainSound();
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
			foreach (Transform cargoPos in truck.CargoPositions)
			{
				GameObject cargo = Instantiate(cargoPrefab, cargoPos);
				cargo.GetComponentInChildren<TMP_Text>().text = answerController.AnswerWords[truckCargoCounter];
				truckCargoCounter++;
			}
			MoveX(truck.transform, centerMovePosX + truck.PosOffsetX, enterDelay, Ease.OutSine);
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


	public IEnumerator StartGameCountdown()
	{
		startButton.gameObject.SetActive(false);
		startCountDown.gameObject.SetActive(true);

		float time = 3.5f;
		while (time > 1)
		{
			startCountDown.text = time.ToString("F0");
			time -= Time.deltaTime;

			yield return null;
		}

		yield return new WaitForSeconds(.5f);

		startPanel.SetActive(false);
		StartGame();
		timer.StartTimer();

		yield return new WaitForSeconds(enterDelay);

		answerController.StartGame();

	}
	public IEnumerator NextGame()
	{
		StartNextGame();

		yield return new WaitForSeconds(enterDelay);

		answerController.NextGame();
	}
	public IEnumerator FinalQuestion()
	{
		yield return new WaitForSeconds(endGameDelay);
		finalQuestionDisplay.SetActive(true);
		finalQuestionDisplay.transform.DOScale(1, 0.5f);
	}

	public IEnumerator FinalFinalQuestion()
	{
		finalQuestionDisplay.transform.DOScale(0, 0.5f);
		yield return new WaitForSeconds(0.5f);
		finalFinalQuestionDisplay.SetActive(true);
		finalFinalQuestionDisplay.transform.DOScale(1, 0.5f);
	}

	public IEnumerator EndGame()
	{
		quitButton.interactable = false;
		finalFinalQuestionDisplay.transform.DOScale(0, 0.5f);

		yield return new WaitForSeconds(victoryScreenDelay);

		victoryDisplay.SetActive(true);
		gameController.EndGame();
		victoryDisplay.transform.DOScale(1, 0.5f);
	}
}
