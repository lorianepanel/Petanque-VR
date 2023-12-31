using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public Rigidbody playingGoalRb;
    public Rigidbody playingBallRb;


    public void CreateGoal()
    {
        GameObject newGoal = Instantiate(goalPrefab, spawnArea.position, transform.rotation);
        // Debug.LogWarning("nouveau goal créé");
        
        // Assurer que le nouveau goal devient le goal actif
        playingGoal = newGoal;
        
        // Récupérer le Rigidbody du goal actif
        playingGoalRb = playingGoal.GetComponent<Rigidbody>();

        // Positionner le goal actif à la zone de spawn si nécessaire
        if (playingGoalRb.transform.position != spawnArea.position)
        {
            playingGoalRb.transform.position = spawnArea.position;
        }
    }

    public void CreateBall(int playerNumber)
    {
        int index = playerNumber - 1;

        GameObject newBall = Instantiate(ballPrefabs[index], spawnArea.position, transform.rotation);
        // Debug.LogWarning("nouvelle balle créé");

        // Assurer que la nouvelle balle devient la balle actuelle
        playingBall = newBall;

        // Récupérer le Rigidbody de la balle actuelle
        playingBallRb = playingBall.GetComponent<Rigidbody>();

        // Positionner la balle actuelle à la zone de spawn si nécessaire
        if (playingBallRb.transform.position != spawnArea.position)
        {
            playingBallRb.transform.position = spawnArea.position;
        }

        // Décrémenter le nombre de tirs disponibles
        numberOfShoots[index] -= 1;
    }

    public void RemoveAllBalls()
    {
        Destroy(playingGoal);
        // Debug.Log("destroying goal");

        // Trouver et détruire toutes les balles avec le tag "Ball"
        GameObject[] allBalls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in allBalls)
        {
            Destroy(ball);
        }
        // Debug.Log("destroying all balls");
    }

    public void ResetRound()
    {
        // Debug.Log("Resetting round...");

        numberOfShoots = new int[2] { 2, 2 };
        // Debug.Log("Number of shoots restored");

        CreateGoal();
        CreateBall(1);
    }

    public bool PlayerStillHaveShoot(int playerNumber)
    {
        int index = playerNumber - 1;
        return numberOfShoots[index] > 0;
    }

}
