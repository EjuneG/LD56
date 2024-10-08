using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUp : MonoBehaviour
{
    GameObject player => GameManager.Instance.Player;
    [SerializeField]CameraZoom cameraZoom;

    void OnEnable(){
        GameEvent.OnFirstSizeUp += FirstLevelUp;
        GameEvent.OnSecondSizeUp += SecondLevelUp;
    }

    void OnDisable(){
        GameEvent.OnFirstSizeUp -= FirstLevelUp;
        GameEvent.OnSecondSizeUp -= SecondLevelUp;
    }

    public void FirstLevelUp(){
        cameraZoom.ZoomOut(7.5f);
        player.transform.localScale = new Vector3(2f, 2f, 2f);
        player.GetComponent<Slime>().FruitDetectionRange *= 2;
    }

    public void SecondLevelUp(){
        cameraZoom.ZoomOut(15f);
        player.transform.localScale = new Vector3(4f, 4f, 4f);
        player.GetComponent<Slime>().FruitDetectionRange *= 2;
    }
}
