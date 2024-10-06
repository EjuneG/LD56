using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCreatureAttack : MonoBehaviour
{
    public GameObject BigCreaturePrefab;
    float gameTime;

    void Update(){
        //Spawn one every 60 seconds
        gameTime += Time.deltaTime;
        if(gameTime >= 60f){
            gameTime = 0;
            SpawnBigCreature();
        }
    }
    public void SpawnBigCreature(){
        //Spawn big creature away from the player
        int negativeFactor = Random.Range(0, 2) == 0 ? -1 : 1;
        Vector3 spawnPosition = new Vector3(GameManager.Instance.Player.transform.position.x + Random.Range(7f, 10f) * negativeFactor, GameManager.Instance.Player.transform.position.y + Random.Range(7f, 10f) * negativeFactor, GameManager.Instance.Player.transform.position.z);

        GameObject bigCreatureGO = Instantiate(BigCreaturePrefab, spawnPosition, Quaternion.identity);
        BigCreature bigCreature = bigCreatureGO.GetComponent<BigCreature>();
        GameEvent.TriggerOnBigCreatureSpawns(bigCreature);
    } 
}
