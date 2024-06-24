using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using UnityEngine.AI;

public class TruckManager : MonoBehaviour
{
    [SerializeField] private GameObject truckPrefab;
    [SerializeField] private float radius = 100f;

    void Start()
    {
        RandomSpawn();
    }

    public void RandomSpawn()
    {
        Vector3 randomPosition;
        NavMeshHit hit;

        do
        {
            randomPosition = RandomNavmeshLocation();
            NavMesh.FindClosestEdge(randomPosition, out hit, NavMesh.AllAreas);
        } while (Vector3.Distance(hit.position, randomPosition) < 1f);

        Instantiate(truckPrefab, randomPosition, Quaternion.identity);
    }
    private Vector3 RandomNavmeshLocation()
    {
        Vector2 randomCirclePos = Random.insideUnitCircle;
        Vector3 randomUnitPos = new Vector3(randomCirclePos.x, 0f, randomCirclePos.y);
        Vector3 randomPosition = transform.position + (radius * randomUnitPos);
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }
}