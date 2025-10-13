using Unity.Mathematics;
using UnityEngine;

public class VfxPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject playerAttack;
    [SerializeField] private GameObject bomb;
    [SerializeField] private GameObject bombExplosion;
    [SerializeField] private GameObject enemyExplosion;
    [SerializeField] private GameObject scoreText;
    private readonly GameObject[] playerAttackPool = new GameObject[10];
    private readonly GameObject[] bombPool = new GameObject[10];
    private readonly GameObject[] bombExplosionPool = new GameObject[10];
    private readonly GameObject[] enemyExplosionPool = new GameObject[10];
    private readonly GameObject[] scoreTextPool = new GameObject[10];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstantiatePlayerAttacks();
        InstantiateBombs();
        InstantiateBombExplosions();
        InstantiateEnemyExplosions();
        InstantiateScoreTexts();
    }

    public GameObject GetPlayerAttack(int damage, Vector2 position, quaternion rotation)
    {
        foreach (var attack in playerAttackPool)
        {
            if (!attack.activeInHierarchy)
            {
                attack.GetComponent<Attack>().SetDamage(damage);
                attack.transform.SetPositionAndRotation(position, rotation);
                attack.SetActive(true);
                return attack;
            }
        }

        return Instantiate(playerAttack, position, rotation);
    }

    private void InstantiatePlayerAttacks()
    {
        for (int i = 0; i < playerAttackPool.Length; i++)
        {
            GameObject attack = Instantiate(playerAttack);
            attack.SetActive(false);
            playerAttackPool[i] = attack;
        }
    }

    public GameObject GetBomb(Vector2 position, quaternion rotation)
    {
        foreach (var bomb in bombPool)
        {
            if (!bomb.activeInHierarchy)
            {
                bomb.transform.SetPositionAndRotation(position, rotation);
                bomb.SetActive(true);
                return bomb;
            }
        }

        return Instantiate(bomb, position, rotation);
    }

    private void InstantiateBombs()
    {
        for (int i = 0; i < bombPool.Length; i++)
        {
            GameObject bombObject = Instantiate(bomb);
            bombObject.SetActive(false);
            bombPool[i] = bombObject;
        }
    }

    public GameObject GetBombExplosion(Vector2 position, quaternion rotation)
    {
        foreach (var explosion in bombExplosionPool)
        {
            if (!explosion.activeInHierarchy)
            {
                explosion.transform.SetPositionAndRotation(position, rotation);
                explosion.SetActive(true);
                return explosion;
            }
        }

        return Instantiate(bombExplosion, position, rotation);
    }

    private void InstantiateBombExplosions()
    {
        for (int i = 0; i < bombExplosionPool.Length; i++)
        {
            GameObject explosion = Instantiate(bombExplosion);
            explosion.SetActive(false);
            bombExplosionPool[i] = explosion;
        }
    }

    public GameObject GetEnemyExplosion(Vector2 position, quaternion rotation)
    {
        foreach (var explosion in enemyExplosionPool)
        {
            if (!explosion.activeInHierarchy)
            {
                explosion.transform.SetPositionAndRotation(position, rotation);
                explosion.SetActive(true);
                return explosion;
            }
        }

        return Instantiate(enemyExplosion, position, rotation);
    }

    private void InstantiateEnemyExplosions()
    {
        for (int i = 0; i < enemyExplosionPool.Length; i++)
        {
            GameObject explosion = Instantiate(enemyExplosion);
            explosion.SetActive(false);
            enemyExplosionPool[i] = explosion;
        }
    }

    public GameObject GetScoreText(int scoreValue, Vector2 position, quaternion rotation)
    {
        foreach (var score in scoreTextPool)
        {
            if (!score.activeInHierarchy)
            {
                score.GetComponent<ScoreGained>().SetText(scoreValue);
                score.transform.SetPositionAndRotation(position, rotation);
                score.SetActive(true);
                return score;
            }
        }

        return Instantiate(scoreText, position, rotation);
    }

    private void InstantiateScoreTexts()
    {
        for (int i = 0; i < scoreTextPool.Length; i++)
        {
            GameObject score = Instantiate(scoreText);
            score.transform.SetParent(GameObject.FindGameObjectWithTag("ScoreGainCanvas").transform, true);
            score.SetActive(false);
            scoreTextPool[i] = score;
        }
    }
}