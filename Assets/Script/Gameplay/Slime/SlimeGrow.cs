using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGrow : MonoBehaviour
{
    public float TotalEnergy = 0;
    public void GainEnergy(float energy)
    {
        TotalEnergy += energy;
    }
}
