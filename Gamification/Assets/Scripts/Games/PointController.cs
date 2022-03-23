using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointController : MonoBehaviour
{
	public event Action<int> onAddPoint;
	public event Action<int> onSetMultiplier;
	public event Action<int> onAddMisstake;
	//public event Action<float> onAddGameTime;

	[SerializeField] private int pointAmount;

	private int pointMultiplier = 1;
	private int currentPoints = 0;
	private int currentMisstakes = 0;
	private float gameTime = 0;

	public int CurrentPoints { get { return currentPoints; } }
	public float GameTime { get { return gameTime; } }

	public void AddPoint()
	{
		currentPoints += (pointAmount * pointMultiplier);
		onAddPoint?.Invoke(currentPoints);
	}

	public void AddPointWithMultiplier(bool isCorrect)
	{
		if (isCorrect)
		{
			currentPoints += (pointAmount * pointMultiplier);
			pointMultiplier++;
			onAddPoint?.Invoke(currentPoints);
			onSetMultiplier?.Invoke(pointMultiplier);
		}
		else
		{
			pointMultiplier = 1;
			onSetMultiplier?.Invoke(pointMultiplier);
		}
	}

	public void AddMisstake()
	{
		currentMisstakes++;
		onAddMisstake?.Invoke(currentMisstakes);
	}

	public void AddGameTime(float time)
	{
		gameTime = time;
		//onAddGameTime?.Invoke(gameTime);
	}
}
