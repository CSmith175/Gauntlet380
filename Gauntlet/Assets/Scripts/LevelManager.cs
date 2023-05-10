using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager: MonoBehaviour
{
    public GameObject[] levels;
    private int currentLevel = 0;
    private int currentPlayerCount = 0;
    private int playersExited = 0;

    private void Start()
    {
        currentLevel = 0;
        LoadLevel(currentLevel);
    }

    public void PlayerJoined()
    {
        currentPlayerCount++;
    }
    public void PlayerLeft()
    {
        currentPlayerCount--;
    }

    public void PlayerExited()
    {
        playersExited++;
        CheckForLevelComplete();
    }

    private void CheckForLevelComplete()
    {
        if(playersExited >= currentPlayerCount)
        {
            currentLevel++;
            if(currentLevel >= levels.Length)
            {
                currentLevel = 0;
            }

            LoadLevel(currentLevel);
        }
    }

    private void LoadLevel(int level)
    {
        foreach (GameObject gameObject in levels)
        {
            gameObject.SetActive(false);
        }

        foreach(EnemyParent enemy in GameObject.FindObjectsOfType<EnemyParent>())
        {
            enemy.gameObject.SetActive(false);
        }

        levels[level].SetActive(true);
    }
}
