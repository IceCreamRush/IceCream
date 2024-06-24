using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;
    public float spawnIntervalMin = 1.5f;
    public float spawnIntervalMax = 4f;
    public float spawnDistance = 5f;
    public float birdLifetime = 10f;
    public float birdHeight = 5f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnBirds());
    }

    IEnumerator SpawnBirds()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));
            SpawnBird();
        }
    }

    void SpawnBird()
    {
        Vector3 spawnPosition = GetRandomPositionOutsideCameraView();
        Vector3 targetPosition = GetRandomPositionInsideCameraView();

        GameObject bird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);

        BirdMovement birdMovement = bird.AddComponent<BirdMovement>();
        birdMovement.SetTargetPosition(targetPosition);

        Destroy(bird, birdLifetime);
    }

    Vector3 GetRandomPositionOutsideCameraView()
    {
        float spawnX = 0f;
        float spawnY = birdHeight;
        float spawnZ = mainCamera.transform.position.z + Random.Range(15f, 20f);

        int side = Random.Range(0, 2);
        switch (side)
        {
            case 0:
                spawnX = mainCamera.ViewportToWorldPoint(new Vector3(-0.1f, 0.5f, spawnDistance)).x;
                break;
            case 1:
                spawnX = mainCamera.ViewportToWorldPoint(new Vector3(1.1f, 0.5f, spawnDistance)).x;
                break;
        }
        return new Vector3(spawnX, spawnY, spawnZ);
    }

    Vector3 GetRandomPositionInsideCameraView()
    {
        float targetX = mainCamera.ViewportToWorldPoint(new Vector3(Random.Range(0.2f, 0.8f), 0.5f, spawnDistance)).x;
        float targetY = birdHeight;
        float targetZ = mainCamera.transform.position.z;

        return new Vector3(targetX, targetY, targetZ);
    }
}
