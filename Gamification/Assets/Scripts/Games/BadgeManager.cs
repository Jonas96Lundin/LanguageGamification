using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BadgeManager
{
	private static Dictionary<string, bool> aquiredBadges;
	private static List<string> previousBadges;

	public static Dictionary<string, bool> GetAquiredBadges(Games game, float time, int points)
	{
		aquiredBadges = new Dictionary<string, bool>();
		previousBadges = Repository.GetAquiredBadges(game);

		switch (game)
		{
			case Games.COLORWHEEL:
				SetGameCompletedBadge(game, "colorWheelCompleted");
				SetTimeBadge(game, "colorWheelSpeedster", time, ColorWheelGameController.badgeTime);
				SetPointBadge(game, "colorWheelMaxPoints", points, ColorWheelGameController.badgePoints);
				break;

			case Games.TRAINGAME:
				SetGameCompletedBadge(game, "trainGameCompleted");
				SetTimeBadge(game, "trainGameSpeedster", time, TrainGameController.badgeTime);
				SetPointBadge(game, "trainGameMaxPoints", points, TrainGameController.badgePoints);
				break;
		}
		return aquiredBadges;
	}

	private static void SetGameCompletedBadge(Games game, string badgeName)
	{
		if (!previousBadges.Contains(badgeName))
		{
			Repository.AddBadge(badgeName, game);
			aquiredBadges.Add(badgeName, true);
		}
		else
		{
			aquiredBadges.Add(badgeName, false);
		}
	}

	private static void SetTimeBadge(Games game, string badgeName, float currentTime, float limitTime)
	{
		if (!previousBadges.Contains(badgeName))
		{
			if (currentTime <= limitTime)
			{
				Repository.AddBadge(badgeName, game);
				aquiredBadges.Add(badgeName, true);
			}
		}
		else
		{
			aquiredBadges.Add(badgeName, false);
		}
	}

	private static void SetPointBadge(Games game, string badgeName, int currentPoints, int limitPoints)
	{
		if (!previousBadges.Contains(badgeName))
		{
			if (currentPoints >= limitPoints)
			{
				Repository.AddBadge(badgeName, game);
				aquiredBadges.Add(badgeName, true);
			}
		}
		else
		{
			aquiredBadges.Add(badgeName, false);
		}
	}
}
