using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;

public class ColorWheelGameController : MonoBehaviour
{


	private string gameName = "ColorWheel";
	[SerializeField] private QuestionDataController questionController;

	[SerializeField] private GameObject victoryScreen;

	[SerializeField] private Button uiSpinButton;
	[SerializeField] private TMP_Text uiSpinButtonText;
	[SerializeField] private PickerWheel pickerWheel;

	[SerializeField] private PointController pointController;
	[SerializeField] private TMP_Text pointText;
	[SerializeField] private TMP_Text comboText;

	private List<AnswerController_ColorWheel> answerContollers = new List<AnswerController_ColorWheel>();

	private int wheelPieceIndex;
	private string currentQuestion;
	

	void Start()
	{
		pointController.onAddPoint += DisplayPoints;
		pointController.onSetMultiplier += DisplayCombo;

		//QuestionData questionData = QuestionController.LoadQuestions(gameName);
		questionController.LoadQuestionData(gameName);
		questionController.SetQuestionsAndAnswers(questionController.QuestionData.questions, questionController.QuestionData.answers);
		foreach(AnswerController_ColorWheel answerController in GetComponentsInChildren<AnswerController_ColorWheel>())
		{
			answerContollers.Add(answerController);
		}

		uiSpinButton.onClick.AddListener(() =>
		{
			uiSpinButton.interactable = false;
			uiSpinButtonText.text = "Filage";

			pickerWheel.OnSpinStart(() =>
			{
				Debug.Log("Spin start");
				foreach (AnswerController_ColorWheel answer in answerContollers)
				{
					answer.IncorrectIndicator.SetActive(false);
				}
			});
			pickerWheel.OnSpinEnd(wheelPiece =>
			{
				Debug.Log("Spin end");

				wheelPieceIndex = wheelPiece.Index;
				currentQuestion = wheelPiece.Label;
				foreach (AnswerController_ColorWheel answer in answerContollers)
				{
					answer.AnswerButton.interactable = true;
				}
			});

			pickerWheel.Spin();
		});
	}

	private void ResetWheel()
	{
		uiSpinButton.interactable = true;
		uiSpinButtonText.text = "Tourner";
		
	}

	public void AnswerQuestion(AnswerController_ColorWheel thisAnswerController)
	{
		foreach (AnswerController_ColorWheel answer in answerContollers)
		{
			answer.AnswerButton.interactable = false;
		}

		if(questionController.IsAnswerCorrect(currentQuestion, thisAnswerController.Answer))
		{
			thisAnswerController.CorrectIndicator.SetActive(true);
			answerContollers.Remove(thisAnswerController);
			if(answerContollers.Count > 0)
			{
				pointController.AddPointWithMultiplier(true);
				pickerWheel.ResetWheelWithout(wheelPieceIndex);
				ResetWheel();
			}
			else
			{
				pointController.AddPoint();
				victoryScreen.SetActive(true);
			}
			
		}
		else
		{
			thisAnswerController.IncorrectIndicator.SetActive(true);
			pointController.AddPointWithMultiplier(false);
			ResetWheel();
		}
	}

	private void DisplayPoints(int points)
	{
		pointText.text = "Points: " + points;
	}
	private void DisplayCombo(int combo)
	{
		comboText.text = "Combiné X " + combo;
	}
}
