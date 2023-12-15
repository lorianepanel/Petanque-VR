using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]

public class GoalBehaviour : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
    void OnCollisionEnter(Collision collision)
    {
        // Vérifier si la collision se produit avec le sol (ajuster le tag selon votre configuration)
        if (collision.gameObject.CompareTag("Sol"))
        {
            // Jouer le son
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
