using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] private Text highscoreText;
    [SerializeField] private Text infoText;

    private void Start()
    {
        StartCoroutine(InfoBlink());
    }

    private void Update()
    {
        highscoreText.text = $"Highscore: {PlayerData.highscore}";
        highscoreText.transform.Find("HighscoreText").GetComponentInChildren<Text>().text = $"Highscore: {PlayerData.highscore}";

        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Stage1Scene");
        }
    }

    private IEnumerator InfoBlink()
    {
        infoText.enabled = false;
        yield return new WaitForSeconds(0.5f);
        infoText.enabled = true;
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(InfoBlink());
    }
}
