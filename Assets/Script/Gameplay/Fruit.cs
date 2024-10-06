using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour, ITarget
{
    public float MaxHealth = 100;
    public float Health = 100;
    [SerializeField] private SpawnEnergyBall ballSpawner;

    public void TakeDamage(float damage, Slime attacker)
    {
        Health -= damage;
        if (Health <= 0)
        {
            // Spawn energy ball near the location of the object
            ballSpawner.Spawn();
            attacker.currentTarget = null;
            attacker.EndEat();
            attacker.CurrentHP += MaxHealth / 2;
            if(attacker.CurrentHP > attacker.MaxHP){
                attacker.CurrentHP = attacker.MaxHP;
            }
            Destroy(gameObject);
        }
    }
}

public interface ITarget{
    void TakeDamage(float damage, Slime attacker);
}
