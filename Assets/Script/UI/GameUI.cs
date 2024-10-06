using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField]GameObject LosePanel;

    void OnEnable(){
        GameEvent.OnBigCreatureEatPlayer += ShowLosePanel;
        LosePanel.SetActive(false);
    }

    void OnDisable(){
        GameEvent.OnBigCreatureEatPlayer -= ShowLosePanel;
    }

    public void ShowLosePanel(){
        LosePanel.SetActive(true);
    }
}
