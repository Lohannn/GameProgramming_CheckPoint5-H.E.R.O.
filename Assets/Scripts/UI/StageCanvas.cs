using UnityEngine;
using UnityEngine.UI;

public class StageCanvas : MonoBehaviour
{
    [SerializeField] private StageData data;
    [SerializeField] private Transform bombSlots;
    [SerializeField] private Text lifeText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highscoreText;
    [SerializeField] private Slider timeSlider;

    private Image[] bombIcons;

    private void Start()
    {
        bombIcons = bombSlots.GetComponentsInChildren<Image>();
    }

    private void Update()
    {
        lifeText.text = PlayerData.life.ToString();
        lifeText.transform.Find("lifeText").GetComponentInChildren<Text>().text = PlayerData.life.ToString();

        for (int i = 0; i < bombIcons.Length; i++)
        {
            if (i >= data.GetBombQuantity())
            {
                bombIcons[i].enabled = false;
            }
        }

        timeSlider.value = (data.GetCurrentTime() / data.GetTimeOfTheStage());

        scoreText.text = $"Score: {PlayerData.score}";
        scoreText.transform.Find("ScoreText").GetComponentInChildren<Text>().text = $"Score: {PlayerData.score}";
        highscoreText.text = $"Highscore: {PlayerData.highscore}";
        highscoreText.transform.Find("HighscoreText").GetComponentInChildren<Text>().text = $"Highscore: {PlayerData.highscore}";
    }
}
