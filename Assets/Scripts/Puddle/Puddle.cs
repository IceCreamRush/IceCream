using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class puddle : MonoBehaviour
{
    [SerializeField] private GameObject puddlePrefab;
    [SerializeField] private float radius = 100f;
    [SerializeField] private float nbPuddles = 20f;

    void Start()
    {
        for (int i = 0; i < nbPuddles; i++)
        {
            RandomSpawn();
        }
    }

    public void RandomSpawn()
    {
        Vector3 randomPosition;
        NavMeshHit hit;
        bool canSpawn = true;

        do
        {
            randomPosition = RandomNavmeshLocation();
            NavMesh.FindClosestEdge(randomPosition, out hit, NavMesh.AllAreas);
        } while (Vector3.Distance(hit.position, randomPosition) < 1f);

        Collider[] hitColliders = Physics.OverlapSphere(randomPosition, 1f);
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Puddle"))
            {
                canSpawn = false;
                RandomSpawn();
                break;
            }
        }
        
        if (canSpawn)
        {
            float angle = Random.Range(0f, 360f);
            Vector3 rotation = new Vector3(0f, angle, 0f);
            randomPosition.y = 0.2f;
            Instantiate(puddlePrefab, randomPosition, Quaternion.Euler(rotation));
        }
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
