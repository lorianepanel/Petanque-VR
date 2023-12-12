using System.Collections;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    private BallsManager ballsManager;
    private TerrainInfo terrainInfo;
    private Collider terrainCollider;

    private bool isAIPlaying = false;

    public bool IsAIPlaying()
    {
        return isAIPlaying;
    }

    // Distance minimale et maximale autour du goal
    private float minDistance = 0.09f; // Distance minimale souhaitée
    private float maxDistance = 1f; // Distance maximale souhaitée

    private float difficulty;

    
    void Start()
    {
        ballsManager = FindObjectOfType<BallsManager>();
        terrainInfo = FindObjectOfType<TerrainInfo>();

        if (ballsManager == null || terrainInfo == null)
        {
            Debug.LogError("AI needs a ballsManager and a TerrainInfo");
        }
        else
        {
            terrainCollider = terrainInfo.GetComponent<Collider>();
            Debug.Log("TerrainInfo collider found");
        }
    }

    public void AIPlay()
    {
        isAIPlaying = true;

        Debug.Log("AI playing");

        ThrowABall();

        isAIPlaying = false;
    }

    // Fonction pour vérifier si une position est à l'intérieur du terrain
    private bool IsPositionInsideTerrain(Vector3 position)
    {
        return terrainCollider.bounds.Contains(position);
    }

    private void ThrowABall()
    {
        // Obtenir une position aléatoire à l'intérieur du terrain
        Vector3 randomPosition = GetRandomPositionInsideTerrain();

        // Définir la position de la balle sur la nouvelle position aléatoire
        ballsManager.playingBallRb.transform.position = randomPosition;
    }

    private Vector3 GetRandomPositionInsideTerrain()
    {
        difficulty = Random.Range(minDistance, maxDistance);
        Debug.LogWarning(difficulty);

        Vector3 randomPosition;
        do
        {
            // Obtenir une position aléatoire en 2D autour du goal
            Vector2 randomOffset2D = Random.insideUnitCircle.normalized * difficulty;

            // Utiliser la position actuelle du goal pour les coordonnées Y
            float fixedPositionY = ballsManager.playingGoalRb.position.y;

            // Créer le vecteur de position avec la coordonnée Y fixe et les valeurs aléatoires pour X et Z
            randomPosition = new Vector3(ballsManager.playingGoalRb.position.x + randomOffset2D.x, fixedPositionY, ballsManager.playingGoalRb.position.z + randomOffset2D.y);

        } while (!IsPositionInsideTerrain(randomPosition));

        return randomPosition;
    }
}
