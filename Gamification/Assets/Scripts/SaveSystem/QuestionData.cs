using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionData
{
	public List<string> questions;
	public List<string> answers;

	public QuestionData(List<string> questions, List<string> answers)
	{
		this.questions = questions;
		this.answers = answers;
	}
}
