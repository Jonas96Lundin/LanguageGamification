using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;
using DG.Tweening;

public class ColorWheelGameController : MonoBehaviour
{
	public static float badgeTime = 60;
	public static int badgePoints = 55;

	[Header("StartPanel")]
	[SerializeField] private GameObject startPanel;
	[SerializeField] private Button startButton;
	[SerializeField] private TMP_Text startCountDown;

	private GameController gameController;

	[SerializeField] private Timer timer;
	[SerializeField] private PickerWheel wheel;
	[SerializeField] private GameObject answerButtons;

	[SerializeField] private QuestionDataController questionController;

	[SerializeField] private GameObject victoryScreen;
	private int victoryScreenEndScale = 1;
	private float displayScaleTimer = 0.5f;

	[SerializeField] private Button uiSpinButton;
	[SerializeField] private TMP_Text uiSpinButtonText;
	[SerializeField] private PickerWheel pickerWheel;
	[SerializeField] private Button quitButton;

	[SerializeField] private PointController pointController;
	[SerializeField] private TMP_Text pointText;
	[SerializeField] private TMP_Text comboText;

	private List<AnswerController_ColorWheel> answerContollers = new List<AnswerController_ColorWheel>();

	private int wheelPieceIndex;
	private string currentQuestion;
	private bool correctAnswer;
	private int currentAnswerAttempt;
	private int answerAttempts = 3;

	[SerializeField] private ParticleSystem particleStar;
	[SerializeField] private AudioSource correctSound;
	[SerializeField] private AudioSource incorrectSound;

	AnswerController_ColorWheel correctAnswerController = new AnswerController_ColorWheel();

	private void OnEnable()
	{
		gameController = GetComponent<GameController>();
		gameController.SetGame(Games.COLORWHEEL);
	}

	void Start()
	{
		pointController.onAddPoint += DisplayPoints;
		pointController.onSetMultiplier += DisplayCombo;

		questionController.LoadQuestionData(gameController.CurrentGame.ToString());
		questionController.SetQuestionsAndAnswers(questionController.QuestionData.questions, questionController.QuestionData.answers);
		BadgeManager.questionSkipped = false;
	}

	public void OnStartGame()
	{
		StartCoroutine(StartGameCountdown());
	}

	public void StartGame()
	{
		wheel.Create();
		uiSpinButton.interactable = true;
		answerButtons.SetActive(true);

		foreach (AnswerController_ColorWheel answerController in GetComponentsInChildren<AnswerController_ColorWheel>())
		{
			answerContollers.Add(answerController);
		}

		uiSpinButton.onClick.AddListener(() =>
		{
			uiSpinButton.interactable = false;
			//uiSpinButtonText.text = "Filage";

			pickerWheel.OnSpinStart(() =>
			{
				if (correctAnswer || currentAnswerAttempt == answerAttempts)
				{
					if(currentAnswerAttempt == answerAttempts)
					{
						correctAnswerController.MissIndicator.GetComponent<Image>().color = Color.black;
					}
					currentAnswerAttempt = 0;
					pickerWheel.ResetWheelWithout(wheelPieceIndex);
				}
				
				//Debug.Log("Spin start");
				foreach (AnswerController_ColorWheel answer in answerContollers)
				{
					answer.IncorrectIndicator.SetActive(false);
				}
			});
			pickerWheel.OnSpinEnd(wheelPiece =>
			{
				//Debug.Log("Spin end");

				wheelPieceIndex = wheelPiece.Index;
				currentQuestion = wheelPiece.Label;
				foreach (AnswerController_ColorWheel answer in answerContollers)
				{
					answer.AnswerButton.interactable = true;
				}
			});

			pickerWheel.Spin();
		});

		timer.StartTimer();
	}

	private void ResetWheel()
	{
		uiSpinButton.interactable = true;
		//uiSpinButtonText.text = "Tourner";

	}

	public void AnswerQuestion(AnswerController_ColorWheel thisAnswerController)
	{
		if (questionController.IsAnswerCorrect(currentQuestion, thisAnswerController.Answer))
		{
			foreach (AnswerController_ColorWheel answer in answerContollers)
			{
				answer.AnswerButton.interactable = false;
			}

			correctAnswer = true;
			correctSound.Play();
			thisAnswerController.CorrectIndicator.SetActive(true);
			answerContollers.Remove(thisAnswerController);
			if (answerContollers.Count > 0)
			{
				pointController.AddPointWithMultiplier(true);
				//pickerWheel.ResetWheelWithout(wheelPieceIndex);
				ResetWheel();
			}
			else
			{
				timer.StopTimer();
				pointController.AddGameTime(timer.TotalTime);
				pointController.AddPoint();
				quitButton.interactable = false;
				victoryScreen.SetActive(true);
				gameController.EndGame();
				victoryScreen.transform.DOScale(victoryScreenEndScale, displayScaleTimer);
				//Instantiate(particleWanderingSpirits, UICamera.transform);
			}
			currentAnswerAttempt = 0;
		}
		else
		{
			correctAnswer = false;
			incorrectSound.Play();
			currentAnswerAttempt++;
			pointController.AddPointWithMultiplier(false);

			if (currentAnswerAttempt == answerAttempts)
			{
				//currentAnswerAttempt = 0;

				//FIND AND REMOVE CORRECT ANSWER
				string correctAnswer = questionController.FindCorrectAnswer(currentQuestion);
				foreach (AnswerController_ColorWheel answerController in answerContollers)
				{
					answerController.AnswerButton.interactable = false;
					if(answerController.Answer == correctAnswer)
					{
						correctAnswerController = answerController;
					}
				}
				correctAnswerController.MissIndicator.SetActive(true);
				answerContollers.Remove(correctAnswerController);
				BadgeManager.questionSkipped = true;
				if (answerContollers.Count > 0)
				{
					ResetWheel();
				}
				else
				{
					timer.StopTimer();
					pointController.AddGameTime(timer.TotalTime);
					pointController.AddPoint();
					quitButton.interactable = false;
					victoryScreen.SetActive(true);
					gameController.EndGame();
					victoryScreen.transform.DOScale(victoryScreenEndScale, displayScaleTimer);
				}
			}
			else
			{
				thisAnswerController.AnswerButton.interactable = false;
			}
			thisAnswerController.IncorrectIndicator.SetActive(true);
		}
	}

	private void DisplayPoints(int points)
	{
		Instantiate(particleStar, pointText.transform.position, particleStar.transform.rotation);
		pointText.text = points.ToString();
	}
	private void DisplayCombo(int combo)
	{
		comboText.text = "Combin? X " + combo;
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
	}

	//private IEnumerator ShowBadges()
	//{
	//	yield return new WaitForSeconds(1f);
	//	gameController.ShowNewBadges();
	//}
}
