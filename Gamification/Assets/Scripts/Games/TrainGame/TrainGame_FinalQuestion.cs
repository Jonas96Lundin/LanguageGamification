using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class TrainGame_FinalQuestion : MonoBehaviour
{
    [SerializeField] TrainGameController gameController;
    [SerializeField] PointController pointController;
    [SerializeField] List<string> answers;
    List<string> shuffledAnswers = new List<string>();
    private string correctAnswer = "1.LE COMPLÉMENT D’OBJET INDIRECT (COI)_2.LE COMPLÉMENT D’OBJET DIRECT (COD)_SAUF À LA TROISIÈME PERSONNE OÙ L’ORDRE EST INVERSÉ: 1 (COD), 2 (COI)";
    int currentAnswerIndex = -1;

    [Header("UI")]
    [SerializeField] TMP_Text uiButtonText;
    [SerializeField] ToggleGroup uiAnswerToggles;
    [SerializeField] List<Text> uiAnswers;
    [SerializeField] List<GameObject> pointDisplay;
    [SerializeField] List<GameObject> checkMarks;
    [SerializeField] List<GameObject> xMarks;
    private bool questionAnswerd;
    private char splitter = '_';

    [SerializeField] ParticleSystem particleStar;
    [SerializeField] AudioSource correctAnswerSound;
    [SerializeField] AudioSource incorrectAnswerSound;

    private void Start()
    {
        int x = answers.Count;
        for (int i = 0; i < x; i++)
        {
            int rand = Random.Range(0, answers.Count);
            shuffledAnswers.Add(answers[rand]);
            answers.RemoveAt(rand);
        }
        for (int i = 0; i < shuffledAnswers.Count; i++)
		{
            string answer = shuffledAnswers[i];
            uiAnswers[i].text = answer.Replace(splitter.ToString(), "\n");
        }
    }

    public void OnToggleAnswer(int answerIndex)
	{
        currentAnswerIndex = answerIndex;
        uiAnswerToggles.allowSwitchOff = false;
    }

    public void OnAnswer()
	{
		if (!questionAnswerd)
		{
            if (currentAnswerIndex != -1)
            {
                if (shuffledAnswers[currentAnswerIndex] == correctAnswer)
                {
                    pointDisplay[currentAnswerIndex].SetActive(true);
                    Instantiate(particleStar, pointDisplay[currentAnswerIndex].transform.position, particleStar.transform.rotation);
                    //pointDisplay[currentAnswerIndex].GetComponentInChildren<TMP_Text>().text = "+" + pointController.CurrentPoints.ToString();
                    correctAnswerSound.Play();
                    pointController.AddPoints(10);
                }
                else
                {
                    xMarks[currentAnswerIndex].SetActive(true);
                    for (int i = 0; i < shuffledAnswers.Count; i++)
                    {
                        if (shuffledAnswers[i] == correctAnswer)
                        {
                            checkMarks[i].SetActive(true);

                        }
                    }
                    incorrectAnswerSound.Play();
                }

                uiButtonText.text = "Continuez";
                questionAnswerd = true;
            }
        }
		else
		{
            StartCoroutine(gameController.FinalFinalQuestion());
        }
	}
}
