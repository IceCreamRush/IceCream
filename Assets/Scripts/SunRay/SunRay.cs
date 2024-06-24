using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;


public class SunRay : MonoBehaviour
{
    [SerializeField] private float baseSpeed;
    private float speed;
    // private float direction;
    [SerializeField] private float m_baseRadius;
    private float m_radius;

    private float timer = 0.0f;

    // For random point within map bounds
    [SerializeField] private Vector2 m_mapCenter;
    [SerializeField] private float m_mapRadius;
    
    // For movement
    private Vector3 m_startPoint;
    private Vector3 m_waypoint;    // The point where the SunRay will be heading
    private Vector3 m_influencePoint;    // To influence the SunRay's movement to make it circular and smooth
    private float m_timeToWaypoint;
    private float m_lerpTimer = 0.0f;

    
    private void Start()
    {
        m_startPoint = transform.position;
        m_waypoint = SetNewWaypoint();
        m_influencePoint = SetNewWaypoint();
        m_timeToWaypoint = 5.0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Check if arrived at waypoint
        if ((m_waypoint - transform.position).magnitude <= 0.5f)
        {
            m_startPoint = transform.position;
            m_waypoint = SetNewWaypoint();
            m_influencePoint = SetNewWaypoint();
            m_lerpTimer = 0.0f;
        }
        
        Move();
        
        // SetRadius();
    }

    private void Move()
    {

        m_lerpTimer += Time.deltaTime;
        float t = m_lerpTimer / m_timeToWaypoint;
        
        // Lerp position
        Vector3 firstLerp = Vector3.Lerp(m_startPoint, m_influencePoint, t);
        Vector3 secondLerp = Vector3.Lerp(m_influencePoint, m_waypoint, t);
        Vector3 thirdLerp = Vector3.Lerp(firstLerp, secondLerp, t);
        
        // Smoothstep speed
        float smoothStepT = Mathf.SmoothStep(0, 1, t);
        speed = baseSpeed * smoothStepT;
        
        transform.position = Vector3.Lerp(transform.position, new Vector3(thirdLerp.x, transform.position.y, thirdLerp.z), speed * Time.deltaTime);
    }

    private Vector3 SetNewWaypoint()
    {
        Vector2 direction = Random.insideUnitCircle.normalized;
        float length = Random.Range(0.0f, m_mapRadius);
        return new Vector3(m_mapCenter.x + direction.x * length, transform.position.y, m_mapCenter.y + direction.y * length);
    }

    private void SetRadius()
    {
        m_radius = m_baseRadius + Mathf.Sin(timer);
        transform.localScale = new Vector3(m_radius, 1.0f, m_radius);
    }
    
    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(m_waypoint, 1.0f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(m_influencePoint, 1.0f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(m_mapCenter.x, transform.position.y, m_mapCenter.y), m_mapRadius);
    }
}
