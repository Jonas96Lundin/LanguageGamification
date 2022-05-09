using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TrainGame_FinalFinalQuestion : MonoBehaviour
{
    [SerializeField] TrainGameController gameController;
    [SerializeField] PointController pointController;
    [SerializeField] List<FinalFinalAnswer> answers;

    [Header("UI")]
    [SerializeField] TMP_Text uiButtonText;
    private bool questionAnswerd;

    [SerializeField] ParticleSystem particleStar;
    [SerializeField] AudioSource correctAnswerSound;
    [SerializeField] AudioSource incorrectAnswerSound;

    private bool wrongAnswer;
    [SerializeField] GameObject correctAnswerDisplay;

    public void OnAnswer()
    {
        if (!questionAnswerd)
        {
			for (int i = 0; i < answers.Count; i++)
			{
                if(i == 3 || i == 4)
				{
                    
					if (answers[i].isSelected)
					{
                        answers[i].GetComponent<Image>().color = Color.green;
                        
                    }
					else
					{
                        answers[i].GetComponent<Image>().color = Color.yellow;
                        wrongAnswer = true;
                    }

                }
				else
				{
					if (answers[i].isSelected)
					{
                        answers[i].GetComponent<Image>().color = Color.red;
                        wrongAnswer = true;
                    }
				}
			}

			if (!wrongAnswer)
			{
                correctAnswerDisplay.SetActive(true);
                Instantiate(particleStar, correctAnswerDisplay.transform.position, particleStar.transform.rotation);
                correctAnswerDisplay.GetComponentInChildren<TMP_Text>().text = "+10";
                correctAnswerSound.Play();
                pointController.AddPoints(10);
            }
			else
			{
                incorrectAnswerSound.Play();
            }

            uiButtonText.text = "Continuez";
            questionAnswerd = true;
        }
        else
        {
            StartCoroutine(gameController.EndGame());
        }
    }
}
