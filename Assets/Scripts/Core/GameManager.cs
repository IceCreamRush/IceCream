using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Flags]
    public enum GameStates
    {
        Playing,
        Paused,
        GameOver,
        GameWin,
        None
    }

    public GameStates gameState;

    public TextMeshProUGUI timeText;
    private float delayTime = 1;
    public float timer = 0;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject settingButton;
    private void Awake()
    {
        Instance = this;

        GameInit();
    }

    private void GameInit()
    {
        Debug.Log("Game init!");
        AudioManager.Instance.Init();
        gameState = GameStates.Playing;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameStates.Playing:
                Gaming();
                break;
            case GameStates.Paused:
                PauseGame();
                break;
            case GameStates.GameOver:
                GameOver();
                break;
            case GameStates.GameWin:
                GameWin();
                break;
            case GameStates.None:
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            gameState = GameStates.Paused;
        }
    }

    // Timer 
    public void SetTimer()
    {
        if (Time.time >= delayTime)
        {
            timer += 1;
            delayTime += 1;
        }
    }

    public void ResetTimer()
    {
        timer = 0;
    }


    // Can be used for any action with 2 parameters
    public void SetTimeout<T1, T2>(Action<T1, T2> action, T1 param1, T2 param2,float milliseconds)
    {
        StartCoroutine(SetTimeoutCoroutine(action, param1, param2, milliseconds * 0.001f)); // Set milliseconds to seconds
    }

    private IEnumerator SetTimeoutCoroutine<T1, T2>(Action<T1, T2> action, T1 param1, T2 param2, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke(param1, param2);
    }

    // Can be used for any action with 1 parameter
    public void SetTimeout<T>(Action<T> action, T param, float milliseconds)
    {
        StartCoroutine(SetTimeoutCoroutine(action, param, milliseconds * 0.001f)); // Set milliseconds to seconds
    }

    private IEnumerator SetTimeoutCoroutine<T>(Action<T> action, T param, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke(param);
    }
    // Default SetTimeout for action without parameters
    public void SetTimeout(Action action, float milliseconds)
    {
        StartCoroutine(SetTimeoutCoroutine(action, milliseconds * 0.001f)); // Set milliseconds to seconds
    }

    private IEnumerator SetTimeoutCoroutine(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }

    //Set game state to playing
    public void Gaming()
    {
        Time.timeScale = 1;
        SetTimer();
        if (timeText)
            timeText.text = "Time: " + timer;
        if (pauseMenu)
            pauseMenu.SetActive(false);
        if (winMenu)
            winMenu.SetActive(false);
        if (loseMenu)
            loseMenu.SetActive(false);

        if(settingButton)
            settingButton.SetActive(true);
    }

    //Set game state to paused
    public void PauseGame()
    {
        Time.timeScale = 0;
        //AudioManager.Instance.BGMPause();
        if (pauseMenu)
        {
            pauseMenu.SetActive(true);
            settingButton.SetActive(false); 
        }
    }

    //Set game state to GameOver
    public void GameOver()
    {
        Time.timeScale = 0;
        AudioManager.Instance.BGMStop();
        if (loseMenu)
        {
            loseMenu.SetActive(true);
            settingButton.SetActive(false);
        }
    }
    //Set game state to GameWin
    public void GameWin()
    {
        Time.timeScale = 0;
        AudioManager.Instance.BGMStop();
        if (winMenu)
        {
            winMenu.SetActive(true);
            settingButton.SetActive(false);
        }
    }

    //Restart the game
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }

    //Back to main menu
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}