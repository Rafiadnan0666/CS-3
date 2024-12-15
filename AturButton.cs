using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AturButton : MonoBehaviour
{

    public void RestartGame()
    {
        Debug.Log("Restart button clicked.");
        //Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Menu");
        SceneManager.LoadScene("Main");
    }

    public void LoadMainMenu()
    {
        Debug.Log("Main Menu button clicked.");
        SceneManager.LoadScene("Menu");
    }

    public void HowTo()
    {
        Debug.Log("How");
        SceneManager.LoadScene("ApaNih");
    }

    public void Single()
    {
        SceneManager.LoadScene("MissSingle");
    }
    public void Online()
    {
        SceneManager.LoadScene("ConectToServer");
    }

}
