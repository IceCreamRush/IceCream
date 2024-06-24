using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Car : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints = new List<Transform>();
    [SerializeField] float minSpeed = 1f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] private float rotateSmoothTime = 0.1f;
    private int currentWaypoint;
    private Vector3 moveVector;
    private float rotateSmoothVelocity;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = 0;
        moveVector = Vector3.zero;
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        FollowWaypoint();
        RotateCar();
    }

    private void FollowWaypoint()
    {
        if (Vector3.Distance(this.transform.position, waypoints[currentWaypoint].position) <= 0.2f)
        {
            if (currentWaypoint == waypoints.Count - 1)
                currentWaypoint = 0;
            else
                currentWaypoint++;
        }
        moveVector = waypoints[currentWaypoint].position - this.transform.position;
        moveVector.Normalize();
        moveVector *= speed * Time.deltaTime;
        this.transform.position += moveVector;
    }

    private void RotateCar()
    {
        float targetAngle = Mathf.Atan2(moveVector.x, moveVector.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotateSmoothVelocity, rotateSmoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
}
