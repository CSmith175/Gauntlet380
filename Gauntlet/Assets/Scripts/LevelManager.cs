using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager: MonoBehaviour
{
    public GameObject[] levels;
    public GameObject gameOverCanvas, startCanvas;

    public static Transform playerSpawn;
    private int currentLevel = 0;
    private PlayerManager playerManager;

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
        TryGetComponent<PlayerManager>(out playerManager);
        currentLevel = 0;
        gameOverCanvas.SetActive(false);
        startCanvas.SetActive(true);
        LoadLevel(currentLevel);
    }
    private void LoadNextLevel()
    {
        Debug.Log("Loading Next Level");
        levels[currentLevel].SetActive(false);
        currentLevel++;
        if(currentLevel >= levels.Length)
        {
            currentLevel = 0;
        }
        if(playerManager != null)
            playerManager.SetActiveAllPlayers(true);


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

        Debug.Log("Loading level: " + currentLevel);
        levels[level].SetActive(true);
    }

    private void MovePlayersToSpawn()
    {
        foreach (Player player in GameObject.FindObjectsOfType<Player>())
        {
            player.gameObject.transform.position = playerSpawn.position;
        }
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
