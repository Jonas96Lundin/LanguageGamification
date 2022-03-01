using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class QuestionController : MonoBehaviour
{
	private List<string> questions;
	private List<string> answers;

	public void LoadQuestions(string gameName)
	{
		QuestionData questionData = SaveSystem.LoadQuestions(gameName);
		questions = questionData.questions;
		answers = questionData.answers;
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
