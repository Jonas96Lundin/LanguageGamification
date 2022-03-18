using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerController_TrainGame : MonoBehaviour
{
    TrainGameController gameController;
    QuestionController_TrainGame questionController;

    [SerializeField] Button answerButton;
    [SerializeField] private string[] answerWords;

    [Header("Light Switch")]
    [SerializeField] private Image greenLight;
    [SerializeField] private Image redLight;
    [SerializeField] private Light switchLight;
    Vector3 redLightPos = new Vector3(0, 39, -1);
    Vector3 greenLightPos = new Vector3(0, -39, -1);

    float answerDelay = 1;

    public string[] AnswerWords { get { return answerWords; } }

	private void OnEnable()
	{
        gameController = GetComponent<TrainGameController>();
        questionController = GetComponent<QuestionController_TrainGame>();
    }
	void Start()
    {
        ActivateRedLight();
    }

    public void NextGame()
	{
        SwitchToRed();
    }

	#region Answer
	public void OnLockAnswer()
    {
        SwitchToGreen();
        StartCoroutine(CheckAnswer());
    }

	IEnumerator CheckAnswer()
	{
        yield return new WaitForSeconds(answerDelay);

        if (questionController.CheckAnswer(gameController.GetAnswer()))
        {
            gameController.EndCurrentGame();
        }
        else
        {
            SwitchToRed();
        }
        
    }
	#endregion

	#region LightSwitch
	private void SwitchToGreen()
    {
        answerButton.interactable = false;
        LightsOff();
        ActivateGreenLight();
    }

    private void SwitchToRed()
    {
        LightsOff();
        ActivateRedLight();
        answerButton.interactable = true;
    }

    private void LightsOff()
    {
        switchLight.gameObject.SetActive(false);

        greenLight.gameObject.SetActive(false);
        redLight.gameObject.SetActive(false);
    }
    private void ActivateGreenLight()
    {
        greenLight.gameObject.SetActive(true);

        switchLight.transform.localPosition = greenLightPos;
        switchLight.color = Color.green;
        switchLight.gameObject.SetActive(true);
    }
    private void ActivateRedLight()
    {
        redLight.gameObject.SetActive(true);

        switchLight.transform.localPosition = redLightPos;
        switchLight.color = Color.red;
        switchLight.gameObject.SetActive(true);
    }
    #endregion
}
