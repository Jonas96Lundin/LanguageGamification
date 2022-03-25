using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadgeManager
{
	private Games currentGame;

	public void SetupBadgeManager(Games game)
	{
		currentGame = game;
	}

	public void HandleBadges()
	{
		switch (currentGame)
		{
			case Games.COLORWHEEL:

				break;
			case Games.TRAINGAME:

				break;
		}
	}
}
