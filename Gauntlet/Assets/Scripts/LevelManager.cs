using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager: MonoBehaviour
{
    public GameObject[] levels;
    public GameObject gameOverCanvas, startCanvas;
    private int currentLevel = 0;
    private int currentPlayerCount = 0;
    private int playersExited = 0;

    private void OnEnable()
    {
        EventBus._allPlayersDeadEvent += DisplayGameOver;
        EventBus._allPlayersClearEvent += LoadNextLevel;
    }

    private void OnDisable()
    {
        EventBus._allPlayersDeadEvent -= DisplayGameOver;
        EventBus._allPlayersClearEvent -= LoadNextLevel;
    }

    private void Start()
    {
        currentLevel = 0;
        gameOverCanvas.SetActive(false);
        startCanvas.SetActive(true);
        LoadLevel(currentLevel);
    }
    private void LoadNextLevel()
    {
        currentLevel++;
        if(currentLevel >= levels.Length)
        {
            currentLevel = 0;
        }

        LoadLevel(currentLevel);
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

    public void StartGame()
    {
        startCanvas.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void DisplayGameOver()
    {
        gameOverCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
