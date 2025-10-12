using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Lamp : MonoBehaviour
{
    [SerializeField] private GameObject darkness;
    [SerializeField] private GameObject extraDarkness;
    [SerializeField] private GameObject lamp;
    [SerializeField] private EnemyBehaviour[] enemies;

    private Tilemap darknessMap;
    private Tilemap extraDarknessMap;

    private List<EnemyBehaviour> snakes = new List<EnemyBehaviour>();

    private void Start()
    {
        darknessMap = darkness.GetComponent<Tilemap>();

        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<SpriteRenderer>().sortingOrder == -1)
            {
                snakes.Add(enemy);
            }
        }

        if (extraDarkness != null)
        {
            extraDarknessMap = extraDarkness.GetComponent<Tilemap>();
        }
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Bomb") != null ||
            GameObject.FindGameObjectWithTag("Explosion") != null)
        {
            darknessMap.color = new Color32((byte)darknessMap.color.r, (byte)darknessMap.color.g, (byte)darknessMap.color.b, 235);

            if (extraDarknessMap != null)
            {
                extraDarknessMap.color = new Color32((byte)darknessMap.color.r, (byte)darknessMap.color.g, (byte)darknessMap.color.b, 0);
            }

            if (snakes.Count > 0)
            {
                foreach (var snake in snakes)
                {
                    snake.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    snake.OnDarknessOut();
                }
            }
        }
        else
        {
            darknessMap.color = Color.white;

            if (extraDarknessMap != null)
            {
                extraDarknessMap.color = Color.white;
            }

            if (snakes.Count > 0)
            {
                foreach (var snake in snakes)
                {
                    snake.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    snake.OnDarkness();
                }
            }
        }
    }

    public void LightsOut()
    {
        darkness.SetActive(true);

        if (extraDarkness != null)
        {
            extraDarkness.SetActive(true);
        }

        lamp.GetComponent<Tilemap>().color = new Color32(100, 100, 100, 255);

        if (enemies.Length > 0)
        {
            foreach (var enemy in enemies)
            {
                enemy.OnDarkness();

                if (snakes.Count > 0)
                {
                    if (snakes.Contains(enemy))
                    {
                        enemy.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    }
                }
            }
        }
    }
}
