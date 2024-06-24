using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TruckTimer : MonoBehaviour
{
    public float hereTimer = 30f;
    public bool haveIce = true;
    [SerializeField] private AudioSource collectSound;
    [SerializeField] private float minDistance = 10f;
    private Transform playerTransform;
    private TextMeshProUGUI timer;

    TruckManager truckManager;

    void Start()
    {
        truckManager = GameObject.Find("TruckManager").GetComponent<TruckManager>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        timer = GameObject.Find("Timer").GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if (haveIce && Vector3.Distance(transform.position, playerTransform.position) <= minDistance)
        {
            hereTimer -= Time.deltaTime;
        }
        if (hereTimer <= 0.0f)
        {
            GameManager.Instance.gameState = GameManager.GameStates.GameOver;
            hereTimer = 0.0f;
        }
        timer.text = hereTimer.ToString("F1");  
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            collectSound.Play();
            truckManager.RandomSpawn();
            Destroy(gameObject);
        }
    }
}
