using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float smooth;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 rotation;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        this.transform.position = player.position + offset;
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 target = player.position + offset;
        Vector3 dir = Vector3.SmoothDamp(this.transform.position, target, ref velocity, smooth);

        characterController.Move(dir - this.transform.position);
        this.transform.rotation = Quaternion.Euler(rotation);
    }
}
