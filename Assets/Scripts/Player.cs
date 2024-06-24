using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    private enum CharacterState
    {
        Idle,
        Running
    }

    private CharacterState m_characterState;
    private CharacterState m_oldCharacterState;

    [SerializeField] private AudioSource[] m_audioSources;
    [SerializeField] private AudioSource footStep;
    [SerializeField] private AudioSource radiation;
    [SerializeField] private AudioSource burn;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float rotateSmoothTime = 0.1f;
    [SerializeField] private float m_deathTimerDefaultValue = 1.5f;
    private float m_deathTimer;
    private float m_rotateSmoothVelocity;
    private float currentSpeed;
    
    private Vector3 m_direction;
    private Vector2 m_moveInput;

    private int m_isColliding = 0;
    private bool isOiled = false;
    private bool isPlayingOilSound = false;
    private bool isPlayingFootSound = false;

    void Start()
    {
        m_characterState = CharacterState.Idle;
        m_oldCharacterState = m_characterState;
        m_direction = Vector3.zero;
        m_moveInput = Vector2.zero;
        m_deathTimer = m_deathTimerDefaultValue;
        currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        SetAnimations();
        
        if (m_isColliding > 0 && m_deathTimer >= 0.0f)
        {
            Debug.Log(m_deathTimer);
            m_deathTimer -= Time.deltaTime;
        }
    }

    private void Move()
    {
        if (isOiled)
        {
            currentSpeed = speed * 0.5f;
            if (m_moveInput.magnitude >= 0.1f && !isPlayingOilSound)
            {
                PlayRandomOilSound();
                footStep.Stop();
                isPlayingOilSound = true;
                isPlayingFootSound = false;
            }
            else if (m_moveInput.magnitude < 0.1f)
            {
                foreach (AudioSource audioSource in m_audioSources)
                    audioSource.Stop();
                isPlayingOilSound = false;
            }
        }
        else
        {
            currentSpeed = speed;
            foreach (AudioSource audioSource in m_audioSources)
                audioSource.Stop();
            isPlayingOilSound = false;
        }

        m_direction = new Vector3(m_moveInput.x, 0.0f, m_moveInput.y).normalized;
        
        m_oldCharacterState = m_characterState;    // Save the current CharacterState to check if there is a difference later
        
        if (m_direction.magnitude >= 0.1f)
        {
            m_characterState = CharacterState.Running;
            if (!isPlayingFootSound && !isOiled)
            {
                isPlayingFootSound = true;
                footStep.Play();
            }
            controller.Move(m_direction * currentSpeed * Time.deltaTime);
            // Smooth rotation
            float targetAngle = Mathf.Atan2(m_direction.x, m_direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_rotateSmoothVelocity, rotateSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }
        else
        {
            m_characterState = CharacterState.Idle;
            isPlayingFootSound = false;
            footStep.Stop();
        }
        
    }

    private void SetAnimations()
    {
        if (m_characterState == CharacterState.Idle)
        {
            if (m_oldCharacterState == CharacterState.Running)
            {
                // Set animation run animation
                GetComponentInChildren<Animator>().SetFloat("speed", m_direction.magnitude * speed);
                // Set next random animation
                GetComponentInChildren<Animator>().SetInteger("idleIndex", Random.Range(0, 3));
            }
        }
        else if (m_characterState == CharacterState.Running)
        {
            // Set animation run animation
            GetComponentInChildren<Animator>().SetFloat("speed", m_direction.magnitude * speed);
        }
    }
    
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.tag.Equals("SunRay"))
        {
            if (m_isColliding == 0)
            {
                StartCoroutine(DeathCoroutine());
            }
            m_isColliding++;
            radiation.Play();
        }

        if (_other.CompareTag("Car"))
            Die();

        if (_other.CompareTag("Puddle"))
            isOiled = true;
    }

    private void OnTriggerExit(Collider _other)
    {
        if (_other.gameObject.tag.Equals("SunRay"))
        {
            m_isColliding--;
            
            if (m_isColliding == 0)
            {
                radiation.Stop();
                m_deathTimer = m_deathTimerDefaultValue;
            }
        }

        if (_other.CompareTag("Puddle"))
            isOiled = false;
    }

    public void MoveInput(InputAction.CallbackContext _callbackContext)
    {
        m_moveInput = _callbackContext.ReadValue<Vector2>();
    }

    private void Die()
    {
        GameManager.Instance.gameState = GameManager.GameStates.GameOver;
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(m_deathTimerDefaultValue);
        if (m_isColliding > 0)
        {
            radiation.Stop();
            burn.Play();
            Die();
        }
    }
    private void PlayRandomOilSound()
    {
        int index = Random.Range(0, m_audioSources.Length);
        Debug.Log(index);
        m_audioSources[index].Play();
    }
}
