using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : MonoBehaviour
{
    public void OnRestartButtonClicked()
    {
        GameManager.Instance.RestartGame();
    }

    public void OnQuitButtonClicked()
    {
        GameManager.Instance.QuitGame();
    }
}
