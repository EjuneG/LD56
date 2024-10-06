using System;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public delegate void BigCreatureSpawnHandler(BigCreature bigCreature);
    public static event BigCreatureSpawnHandler OnBigCreatureSpawns;
    public static event Action OnBigCreatureEatPlayer;
    public static event Action OnPlayerDeath;
    public static event Action OnLevelUp;
    public static event Action OnEnergyPickup;
    public static event Action OnFirstSizeUp;
    public static event Action OnSecondSizeUp;
    public static event Action OnPlayShade;
    public static event Action OnWin;
    

    public static void TriggerOnBigCreatureSpawns(BigCreature bigCreature) => OnBigCreatureSpawns?.Invoke(bigCreature);
    public static void TriggerOnBigCreatureEatPlayer() => OnBigCreatureEatPlayer?.Invoke();
    public static void TriggerOnLevelUp() => OnLevelUp?.Invoke();
    public static void TriggerOnEnergyPickup() => OnEnergyPickup?.Invoke();
    public static void TriggerOnFirstSizeUp() => OnFirstSizeUp?.Invoke();
    public static void TriggerOnSecondSizeUp() => OnSecondSizeUp?.Invoke();
    public static void TriggerOnPlayerDeath() => OnPlayerDeath?.Invoke();
    public static void TriggerOnPlayShade() => OnPlayShade?.Invoke();
    public static void TriggerOnWin() => OnWin?.Invoke();
}
