using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TrainGameController : MonoBehaviour
{
    private string gameName = "TrainGame";

    private Vector2 screenCenter = new Vector2(960, 540);
    private float moveTime = 3;
    

    [Header("Phrases")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private string[] truckWords;
    [SerializeField] private string[] currentTrainphrase;
    private int phraseCounter = 0;

    [SerializeField] string[] questionArray;//plats1 Pierre me donne un cadeau, plats2 Les invités vous ont offert un livre....
    [SerializeField] string[] answerArray; //plats 1 Pierre_me_le_donne, plats2 Les invités_vous_l’ont_offert....
    char splitter = '_';

    [Header("Train")]
    [SerializeField] private GameObject trainStartPos;
    [SerializeField] private GameObject train;
    [SerializeField] private GameObject locomotive;
    [SerializeField] private GameObject wagonWithoutCargo;
    [SerializeField] private GameObject wagonWithCargo;

    private float startWagonXOffset;
    private int trainWagonXOffset = 400;

    private int currentWagon;
    private int numberOfWagons;

    [Header("Cargo")]
    [SerializeField] private GameObject cargoPrefab;



    [Header("Trucks")]
    [SerializeField] private List<GameObject> truckPrefabs;
    [SerializeField] private List<Transform> truckStartPositions;
    private List<TruckController> trucks = new List<TruckController>();


    void Start()
    {
        LoadQuestionData();
        LoadCurrentQuestion();
        CreateTrucks();
        LoadTrucks();
        CreateTrain();
        train.transform.DOMoveX(screenCenter.x, moveTime);
    }

    private void LoadQuestionData()
	{
        QuestionData questionData = SaveSystem.LoadQuestions(gameName);
        questionArray = new string[questionData.questions.Count];
        answerArray = new string[questionData.answers.Count];
        for (int i = 0; i < questionData.questions.Count; i++)
        {
            questionArray[i] = questionData.questions[i];
            answerArray[i] = questionData.answers[i];
        }
    }

    private void LoadCurrentQuestion()
	{
        questionText.text = questionArray[phraseCounter];
        currentTrainphrase = answerArray[phraseCounter].Split(splitter);
        phraseCounter++;
    }

    private void CreateTrain()
	{
		train.transform.position = trainStartPos.transform.position;

		currentWagon = 0;
        numberOfWagons = currentTrainphrase.Length;
        startWagonXOffset = -(trainWagonXOffset * (numberOfWagons + 1) / 2);

        SetWagonPosition(Instantiate(locomotive, train.transform));
        currentWagon++;

        SetWagonPosition(Instantiate(wagonWithCargo, train.transform)).GetComponentInChildren<TMP_Text>().text = currentTrainphrase[0];
        currentWagon++;

		for (; currentWagon < numberOfWagons; currentWagon++)
		{
            SetWagonPosition(Instantiate(wagonWithoutCargo, train.transform));
        }

        SetWagonPosition(Instantiate(wagonWithCargo, train.transform)).GetComponentInChildren<TMP_Text>().text = currentTrainphrase[currentTrainphrase.Length - 1];
    }

    private GameObject SetWagonPosition(GameObject wagon)
	{
        wagon.transform.localPosition = new Vector2(startWagonXOffset + (currentWagon * trainWagonXOffset), wagon.transform.localPosition.y);
        return wagon;
    }

    private void CreateTrucks()
	{
		for (int i = 0; i < truckPrefabs.Count; i++)
		{
            trucks.Add(Instantiate(truckPrefabs[i].GetComponent<TruckController>(), truckStartPositions[i]));
        }
	}

    private void ResetTrucks()
	{
        //Delete truckCargo

		for (int i = 0; i < trucks.Count; i++)
		{
            trucks[i].transform.position = truckStartPositions[i].position;
        }
        LoadTrucks();
	}

    private void LoadTrucks()
	{
        int answer = 0;
        foreach(TruckController truck in trucks)
		{
            foreach(Transform cargoPos in truck.CargoPositions)
			{
                GameObject cargo = Instantiate(cargoPrefab, cargoPos);
                cargo.GetComponentInChildren<TMP_Text>().text = truckWords[answer];
                answer++;
            }
            MoveTruck(truck.gameObject, truck.XPosOffset);
		}
	}

    private void MoveTruck(GameObject truck, float xPosOffset)
	{
        truck.transform.DOMoveX(screenCenter.x + xPosOffset, moveTime);
    }
}
