using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BigCreature : MonoBehaviour
{
    [SerializeField] float movespeed = 1f;
    public Vector3 TargetPosition;
    public AudioSource audioSource;
    public Animator shadeAnim;

    float playSoundTime = 0f;
    float playSoundInterval = 5f;

    float lifeTime = 25f;

    [SerializeField] float killRadius = 2f;

    bool playerDead = false;
    void Start(){
        AudioManager.Instance.StopMusic();
    }
    void FixedUpdate(){
        TargetPosition = GameManager.Instance.Player.transform.position;
        MoveTo(TargetPosition);
        if(playerDead){
            return;
        }
        playSoundTime -= Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if(playSoundTime <= 0f){
            Stomp();
            playSoundTime = playSoundInterval;
        }

        if(lifeTime <= 0f){
            AudioManager.Instance.PlayMusic("BGM");
            Destroy(gameObject);
        }
    }

    void MoveTo(Vector3 target){
        transform.position = Vector3.MoveTowards(transform.position, target, 0.01f * movespeed);
    }

    void Stomp(){
        MakeSound();
        if(playerIsInRange()){
            playerDead = true;
            GameManager.Instance.EndGame(EndCondition.Stomp);
        }else{
            UpdateNextStompTime();
        }
    }
    void MakeSound(){
        audioSource.Play();
    }

    void UpdateNextStompTime(){
        if(distanceToPlayer() > 10f){
            playSoundInterval = 5f;
        }else{
            playSoundInterval = distanceToPlayer() / 3f;
        }

    }

    bool playerIsInRange(){
        if(Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position) <= killRadius){
            return true;
        }else{
            return false;
        }
    }

    float distanceToPlayer(){
        return Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position);
    }


}
