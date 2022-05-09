using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerController_ColorWheel : MonoBehaviour
{
	[SerializeField] ColorWheelGameController gameController;

	[SerializeField] private string answer;
	[SerializeField] private Button answerButton;
	[SerializeField] private GameObject correctIndicator;
	[SerializeField] private GameObject incorrectIndicator;
	[SerializeField] private GameObject missIndicator;

	public string Answer { get { return answer; } set { answer = value; } }
	public Button AnswerButton { get { return answerButton; } set { answerButton = value; } }
	public GameObject CorrectIndicator { get { return correctIndicator; } set { correctIndicator = value; } }
	public GameObject IncorrectIndicator { get { return incorrectIndicator; } set { incorrectIndicator = value; } }
	public GameObject MissIndicator { get { return missIndicator; } set { missIndicator = value; } }


	public void OnButtonClick()
	{
		gameController.AnswerQuestion(this);
	}
}
