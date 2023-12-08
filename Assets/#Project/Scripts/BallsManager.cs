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

    public Rigidbody playingGoalRb;
    public Rigidbody playingBallRb;

    private bool isAIPlaying = false;

    public bool IsAIPlaying()
    {
        return isAIPlaying;
    }


    public void CreateGoal()
    {
        // Instantier un nouveau goal
        GameObject newGoal = Instantiate(goalPrefab, spawnArea.position, transform.rotation);
        Debug.LogWarning("nouveau goal créé");
        
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

        // Instantier une nouvelle balle
        GameObject newBall = Instantiate(ballPrefabs[index], spawnArea.position, transform.rotation);
        Debug.LogWarning("nouvelle balle créé");

        
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
        // Détruire le goal actif
        Destroy(playingGoal);
        Debug.Log("destroying goal");

        // Trouver et détruire toutes les balles avec le tag "Ball"
        GameObject[] allBalls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in allBalls)
        {
            Destroy(ball);
        }
        Debug.Log("destroying all balls");
    }

    public void ResetRound()
    {
        Debug.Log("Resetting round...");

        // Réinitialiser le nombre de tirs
        numberOfShoots = new int[2] { 2, 2 };
        Debug.Log("Number of shoots restored");

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


    public void PlayForP2WithDelay(float delay)
    {
        if (!isAIPlaying)
        {
            StartCoroutine(PlayForP2Coroutine(delay));
        }
    }

    private IEnumerator PlayForP2Coroutine(float delay)
    {
        isAIPlaying = true;

        yield return new WaitForSeconds(delay);

        Debug.Log("AI playing");

        // Get a random position around the goal
        Vector3 randomOffset = Random.onUnitSphere * 2f; // You can adjust the magnitude (2f) as needed
        Vector3 newPosition = playingGoalRb.position + randomOffset;

        // Set the position of the playing ball to the new random position
        playingBallRb.transform.position = newPosition;

        isAIPlaying = false;
    }


    
}
