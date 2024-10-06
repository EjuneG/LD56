using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject OptionMenu;
    public void StartGame(){
        //load next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame(){
        //quit game
        Application.Quit();
    }

    public void OpenOptionMenu(){
        OptionMenu.SetActive(true);
    }
}
