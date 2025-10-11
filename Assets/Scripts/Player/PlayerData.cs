using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static int life;
    public static int highscore;
    public static int score;

    public void ResetData()
    {
        life = 3;
        score = 0;
    }

    public static void AddScore(int points)
    {
        score += points;

        if (score > highscore)
        {
            highscore = score;
        }
    }
}
