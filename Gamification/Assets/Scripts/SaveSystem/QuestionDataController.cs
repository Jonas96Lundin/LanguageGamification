using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class QuestionDataController : MonoBehaviour
{
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

	public bool IsAnswerCorrect(string question, string answer)
	{
		Debug.Log("Question: " + question + " Answer: " + answer);
		for (int i = 0; i < questions.Count; i++)
		{
			if (question.ToLower() == questions[i].ToLower())
			{
				Debug.Log("Answer: " + answers[i]);
				Debug.Log(question + answer);
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
