using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BadgeManager
{
	private static Dictionary<string, bool> aquiredBadges;
	private static List<string> previousBadges;
	public static bool questionSkipped = false;

	public static Dictionary<string, bool> GetAquiredBadges(Games game, float time, int points)
	{
		aquiredBadges = new Dictionary<string, bool>();
		previousBadges = Repository.GetAquiredBadges(game);
		foreach (string prevBadge in previousBadges)
		{
			Debug.Log(prevBadge);
		}

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
			case Games.HOMESCREEN:
				SetCompletionerBadge(game);
				SetPointsCollectorBadge(game);
				SetSpeedDemonBadge(game);
				break;
		}
		return aquiredBadges;
	}

	private static void SetGameCompletedBadge(Games game, string badgeName)
	{
		if (!previousBadges.Contains(badgeName))
		{
			Debug.Log(questionSkipped);
			if (!questionSkipped)
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

	private static void SetCompletionerBadge(Games game)
	{
		if (!previousBadges.Contains("homeCompletioner"))
		{
			if (previousBadges.Contains("colorWheelCompleted") && previousBadges.Contains("trainGameCompleted"))
			{
				Repository.AddBadge("homeCompletioner", game);
				aquiredBadges.Add("homeCompletioner", true);
			}
		}
		else
		{
			aquiredBadges.Add("homeCompletioner", false);
		}
	}
	private static void SetPointsCollectorBadge(Games game)
	{
		if (!previousBadges.Contains("homePointsCollector"))
		{
			if (previousBadges.Contains("colorWheelMaxPoints") && previousBadges.Contains("trainGameMaxPoints"))
			{
				Repository.AddBadge("homePointsCollector", game);
				aquiredBadges.Add("homePointsCollector", true);
			}
		}
		else
		{
			aquiredBadges.Add("homePointsCollector", false);
		}
	}
	private static void SetSpeedDemonBadge(Games game)
	{
		if (!previousBadges.Contains("homeSpeedDemon"))
		{
			if (previousBadges.Contains("colorWheelSpeedster") && previousBadges.Contains("trainGameSpeedster"))
			{
				Repository.AddBadge("homeSpeedDemon", game);
				aquiredBadges.Add("homeSpeedDemon", true);
			}
		}
		else
		{
			aquiredBadges.Add("homeSpeedDemon", false);
		}
	}
}
