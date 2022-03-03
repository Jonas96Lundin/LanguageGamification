using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionAdder : MonoBehaviour
{
	[SerializeField] private TMP_Dropdown gameName;
	[SerializeField] private TMP_InputField questionInput;
	[SerializeField] private TMP_InputField answerInput;


	private string question;
	private string answer;
	private string[] questionArray;
	private string[] answerArray;



	public void OnLoadClick()
	{
		QuestionData questionData = SaveSystem.LoadQuestions(gameName.options[gameName.value].text);
		if (questionData != null)
		{
			if (questionData.questions.Count == questionData.answers.Count)
			{
				questionInput.text = "";
				answerInput.text = "";

				for (int i = 0; i < questionData.questions.Count; i++)
				{
					questionInput.text += questionData.questions[i];
					answerInput.text += questionData.answers[i];
					if (i < questionData.questions.Count - 1)
					{
						questionInput.text += "\n";
						answerInput.text += "\n";
					}
				}
				Debug.Log("Questions loaded");
			}
			else
			{
				Debug.Log("Question data and answer data don't add up");
			}
		}
		else
		{
			Debug.Log("No data available");
		}
	}


	public void OnSaveClick()
	{
		List<string> questions = new List<string>();
		List<string> answers = new List<string>();

		questionArray = questionInput.text.Split('\n');
		answerArray = answerInput.text.Split('\n');

		if(questionArray.Length == answerArray.Length)
		{
			for (int i = 0; i < questionArray.Length; i++)
			{
				question = questionArray[i].Replace("\n", "");
				answer = answerArray[i].Replace("\n", "");

				questions.Add(question);
				answers.Add(answer);
			}
			SaveSystem.SaveQuestions(gameName.options[gameName.value].text, questions, answers);

			Debug.Log("Questions saved");
		}
		else
		{
			Debug.Log("Questions and answers needs to be same amount");
		}
		
	}

}
