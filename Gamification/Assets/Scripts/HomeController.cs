using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
	[SerializeField] GameObject HomePanel;
	[SerializeField] GameObject ColorWheelPanel;
    [SerializeField] GameObject TrainGamePanel;
    [SerializeField] TMP_Text leaderboardTitle;
	[SerializeField] TMP_Text leaderboardNames;
	[SerializeField] TMP_Text leaderboardScores;
    [SerializeField] TMP_Text leaderboardTimes;
    [SerializeField] GameObject CommonBadges;
    [SerializeField] GameObject ColorWheelBadges;
    [SerializeField] GameObject TrainGameBadges;

    [SerializeField] List<Image> badges;
    List<string> aquiredBadges;

    [SerializeField] GameController gameController;

    [SerializeField] TMP_Text ColorWheelDesc;
    [SerializeField] TMP_Text TrainGameDesc;

    static int maxLeaderboardPositions = 5;

    private void Start()
    {
        gameController.SetGame(Games.HOMESCREEN);
        gameController.SetBadges();
    }

    public void OpenColorWheelPanel()
    {
        HomePanel.SetActive(false);
        ColorWheelPanel.SetActive(true);
        GetLeaderboard(Games.COLORWHEEL);
        ColorWheelBadges.SetActive(true);
        CommonBadges.SetActive(false);
        ShowBadges(Games.COLORWHEEL);
    }
    public void OpenTrainGamePanel()
    {
        HomePanel.SetActive(false);
        TrainGamePanel.SetActive(true);
        GetLeaderboard(Games.TRAINGAME);
        TrainGameBadges.SetActive(true);
        CommonBadges.SetActive(false);
        ShowBadges(Games.TRAINGAME);
    }
    public void BackToHomePanel(string panelToClose)
    {
        GameObject.Find(panelToClose).SetActive(false);
        HomePanel.SetActive(true);
        CommonBadges.SetActive(true);
        ColorWheelBadges.SetActive(false);
        TrainGameBadges.SetActive(false);
        //TODO show common badges here
    }

    private void GetLeaderboard(Games game)
    {
        //Dictionary<string, int> leaderboard = new Dictionary<string, int>();
        leaderboardNames.text = "";
        leaderboardScores.text = "";
        leaderboardTimes.text = "";
        switch (game)
        {
            case Games.COLORWHEEL:
                //leaderboard = Repository.GetColorWheelLeaderboard();
                leaderboardTitle.text = "Classement palette de couleurs";
                //ShowPointsLeaderboard(Repository.GetColorWheelLeaderboard());
                ShowPointsAndTimeLeaderboard(Repository.GetColorWheelLeaderboard());

                leaderboardNames.text += "\n" + ".........." + "\n" + "Votre meilleur score: ";
                leaderboardScores.text += "\n" + ".........." + "\n" + Repository.GetBestResult(Games.COLORWHEEL)[0] + "p";
                DisplayTime(Repository.GetBestResult(Games.COLORWHEEL)[1], true);

                break;
            case Games.TRAINGAME:
                //leaderboard = Repository.GetTraingameLeaderboard();
                //leaderboardTitle.text = "Palette de couleurs Classement";
                leaderboardTitle.text = "Classement du jeu de trains";
                ShowPointsAndTimeLeaderboard(Repository.GetTraingameLeaderboard());

                leaderboardNames.text += "\n" + ".........." + "\n" + "Votre meilleur score: ";
                leaderboardScores.text += "\n" + ".........." + "\n" + Repository.GetBestResult(Games.TRAINGAME)[0] + "pts";
                DisplayTime(Repository.GetBestResult(Games.TRAINGAME)[1], true);
                break;
        }



    }
    private void ShowPointsLeaderboard(Dictionary<string, int> leaderboard)
    {
        int leaderboardCounter = 0;
        foreach (KeyValuePair<string, int> pair in leaderboard)
        {
            leaderboardCounter++;
            leaderboardNames.text += "\n" + leaderboardCounter + ": " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pair.Key);

            leaderboardScores.text += "\n" + pair.Value + "pts";
            if (leaderboardCounter >= 3)
            {
                break;
            }
        }
    }
    private void ShowPointsAndTimeLeaderboard(Dictionary<string, List<float>> leaderboard)
    {
        int leaderboardCounter = 0;
        foreach (KeyValuePair<string, List<float>> pair in leaderboard)
        {
            leaderboardCounter++;
            leaderboardNames.text += "\n" + leaderboardCounter + ": " + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pair.Key);

            leaderboardScores.text += "\n" + pair.Value[0] + "pts";

            //leaderboardTimes.text += "\n"/* + pair.Value[1]*/;
            DisplayTime(pair.Value[1], false);
            if (leaderboardCounter >= maxLeaderboardPositions)
            {
                break;
            }
        }
    }
    private void CalculateTime(float totalTime)
    {
        int hours = System.TimeSpan.FromSeconds(totalTime).Hours;
        int minutes = System.TimeSpan.FromSeconds(totalTime).Minutes;
        int seconds = System.TimeSpan.FromSeconds(totalTime).Seconds;
        float milliSeconds = (totalTime - seconds) * 10;
        if (milliSeconds > 9)
            milliSeconds = 0;
    }

    private void DisplayTime(float totalTime, bool isPersonalBestTime)
    {
        int hours = System.TimeSpan.FromSeconds(totalTime).Hours;
        int minutes = System.TimeSpan.FromSeconds(totalTime).Minutes;
        int seconds = System.TimeSpan.FromSeconds(totalTime).Seconds;
        float milliSeconds = (totalTime - seconds) * 10;
        if (milliSeconds > 9)
            milliSeconds = 0;

        if (isPersonalBestTime)
        {
            leaderboardTimes.text += "\n" + "..........";
        }
        if (hours > 0)
        {
            leaderboardTimes.text += "\n" + hours.ToString() + "h " + minutes.ToString() + "mn " + seconds.ToString() + "s";
        }
        else if (minutes > 0)
        {
            leaderboardTimes.text += "\n" + minutes.ToString() + "mn " + seconds.ToString() + "s";
        }
        else
        {
            leaderboardTimes.text += "\n" + seconds.ToString() + "." + milliSeconds.ToString("F0") + "s";
        }
    }

    public void OnColorWheel()
	{
		SceneManager.LoadScene(2);
	}
    public void OnTrainGame()
    {
        SceneManager.LoadScene(3);
    }

	public void LogOut()
	{
		SceneManager.LoadScene(0);
		PlayerPrefs.SetString("username", "");
	}

    private void ShowBadges(Games game)
    {
        aquiredBadges = Repository.GetAquiredBadges(game);

        foreach (Image badge in badges)
        {
            if (aquiredBadges.Contains(badge.name))
            {
                badge.color = Color.white;
            }
        }
    }

    public void SetColorWheelDescLanguage(string chosenLanguage)
    {
        switch (chosenLanguage)
        {
            case "English":
                ColorWheelDesc.text = "Spin the wheel to select a random color. " +
                    "Then choose the correct button that combines with the color to create a well known french expression.  " +
                    "You have three tries to choose the correct button on every spin. For each consecutive answer that is correct you get more points per answer.";
                break;
            case "French":
                ColorWheelDesc.text = "Faites tourner la roue pour sélectionner une couleur au hasard. " +
                    "Choisissez ensuite le bon bouton qui se combine avec la couleur pour créer une expression française bien connue. " +
                    "Vous avez trois essais pour choisir le bon bouton à chaque tour. Pour chaque réponse consécutive correcte, vous obtenez plus de points par réponse.";
                break;
            case "German":
                ColorWheelDesc.text = "Drehen Sie das Rad, um eine zufällige Farbe auszuwählen. " +
                    "Wählen Sie dann die richtige Taste, die in Kombination mit der Farbe einen bekannten französischen Ausdruck ergibt. " +
                    "Sie haben drei Versuche, bei jeder Drehung die richtige Taste zu wählen. Für jede aufeinanderfolgende richtige Antwort erhalten Sie mehr Punkte pro Antwort.";
                break;
        }
    }
    public void SetTrainGameDescLanguage(string chosenLanguage)
    {
        switch (chosenLanguage)
        {
            case "English":
                TrainGameDesc.text = "Look at the expression in the cloud and create a similar sentence on the train (there are 11 in total) while using pronouns instead of nouns complements " +
                    "by dragging the boxes from the trucks to the train. To lock your answer press on the switch in the upper left corner.";
                break;
            case "French":
                TrainGameDesc.text = "Regardez l'expression dans le nuage et créez une phrase équivalente dans le train (il y en a 11 au total) en utilisant uniquement des pronoms au lieu des substantifs compléments. " +
                    "Pour cela faites glisser les charges  des camions vers le train. Afin d'enregistrer votre réponse, appuyez sur le feu dans le coin supérieur gauche.";
                break;
            case "German":
                TrainGameDesc.text = "Schauen Sie sich den Ausdruck in der Wolke an und bilden Sie einen äquivalenten Satz im Zug (es gibt insgesamt 11 Sätze), indem Sie ausschließlich Pronomen anstelle von ergänzenden Substantiven verwenden. " +
                    "Ziehen  Sie  dazu die Frachten von den Lastwagen auf den Zug. Um Ihre Antwort zu speichern, drücken Sie auf den Ampelschalter in der oberen linken Ecke.";
                break;
        }
    }
}
