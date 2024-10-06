using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    public float energy;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Slime slime = other.GetComponent<Slime>();
            slime.Energy += energy;
            slime.CheckGrow();
            GameEvent.TriggerOnEnergyPickup();
            //destroy the energy ball
            Destroy(gameObject);
        }else if(other.CompareTag("Slime")){
            Slime slime = other.GetComponent<Slime>();
            if(slime == null){
                Debug.Log($"Slime is null, {other.gameObject.name}");
                return;
            }
            slime.Energy += energy;
            slime.CheckGrow();
            //destroy the energy ball
            Destroy(gameObject);
        }
    }
}
