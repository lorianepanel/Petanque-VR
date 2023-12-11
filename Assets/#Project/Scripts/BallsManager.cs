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

    private TerrainInfo terrainInfo;
    private Collider terrainCollider;


    void Start() {
        // Récupérer le Collider du TerrainInfo
        terrainInfo = FindObjectOfType<TerrainInfo>();

        if (terrainInfo != null)
        {
            terrainCollider = terrainInfo.GetComponent<Collider>();
            Debug.Log("TerrainInfo collider found");
        }
        else
        {
            Debug.LogError("BallsManager needs TerrainInfo");
        }
    }



    public void CreateGoal()
    {
        // Instantier un nouveau goal
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

        // Instantier une nouvelle balle
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
        // Détruire le goal actif
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


    public void AIPlay(float delay)
    {
        if (!isAIPlaying && terrainInfo != null)
        {
            StartCoroutine(AIPlayCoroutine(delay));
        }
    }

    private IEnumerator AIPlayCoroutine(float delay)
    {
        isAIPlaying = true;

        yield return new WaitForSeconds(delay);

        Debug.Log("AI playing");

        // Distance minimale et maximale autour du goal
        float minDistance = 0.1f; // Distance minimale souhaitée
        float maxDistance = 0.9f; // Distance maximale souhaitée


        // Tant que la position générée est en dehors du terrain, continuez à générer une nouvelle position
        Vector3 randomPosition;
        do
        {
            // Obtenez une position aléatoire en 2D autour du goal
            Vector2 randomOffset2D = Random.insideUnitCircle.normalized * Random.Range(minDistance, maxDistance);

            // Utilisez la position actuelle du goal pour les coordonnées Y
            float fixedPositionY = playingGoalRb.position.y;

            // Créez le vecteur de position avec la coordonnée Y fixe et les valeurs aléatoires pour X et Z
            randomPosition = new Vector3(playingGoalRb.position.x + randomOffset2D.x, fixedPositionY, playingGoalRb.position.z + randomOffset2D.y);

        } while (!IsPositionInsideTerrain(randomPosition, terrainCollider));

        // Définissez la position de la balle sur la nouvelle position aléatoire
        playingBallRb.transform.position = randomPosition;

        yield return new WaitForSeconds(delay);

        isAIPlaying = false;
    }

    // Fonction pour vérifier si une position est à l'intérieur du terrain
    private bool IsPositionInsideTerrain(Vector3 position, Collider terrainCollider)
    {
        return terrainCollider.bounds.Contains(position);
    }
     
}
