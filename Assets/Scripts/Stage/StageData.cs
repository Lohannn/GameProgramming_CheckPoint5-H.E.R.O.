using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageData : MonoBehaviour
{
    [Header("Stage Settings")]
    [SerializeField] private Player player;
    [SerializeField] private float timeOfTheStage;
    private float currentTime;
    [SerializeField] private float secondsPassing;
    [SerializeField] private int scoreValueForEachSecond;
    private int bombQuantity = 6;
    [SerializeField] private int scoreValueForEachBomb;

    private bool stageCleared = false;

    private StageAudioPlayer ap;

    private void Start()
    {
        ap = GetComponent<StageAudioPlayer>();

        currentTime = timeOfTheStage;

        StartCoroutine(StageTimer(secondsPassing));
    }

    public int GetBombQuantity()
    {
        return bombQuantity;
    }

    public void RemoveBomb()
    {
        bombQuantity -= 1;
    }

    public float GetTimeOfTheStage() { 
        return timeOfTheStage;
    }

    public float GetCurrentTime() { 
        return currentTime;
    }

    private IEnumerator StageTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (!stageCleared)
        {
            currentTime -= seconds;

            if (currentTime > 0)
            {
                StartCoroutine(StageTimer(seconds));
            }
            else
            {
                player.OnDeathByTime();
            }
        }
    }

    public IEnumerator ScoreCollector()
    {
        stageCleared = true;

        while (currentTime > 0)
        {
            currentTime--;
            PlayerData.AddScore(scoreValueForEachSecond);
            ap.PlayAudio(ap.ADDSCORE);
            yield return new WaitForSecondsRealtime(0.05f);
        }

        while (bombQuantity > 0)
        {
            bombQuantity--;
            PlayerData.AddScore(scoreValueForEachBomb);
            ap.PlayAudio(ap.ADDSCORE);
            yield return new WaitForSecondsRealtime(0.3f);
        }

        ap.PlayAudio(ap.STAGEEND);

        yield return new WaitForSecondsRealtime(2.0f);

        print("Passou de fase");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(PlayerData.GetNextScene());
    }
}
