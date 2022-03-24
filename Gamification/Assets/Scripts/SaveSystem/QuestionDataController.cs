using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class QuestionDataController : MonoBehaviour
{
	[SerializeField] private GameObject answerButtons;

	private QuestionData questionData;

	private List<string> questions;
	private List<string> answers;

	public QuestionData QuestionData { get { return questionData; } }

	public void LoadQuestionData(string gameName)
	{
		questionData = SaveSystem.LoadQuestions(gameName);
	}

	public void SetQuestionsAndAnswers(List<string> que, List<string> ans)
	{
		questions = que;
		answers = ans;
	}

	public void StartGame()
	{
		answerButtons.SetActive(true);
	}

	public bool IsAnswerCorrect(string question, string answer)
	{
		for (int i = 0; i < questions.Count; i++)
		{
			if (question.ToLower() == questions[i].ToLower())
			{
				if (answer == answers[i])
				{
					return true;
				}
				break;
			}
		}
		return false;
	}
}
