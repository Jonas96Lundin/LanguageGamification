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
    [SerializeField] private Button skipButton;
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Timer timer;
    [SerializeField] private TMP_Text uiPoints;
    [SerializeField] private TMP_Text excellentDisplayText;
    [SerializeField] private TMP_Text excellentDisplayPoints;
    private List<string> exelentPharses = new List<string>() { "Excellent", " Bien joué", "Super", "Génial", "Fantastique", "Tu es un champion", "Tu es très fort" };

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
    [SerializeField] private GameObject correctAnswerDisplay;
    [SerializeField] private Image wrongCloud;
    [SerializeField] private GameObject wrongLightning;
    [SerializeField] private TMP_Text wrongText;
    private GameObject answerDisplay;

    private bool noWrongAnswer = true;

    private float checkAnswerDelay = 1;
    private float enterDelay = 3;
    private float exitDelay = 5;
    private float displayScaleTime = 0.5f;
    private float wrongAnswreDisplayTime = 1;

    [SerializeField] ParticleSystem particleStar;

    [Header("Sounds")]
    [SerializeField] private AudioSource correctSound;
    [SerializeField] private AudioSource incorrectSound;
    [SerializeField] private AudioSource trainArriveSound;
    [SerializeField] private AudioSource trainDepartSound;

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
    public void OnTryAgain()
	{
        RemoveBadWeather();
        SwitchToRed();
    }
    public void OnSkipQuestion()
	{
        RemoveBadWeather();
        StartCoroutine(SkipQuestion());
    }

    public void OnNextQuestion()
	{
        correctAnswerDisplay.GetComponentInChildren<Button>().interactable = false;
        timer.StartTimer();
        NextQuestion();
    }

    private void NextQuestion()
	{
        if (questionController.QuestionCounter < questionController.QuestionArray.Length)
        //if (questionController.QuestionCounter < 1)
        {
            answerDisplay.transform.DOScale(0, displayScaleTime);
            noWrongAnswer = true;
            StartCoroutine(gameController.NextGame());
        }
        else
        {
            questionController.QuestionText.text = "";
            timer.StopTimer();
            pointController.AddGameTime(timer.TotalTime);
            EndGame();
            StartCoroutine(gameController.FinalQuestion());
        }
    }

    public void NewTrainSound()
	{
        trainArriveSound.Play();
        StartCoroutine(FadeOutSound(trainArriveSound, enterDelay));
    }

    IEnumerator CheckAnswer()
	{
        yield return new WaitForSeconds(checkAnswerDelay);

        if (questionController.CheckAnswer(gameController.GetAnswer()))
        {
            answerDisplay = excellentDisplay;

            if (noWrongAnswer)
			{
                //answerDisplay = excellentDisplay;
                pointController.AddPoints(2);
                excellentDisplayPoints.text = "2";
                //pointController.AddPoint();
                //Instantiate(particleStar, uiPoints.transform.position, particleStar.transform.rotation);
                //uiPoints.text = pointController.CurrentPoints.ToString();
			}
			else
			{
                //answerDisplay = wellDoneDisplay;
                
                pointController.AddPoints(1);
                excellentDisplayPoints.text = "1";
            }

            int i = Random.Range(0, exelentPharses.Count);
            excellentDisplayText.text = exelentPharses[i];

            Instantiate(particleStar, uiPoints.transform.position, particleStar.transform.rotation);
            uiPoints.text = pointController.CurrentPoints.ToString();

            correctSound.Play();
            EndCurrentQuestion();
            //trainSound.Play();

            //answerDisplay.transform.DOScale(1, displayScaleTime);
            //gameController.EndCurrentGame();

            yield return new WaitForSeconds(exitDelay);
            NextQuestion();
        }
        else
        {
            pointController.AddMisstake();
            StartCoroutine(WrongAnswer());
        }
    }

    private void EndCurrentQuestion()
	{
        trainDepartSound.Play();
        answerDisplay.transform.DOScale(1, displayScaleTime);
        gameController.EndCurrentGame();
        StartCoroutine(FadeOutSound(trainDepartSound, exitDelay));
    }

    IEnumerator WrongAnswer()
	{
        noWrongAnswer = false;
        
        //FadeOutSound(incorrectSound, wrongAnswreDisplayTime);
        // wrongCargoDisplay.transform.DOScale(1, displayScaleTime);

        wrongCloud.DOFade(2, displayScaleTime);
        wrongText.DOFade(2, displayScaleTime);

        yield return new WaitForSeconds(displayScaleTime);

        wrongLightning.gameObject.SetActive(true);
        incorrectSound.time = 0.5f;
        incorrectSound.Play();
        tryAgainButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);

        //yield return new WaitForSeconds(wrongAnswreDisplayTime);

        //wrongLightning.gameObject.SetActive(false);
        //wrongCloud.DOFade(0, displayScaleTime);
        //wrongText.DOFade(0, displayScaleTime);

        //SwitchToRed();
        //skipButton.interactable = true;

        //wrongCargoDisplay.transform.DOScale(0, displayScaleTime);
    }
    private void RemoveBadWeather()
	{
        wrongLightning.gameObject.SetActive(false);
        wrongCloud.DOFade(0, displayScaleTime);
        wrongText.DOFade(0, displayScaleTime);
        tryAgainButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
    }
	#endregion

	IEnumerator SkipQuestion()
	{
        BadgeManager.questionSkipped = true;

        correctAnswerDisplay.GetComponentInChildren<TMP_Text>().text = questionController.GetCorrectAnswer();
		answerDisplay = correctAnswerDisplay;
		//SwitchToGreen();
		EndCurrentQuestion();
        //answerDisplay.transform.DOScale(1, displayScaleTime);
        //gameController.EndCurrentGame();

        
        yield return new WaitForSeconds(exitDelay);
		//trainSound.Stop();
		timer.StopTimer();
		correctAnswerDisplay.GetComponentInChildren<Button>().interactable = true;
	}

	public static IEnumerator FadeOutSound(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
    #region LightSwitch
    private void SwitchToGreen()
    {
        player.IsPaused = true;
        answerButton.interactable = false;
        //skipButton.interactable = false;
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
        //skipButton.interactable = true;
        player.IsPaused = false;

        redLight.gameObject.SetActive(true);

        switchLight.transform.localPosition = redLightPos;
        switchLight.color = Color.red;
        switchLight.gameObject.SetActive(true);
    }
    #endregion
}
