using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    public static int life = 3;
    public static int highscore;
    public static int score;

    private static int bonusLifeScoreCount = 0;
    public static event Action OnBonusLifeGained;

    public static void ResetData()
    {
        life = 3;
        score = 0;
    }

    public static void AddScore(int points)
    {
        score += points;
        bonusLifeScoreCount += points;

        if (bonusLifeScoreCount >= 20000)
        {
            life += 1;
            bonusLifeScoreCount = 0;

            OnBonusLifeGained?.Invoke();
        }

        if (score > highscore)
        {
            highscore = score;
        }
    }

    public static string GetNextScene()
    {
        string actualScene = SceneManager.GetActiveScene().name;

        switch (actualScene)
        {
            case "Stage1Scene":
                return "Stage2Scene";

            case "Stage2Scene":
                return "Stage3Scene";

            case "Stage3Scene":
                return "Stage4Scene";

            case "Stage4Scene":
                return "Stage5Scene";

            case "Stage5Scene":
                return "Stage1Scene";

            default:
                return actualScene;
        }
    }
}
