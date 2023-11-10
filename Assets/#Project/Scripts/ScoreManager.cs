using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class ScoreManager : MonoBehaviour
{
    private List<GameObject> balls;
    private Transform goal;

    public TMP_Text scorePlayerBlueText;
    public TMP_Text scorePlayerRedText;

    // public TMP_Text distanceText;

    private float closestDistance;



    // public BallBehaviour ballIsStabilized;

    // [SerializeField]
    // private bool isStabilized;




    void Start()
    {
        // Instanciate()
        goal = GameObject.FindWithTag("Goal")?.transform;

        // isStabilized = ballIsStabilized.isStable;
        // isStabilized = false;
        
    }


    void Update()
    {

        
        balls = new(GameObject.FindGameObjectsWithTag("Ball"));

        // if(isStabilized == true) AddScore();

        AddScore();


    }


    // fonction pour gérer le score qui est lancée quand isStable = true
    void AddScore()
    {
        if(goal == null || balls.Count == 0) return;
        BallBehaviour.PlayerColor currentPlayerColor;
        float distance = CheckTheDistance(out currentPlayerColor);
        // distanceText.SetText($"Distance from goal : {distance:f}m");
        
        int scorePlayerBlue = 0;
        int scorePlayerRed = 0;
        BallBehaviour.PlayerColor memoColor = currentPlayerColor;

        while(memoColor == currentPlayerColor)
        {
            if(currentPlayerColor == BallBehaviour.PlayerColor.Blue)
            {
                scorePlayerBlue++;
                scorePlayerBlueText.SetText($"Blue : {scorePlayerBlue} points");
            } 
            else if(currentPlayerColor == BallBehaviour.PlayerColor.Red)
            {
                scorePlayerRed++;
                scorePlayerRedText.SetText($"Red : {scorePlayerRed} points");
            } 
            CheckTheDistance(out currentPlayerColor);
        }
    }


    // methode qui check la distance entre toutes les boules et le goal, out la couleur de la ball la plus près, return la plus petite distance
    private float CheckTheDistance(out BallBehaviour.PlayerColor playerColor)
    {

        playerColor = BallBehaviour.PlayerColor.None;
        closestDistance = Mathf.Infinity;
        GameObject memorizedBall = null;

        foreach (GameObject ball in balls)
        {
            float distanceFromGoal = Vector3.Distance(ball.transform.position, goal.position);
            // Debug.Log($"{ball.name} : {distanceFromGoal}");

            if(distanceFromGoal < closestDistance)
            {
                closestDistance = distanceFromGoal;
                playerColor = ball.GetComponent<BallBehaviour>().playerColor;
                memorizedBall = ball;               
            }
        }
        if (memorizedBall != null) balls.Remove(memorizedBall);
        return closestDistance;
    }

}
