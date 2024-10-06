using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class ShadeAnimation : MonoBehaviour
{
    public Animator animator;

    void OnEnable(){
        GameEvent.OnPlayShade += PlayShadeAnim;
    }

    void OnDisable(){
        GameEvent.OnPlayShade -= PlayShadeAnim;
    }

    public void PlayShadeAnim(){
        GetComponent<Image>().enabled = true;
        animator.Play("TurnDark");
    }
}
