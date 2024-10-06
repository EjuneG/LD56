using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void PauseGame(){
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    public void ResumeGame(){
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void RestartGame(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void QuitGame(){
        Application.Quit();
    }
}
