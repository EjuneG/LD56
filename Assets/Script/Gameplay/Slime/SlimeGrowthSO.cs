using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlimeGrowthSO", menuName = "ScriptableObjects/SlimeGrowthSO", order = 1)]
public class SlimeGrowthSO : ScriptableObject
{
    public List<float> EnergyNeeded;
    public List<float> Damages;
    public List<float> MaxHP;
}
