using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGained : MonoBehaviour
{
    private Text scoreText;
    private Text scoreTextShade;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Transform scoreTextTransform = transform.Find("TextScoreGained");
        scoreText = transform.Find("TextScoreGained") != null ? 
            scoreTextTransform.GetComponent<Text>() : null;

        scoreTextShade = GetComponent<Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(Blink());
        StartCoroutine(DisableTimer());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(50.0f * Time.deltaTime * Vector2.up);
    }

    private IEnumerator Blink()
    {
        scoreText.color = new Color32(255, 254, 125, 255);
        yield return new WaitForSecondsRealtime(0.08f);
        scoreText.color = new Color32(255, 175, 235, 255);
        yield return new WaitForSecondsRealtime(0.08f);
        scoreText.color = Color.white;
        yield return new WaitForSecondsRealtime(0.08f);
        StartCoroutine(Blink());
    }

    private IEnumerator DisableTimer()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        gameObject.SetActive(false);
    }

    public void SetText(int score)
    {
        if (scoreTextShade != null && scoreText != null)
        {
            scoreTextShade.text = score.ToString();
            scoreText.text = score.ToString();
        }
    }
}
