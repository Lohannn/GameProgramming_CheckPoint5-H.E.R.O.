using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Image pausePanel;

    private StageMusicPlayer smp;

    private void Start()
    {
        smp = GameObject.FindGameObjectWithTag("StageMusicPlayer").GetComponent<StageMusicPlayer>();
    }

    private void Update()
    {
        if (player.isDead || player.onDeathScene || player.hasWon || player.isReadyToResume) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.gameObject.SetActive(!pausePanel.gameObject.activeSelf);
            Time.timeScale = Time.timeScale == 0.0f ? 1.0f : 0.0f;
        }
    }

    public void Resume()
    {
        pausePanel.gameObject.SetActive(false);
    }

    public void BackToMainMenu()
    {
        PlayerData.ResetData();
        PlayerData.ResetBonusLifeCounter();
        Time.timeScale = 1.0f;
        smp.PlayAudio(smp.MAINMENU);
        SceneManager.LoadScene("MainMenuScene");
    }
}
