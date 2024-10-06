using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    public List<Fruit> FruitsInRange { get; private set; }
    public List<Slime> SlimesInRange { get; private set; }
    public List<GameObject> EnergyBallsInRange { get; private set; }
    [SerializeField] float detectionRadius = 3f;
    LayerMask detectionLayer;

    private void Awake()
    {
        FruitsInRange = new List<Fruit>();
        SlimesInRange = new List<Slime>();
        EnergyBallsInRange = new List<GameObject>();
        detectionLayer = LayerMask.GetMask("Default");
    }

    public void ClearLists(){
        if(FruitsInRange == null || SlimesInRange == null || EnergyBallsInRange == null){
            return;
        }
        FruitsInRange.Clear();
        SlimesInRange.Clear();
        EnergyBallsInRange.Clear();
    }
    public void UpdateObjectsInRange()
    {
        ClearLists();
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer);
        foreach (Collider2D obj in objectsInRange)
        {
            if (obj.gameObject.name.Contains("EnergyBall"))
            {
                if (!EnergyBallsInRange.Contains(obj.gameObject))
                {
                    EnergyBallsInRange.Add(obj.gameObject);
                }
            }
            else if (obj.CompareTag("Fruit"))
            {
                Fruit fruit = obj.GetComponent<Fruit>();
                if (!FruitsInRange.Contains(fruit))
                {
                    FruitsInRange.Add(fruit);
                }
            }
            else if (obj.CompareTag("Slime"))
            {
                Slime slime = obj.GetComponent<Slime>();
                if (!SlimesInRange.Contains(slime))
                {
                    SlimesInRange.Add(slime);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fruit"))
        {
            Fruit fruit = other.GetComponent<Fruit>();
            FruitsInRange.Add(fruit);
        }
        else if (other.CompareTag("Slime"))
        {
            Slime slime = other.GetComponent<Slime>();
            SlimesInRange.Add(slime);
        }
        else if (other.name.Contains("EnergyBall"))
        {
            EnergyBallsInRange.Add(other.gameObject);
        }
    }


}
