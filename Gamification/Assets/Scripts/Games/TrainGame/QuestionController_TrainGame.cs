using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionController_TrainGame : MonoBehaviour
{
    QuestionDataController questionDataController;

    [SerializeField] private TMP_Text questionText;

    private string[] questionArray;
    private string[] answerArray;
    private string[] currentAnswerphrase;

    private int questionCounter = 0;
    private char splitter = '_';

    public string[] QuestionArray { get { return questionArray; } }
    public string[] AnswerArray { get { return answerArray; } }
    public string[] CurrentAnswerphrase { get { return currentAnswerphrase; } set { currentAnswerphrase = value; } }
    public int QuestionCounter { get { return questionCounter; } }

	private void OnEnable()
	{
        questionDataController = GetComponent<QuestionDataController>();
    }

    public void LoadQuestionData()
    {
        questionDataController.LoadQuestionData(Games.TRAINGAME.ToString());

        questionArray = questionDataController.QuestionData.questions.ToArray();
        answerArray = questionDataController.QuestionData.answers.ToArray();

        List<string> answers = new List<string>();
        foreach (string answerData in questionDataController.QuestionData.answers)
        {
            string answer = answerData.Replace(splitter.ToString(), " ");
            answers.Add(answer);
        }
        questionDataController.SetQuestionsAndAnswers(questionDataController.QuestionData.questions, answers);
    }

    public void LoadCurrentQuestion()
    {
        questionText.text = questionArray[questionCounter];
        currentAnswerphrase = answerArray[questionCounter].Split(splitter);
        questionCounter++;
    }

    public bool CheckAnswer(string answer)
    {
        if (questionDataController.IsAnswerCorrect(questionText.text, answer))
        {
            return true;
        }
        return false;
    }
}
