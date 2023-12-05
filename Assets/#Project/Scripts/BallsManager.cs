using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallsManager : MonoBehaviour
{
    public GameObject playingGoal = null;
    public GameObject goalPrefab;

    public GameObject playingBall = null;
    public GameObject[] ballPrefabs;

    public Transform spawnArea;

    public int[] numberOfShoots = new int[2] { 2, 2 };

    public Rigidbody playingGoalRigidbody;
    public Rigidbody playingBallRigidbody;



    public void CreateGoal()
    {
        // Instantier un nouveau goal
        GameObject newGoal = Instantiate(goalPrefab, spawnArea.position, transform.rotation);
        Debug.LogWarning("nouveau goal créé");
        
        // Assurer que le nouveau goal devient le goal actif
        playingGoal = newGoal;
        
        // Récupérer le Rigidbody du goal actif
        playingGoalRigidbody = playingGoal.GetComponent<Rigidbody>();

        // Positionner le goal actif à la zone de spawn si nécessaire
        if (playingGoalRigidbody.position != spawnArea.position)
        {
            playingGoalRigidbody.position = spawnArea.position;
        }
    }

    public void CreateBall(int playerNumber)
    {
        int index = playerNumber - 1;

        // Instantier une nouvelle balle
        GameObject newBall = Instantiate(ballPrefabs[index], spawnArea.position, transform.rotation);
        Debug.LogWarning("nouvelle balle créé");

        
        // Assurer que la nouvelle balle devient la balle actuelle
        playingBall = newBall;

        // Récupérer le Rigidbody de la balle actuelle
        playingBallRigidbody = playingBall.GetComponent<Rigidbody>();

        // Positionner la balle actuelle à la zone de spawn si nécessaire
        if (playingBallRigidbody.position != spawnArea.position)
        {
            playingBallRigidbody.position = spawnArea.position;
        }

        // Décrémenter le nombre de tirs disponibles
        numberOfShoots[index] -= 1;
    }

    public void RemoveAllBalls()
    {
        // Détruire le goal actif
        Destroy(playingGoal);
        Debug.Log("destroying goal");

        // Trouver et détruire toutes les balles avec le tag "Ball"
        GameObject[] allBalls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in allBalls)
        {
            Destroy(ball);
            Debug.Log("destroying balls");
        }
    }

    public void ResetRound()
    {
        // Réinitialiser le nombre de tirs
        numberOfShoots = new int[2] { 2, 2 };
        Debug.LogWarning("Number of shoots restored");

        RemoveAllBalls();


        Debug.Log("Resetting round...");
        // Créer un nouveau goal
        CreateGoal();

        // Créer une nouvelle balle pour le joueur 1
        CreateBall(1);
    }

    public bool PlayerStillHaveShoot(int playerNumber)
    {
        int index = playerNumber - 1;
        return numberOfShoots[index] > 0;
    }
}
