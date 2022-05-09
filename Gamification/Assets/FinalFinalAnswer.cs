using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalFinalAnswer : MonoBehaviour
{
	public bool isSelected;

    public void OnAnswerClick()
	{
		isSelected = !isSelected;

		if (isSelected)
		{
			GetComponent<Image>().color = Color.white;
		}
		else
		{
			GetComponent<Image>().color = Color.grey;
		}
	}
}
