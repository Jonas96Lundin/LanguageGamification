using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem
{
	public static void SaveQuestions(string gameName, List<string> questions, List<string> answers)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		QuestionData questionData = new QuestionData(questions, answers);

		//string path = Application.dataPath + "/QuestionsAndAnswers/" + gameName + ".save";
		string path = Application.persistentDataPath + "/" + gameName + ".save";
		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, questionData);
		stream.Close();
	}


	public static QuestionData LoadQuestions(string gameName)
	{
		string path = Application.persistentDataPath + "/" + gameName + ".save";
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);

			QuestionData questiondata = formatter.Deserialize(stream) as QuestionData;
			stream.Close();

			return questiondata;
		}
		else
		{
			Debug.Log("Save file not found" + path);
			return null;
		}
	}
}
