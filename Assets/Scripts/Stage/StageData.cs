using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StageData : MonoBehaviour
{
    [Header("Stage Settings")]
    [SerializeField] private float timeOfTheStage;
    private float currentTime;
    [SerializeField] private float secondsPassing;
    private int bombQuantity = 6;

    private void Start()
    {
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
        currentTime -= seconds;

        if (currentTime > 0)
        {
            StartCoroutine(StageTimer(seconds));
        }
    }
}
