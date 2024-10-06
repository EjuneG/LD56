using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameObject Player;
    public BigCreatureAttack BigCreatureAttack;
    public EndCondition EndCondition;
    public PauseMenu PauseMenu;

    new void Awake()
    {
        base.Awake();
        transform.SetParent(null);
        DontDestroyOnLoad(this.gameObject);
    }

    public void EndGame(EndCondition endCondition)
    {
        switch (endCondition)
        {
            case EndCondition.Win:
                SceneManager.LoadScene(2);
                break;
            case EndCondition.Stomp:
                SceneManager.LoadScene(3);
                break;
            case EndCondition.Dead:
                SceneManager.LoadScene(4);
                break;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CallPauseMenu(){
        if(PauseMenu.gameObject.activeSelf){
            PauseMenu.ResumeGame();
        }else{
            PauseMenu.PauseGame();
        }
    }

    //do not destroy on scene load

}

public enum EndCondition
{
    Win,
    Stomp,
    Dead
}
