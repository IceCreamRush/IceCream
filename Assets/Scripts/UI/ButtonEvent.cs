using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonEvent : MonoBehaviour
{
    public void GoMainMenu()
    {
        PlayEffect();
        SceneManager.LoadScene(0);
    }
    public void GameStart()
    {
        PlayEffect();
        SceneManager.LoadScene(1);
    }
    public void GameReStart()
    {
        PlayEffect();
        SceneManager.LoadScene(1);
    }

    public void GameExit()
    {
        PlayEffect();
        Application.Quit();
    }
    public void ContinueGame()
    {
        PlayEffect();
        GameManager.Instance.gameState = GameManager.GameStates.Playing;
    }
    public void PauseGame()
    {
        PlayEffect();
        GameManager.Instance.gameState = GameManager.GameStates.Paused;
    }

    private void PlayEffect()
    {

       AudioManager.Instance.PlayEffect("button");
    }
}
