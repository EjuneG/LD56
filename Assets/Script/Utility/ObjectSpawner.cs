using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject FruitPrefab;
    public GameObject SlimePrefab;
    public GameObject MidSlimePrefab;
    public GameObject BigSlimePrefab;
    public GameObject BigFruitPrefab;
    public GameObject GiantFruitPrefab;
    public Transform parentTransform;
    public int fruitCount = 13;
    public int bigFruitCount = 5;
    public int giantFruitCount = 2;
    public int slimeCount = 10;
    public int midSlimeCount = 5;
    public int bigSlimeCount = 3;
    public float minDistance = 1f;  // Unity units
    public Vector2 spawnArea = new Vector2(10f, 10f);  // Width and height of spawn area

    private List<Vector2> spawnedPositions = new List<Vector2>();
    [SerializeField] private GameObject fruitParent;
    [SerializeField] private GameObject slimeParent;

    void Start()
    {
        SpawnObjects();
    }

    void Update(){
        //if there's less than 5 fruits, spawn more
        if(fruitParent.transform.childCount < 5){
            RespawnFruits();
        }

        //if there's less than 5 slimes, spawn more
        if(slimeParent.transform.childCount < 10){
            RespawnSlimes();
        }
    }

    public void SpawnObjects()
    {
        // Spawn fruits
        for (int i = 0; i < fruitCount; i++)
        {
            SpawnFruit(FruitPrefab);
        }

        // Spawn slimes
        for (int i = 0; i < slimeCount; i++)
        {
            SpawnSlime(SlimePrefab);
        }

        // Spawn mid slimes
        for (int i = 0; i < midSlimeCount; i++)
        {
            SpawnSlime(MidSlimePrefab);
        }

        // Spawn big slimes
        for (int i = 0; i < bigSlimeCount; i++)
        {
            SpawnSlime(BigSlimePrefab);
        }

        // Spawn big fruits
        for (int i = 0; i < bigSlimeCount; i++)
        {
            SpawnFruit(BigFruitPrefab);
        }

        // Spawn giant fruits
        for (int i = 0; i < bigSlimeCount; i++)
        {
            SpawnFruit(GiantFruitPrefab);
        }
    }

    public void SpawnFruits(){
        for (int i = 0; i < fruitCount; i++)
        {
            SpawnFruit(FruitPrefab);
        }

        for (int i = 0; i < bigFruitCount; i++)
        {
            SpawnFruit(BigFruitPrefab);
        }

        for (int i = 0; i < giantFruitCount; i++)
        {
            SpawnFruit(GiantFruitPrefab);
        }
    }

    public void SpawnSlimes(){
        for (int i = 0; i < slimeCount; i++)
        {
            SpawnSlime(SlimePrefab);
        }

        for (int i = 0; i < midSlimeCount; i++)
        {
            SpawnSlime(MidSlimePrefab);
        }

        for (int i = 0; i < bigSlimeCount; i++)
        {
            SpawnSlime(BigSlimePrefab);
        }
    }

    private void SpawnObject(GameObject prefab)
    {
        Vector2 position = GeneratePosition();
        if (position != Vector2.zero)
        {
            GameObject obj = Instantiate(prefab, position, Quaternion.identity);
            obj.transform.SetParent(transform);
            spawnedPositions.Add(position);
        }
        else
        {
            Debug.LogWarning("Could not find a suitable position to spawn object.");
        }
    }

    private void SpawnSlime(GameObject prefab)
    {
        Vector2 position = GeneratePosition();
        if (position != Vector2.zero)
        {
            GameObject obj = Instantiate(prefab, position, Quaternion.identity);
            obj.transform.SetParent(slimeParent.transform);
            spawnedPositions.Add(position);
        }
        else
        {
            Debug.LogWarning("Could not find a suitable position to spawn object.");
        }
    }

    private void SpawnFruit(GameObject prefab)
    {
        Vector2 position = GeneratePosition();
        if (position != Vector2.zero)
        {
            GameObject obj = Instantiate(prefab, position, Quaternion.identity);
            obj.transform.SetParent(fruitParent.transform);
            spawnedPositions.Add(position);
        }
        else
        {
            Debug.LogWarning("Could not find a suitable position to spawn object.");
        }
    }

    private Vector2 GeneratePosition()
    {
        int attempts = 0;
        while (attempts < 100)  // Limit attempts to avoid infinite loop
        {
            Vector2 position = new Vector2(
                Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
                Random.Range(-spawnArea.y / 2, spawnArea.y / 2)
            );

            if (IsPositionValid(position))
            {
                return position;
            }

            attempts++;
        }

        return Vector2.zero;  // Return zero vector if no suitable position found
    }

    private bool IsPositionValid(Vector2 position)
    {
        foreach (Vector2 spawnedPosition in spawnedPositions)
        {
            if (Vector2.Distance(position, spawnedPosition) < minDistance)
            {
                return false;
            }
        }
        return true;
    }

    private void ClearExistingObjects()
    {
        spawnedPositions.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Optional: Add a method to respawn objects (e.g., for level reset)
    public void RespawnObjects()
    {
        SpawnObjects();
    }

    public void RespawnFruits(){
        SpawnFruits();
    }

    public void RespawnSlimes(){
        SpawnSlimes();
    }

}
