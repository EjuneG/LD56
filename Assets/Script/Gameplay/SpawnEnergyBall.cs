using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnergyBall : MonoBehaviour
{
    public int EnergyBall = 5;
    public GameObject EnergyBallPrefab;
    public float MinDistance = 0.5f;
    public float SpawnRadius = 1f;
    private List<Vector3> spawnedPositions = new List<Vector3>();

    // public void Spawn()
    // {
    //     // Spawn energy ball near the location of the object
    //     for (int i = 0; i < EnergyBall; i++)
    //     {
    //         Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
    //         GameObject energyBall = Instantiate(EnergyBallPrefab, spawnPosition, Quaternion.identity);
    //     }
    // }

    public void Spawn()
    {
        spawnedPositions.Clear();

        for (int i = 0; i < EnergyBall; i++)
        {
            Vector3 spawnPosition = GenerateSpawnPosition();
            //if spawnPosition is out of border, don't spawn
            float xBorder = 25f;
            float yBorder = 25f;
            if (spawnPosition.x > xBorder || spawnPosition.x < -xBorder || spawnPosition.y > yBorder || spawnPosition.y < -yBorder)
            {
                continue;
            }
            if (spawnPosition != Vector3.zero)
            {
                GameObject energyBall = Instantiate(EnergyBallPrefab, spawnPosition, Quaternion.identity);
                spawnedPositions.Add(spawnPosition);
            }
            else
            {
                Debug.LogWarning("Could not find a suitable position to spawn energy ball.");
            }
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        int attempts = 0;
        while (attempts < 100) // Limit attempts to avoid infinite loop
        {
            Vector3 candidatePosition = transform.position + Random.insideUnitSphere * SpawnRadius;
            candidatePosition.z = transform.position.z; // Ensure it stays in the 2D plane

            if (IsPositionValid(candidatePosition))
            {
                return candidatePosition;
            }

            attempts++;
        }

        return Vector3.zero; // Return zero vector if no suitable position found
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < MinDistance)
            {
                return false;
            }
        }
        return true;
    }
}
