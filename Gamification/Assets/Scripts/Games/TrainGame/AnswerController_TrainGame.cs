using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnswerController_TrainGame : MonoBehaviour
{
    private TrainPlayerController player;
    private TrainGameController gameController;
    private QuestionController_TrainGame questionController;
    private PointController pointController;

    [SerializeField] private string[] answerWords;
    [SerializeField] private Button answerButton;
    [SerializeField] private Timer timer;

    [Header("Light Switch")]
    [SerializeField] private Image greenLight;
    [SerializeField] private Image redLight;
    [SerializeField] private Light switchLight;
    private Vector3 redLightPos = new Vector3(0, 39, -1);
    private Vector3 greenLightPos = new Vector3(0, -39, -1);

    [Header("Display objects")]
    [SerializeField] private GameObject wellDoneDisplay;
    [SerializeField] private GameObject excellentDisplay;
    [SerializeField] private GameObject wrongCargoDisplay;
    private GameObject answerDisplay;

    private bool noWrongAnswer = true;

    private float checkAnswerDelay = 1;
    private float displayScaleTime = 0.5f;
    private float wrongAnswreDisplayTime = 1;

    public string[] AnswerWords { get { return answerWords; } }


	private void OnEnable()
	{
        player = GetComponent<TrainPlayerController>();
        gameController = GetComponent<TrainGameController>();
        questionController = GetComponent<QuestionController_TrainGame>();
        pointController = GetComponent<PointController>();
    }
	public void StartGame()
    {
        //answerButton.interactable = true;
        //player.IsPaused = false;
        ActivateRedLight();
    }
    public void HideAnwerDisplay()
	{
        answerDisplay.transform.DOScale(0, displayScaleTime);
    }
    public void NextGame()
	{
        SwitchToRed();
        //answerDisplay.transform.DOScale(0, displayScaleTime);
    }

    public void EndGame()
	{
        LightsOff();
        answerDisplay.transform.DOScale(0, displayScaleTime);
    }

	#region Answer
	public void OnLockAnswer()
    {
        SwitchToGreen();
        StartCoroutine(CheckAnswer());
    }

	IEnumerator CheckAnswer()
	{
        yield return new WaitForSeconds(checkAnswerDelay);

        if (questionController.CheckAnswer(gameController.GetAnswer()))
        {
			if (noWrongAnswer)
			{
                answerDisplay = excellentDisplay;
                pointController.AddPoint();
			}
			else
			{
                answerDisplay = wellDoneDisplay;
                noWrongAnswer = true;
            }
            answerDisplay.transform.DOScale(1, displayScaleTime);
            gameController.EndCurrentGame();

            //if (questionController.QuestionCounter < questionController.QuestionArray.Length)
            if (questionController.QuestionCounter < 2)
			{
                StartCoroutine(gameController.NextGame());
            }
			else
			{
                timer.StopTimer();
                pointController.AddGameTime(timer.TotalTime);
                StartCoroutine(gameController.EndGame());
            }
        }
        else
        {
            pointController.AddMisstake();
            StartCoroutine(WrongAnswer());
        }
    }

    IEnumerator WrongAnswer()
	{
        SwitchToRed();
        noWrongAnswer = false;
        wrongCargoDisplay.transform.DOScale(1, displayScaleTime);

        yield return new WaitForSeconds(wrongAnswreDisplayTime);

        wrongCargoDisplay.transform.DOScale(0, displayScaleTime);
    }
	#endregion

	#region LightSwitch
	private void SwitchToGreen()
    {
        player.IsPaused = true;
        answerButton.interactable = false;
        LightsOff();
        ActivateGreenLight();
    }

    private void SwitchToRed()
    {
        LightsOff();
        ActivateRedLight();
        
    }

    private void LightsOff()
    {
        switchLight.gameObject.SetActive(false);

        greenLight.gameObject.SetActive(false);
        redLight.gameObject.SetActive(false);
    }
    private void ActivateGreenLight()
    {
        greenLight.gameObject.SetActive(true);

        switchLight.transform.localPosition = greenLightPos;
        switchLight.color = Color.green;
        switchLight.gameObject.SetActive(true);
    }
    private void ActivateRedLight()
    {
        answerButton.interactable = true;
        player.IsPaused = false;

        redLight.gameObject.SetActive(true);

        switchLight.transform.localPosition = redLightPos;
        switchLight.color = Color.red;
        switchLight.gameObject.SetActive(true);
    }
    #endregion
}
