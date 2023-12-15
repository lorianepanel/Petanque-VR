using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]

public class BallBehaviour : MonoBehaviour
{
    private AudioSource audioSource;
    public enum PlayerNumber
    {
        P1,
        P2,
        None
    }

    public static PlayerNumber[] playerNumbers = new PlayerNumber[2] { PlayerNumber.P1, PlayerNumber.P2};
    public PlayerNumber playerNumber;


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
