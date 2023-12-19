using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]

public class GoalBehaviour : MonoBehaviour
{
    private AudioSource audioSource;

    
    public AudioClip groundCollisionSound;
    public AudioClip otherBallCollisionSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Sol"))
        {
            // Jouer le son pour le sol
            if (audioSource != null && groundCollisionSound != null)
            {
                audioSource.clip = groundCollisionSound;
                audioSource.Play();
            }
        }
        else if (collision.gameObject.CompareTag("Ball"))
        {
            // Jouer le son pour les autres balles
            if (audioSource != null && otherBallCollisionSound != null)
            {
                audioSource.clip = otherBallCollisionSound;
                audioSource.Play();
            }
        }
    }
}
