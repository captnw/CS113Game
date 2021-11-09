using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private void AudioManagerDo(int thingToDo)
    {
        if (AudioManager.instance != null)
        {
            switch (thingToDo)
            {
                case 0:
                    AudioManager.instance.StopAll(); // stop playing all sounds
                    break;
                case 1:
                    AudioManager.instance.PauseAll();
                    break;
                default:
                    // aka case 2
                    AudioManager.instance.UnpauseAll();
                    break;
            }
        }
    }

    public void PlayGame()
    {
        AudioManagerDo(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Settings()
    {
        AudioManagerDo(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void QuitGame()
    {
        //Debug.Log("Quit");
        AudioManagerDo(0);
        Application.Quit();
    }

    public void Back2()
    {
        AudioManagerDo(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void GameToSettings()
    {
        AudioManagerDo(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PauseToSettings()
    {
        AudioManagerDo(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void Unpause()
    {
        AudioManagerDo(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }

    public void Pause()
    {
        AudioManagerDo(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }
}
