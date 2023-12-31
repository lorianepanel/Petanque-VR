using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class ScoreManager : MonoBehaviour
{
    public List<GameObject> balls;

    public TMP_Text scoreP1Text;
    public TMP_Text scoreP2Text;

    public TMP_Text distanceText;

    private float smallestDistanceFromGoal;

    [SerializeField]
    private Transform _goal;
    
    public Transform goal {
        get { 
            if (_goal == null) {
                _goal = GameObject.FindWithTag("Goal")?.transform;
                }
            return _goal;}
    }

    [HideInInspector]
    public int scoreP1 = 0;

    [HideInInspector]
    public int scoreP2 = 0;

    public int winningScore = 13;


    public void UpdateCheckTheDistance()
    {
        if(goal == null || balls.Count == 0) return;

        BallBehaviour.PlayerNumber currentPlayerNumber;
        float distance = CheckTheDistance(out currentPlayerNumber);
        distanceText.SetText($"{currentPlayerNumber} is closer <br>{distance:f}m");   
    }


    // methode qui check la distance entre toutes les boules et le goal, out la couleur de la ball la plus près, return la plus petite distance
    public float CheckTheDistance(out BallBehaviour.PlayerNumber playerNumber)
    {
        playerNumber = BallBehaviour.PlayerNumber.None;
        smallestDistanceFromGoal = Mathf.Infinity;

        foreach (GameObject ballObject in balls)
        {
            // Assurez-vous que la référence à la balle n'est pas nulle
            if (ballObject != null)
            {
                // Assurez-vous que la balle est toujours valide avant d'accéder à ses composants
                BallBehaviour ball = ballObject.GetComponent<BallBehaviour>();
                if (ball != null)
                {
                    float distanceBetweenGoalAndBall = Vector3.Distance(ball.transform.position, goal.position);

                    if (distanceBetweenGoalAndBall < smallestDistanceFromGoal)
                    {
                        smallestDistanceFromGoal = distanceBetweenGoalAndBall;
                        playerNumber = ball.playerNumber;
                    }
                }
            }
        }
        return smallestDistanceFromGoal;
    }

    public int GetTheLooser(){

        BallBehaviour.PlayerNumber playerNumber = BallBehaviour.PlayerNumber.None;
        CheckTheDistance(out playerNumber);  

        int nPlayer = 0;
        bool ok = false;
        foreach (BallBehaviour.PlayerNumber ballNumber in BallBehaviour.playerNumbers){
            nPlayer++;
            if(ballNumber == playerNumber) {
                ok = true;
                break;
            }
        }
        
        if(!ok)
            return -1;

        if(nPlayer == 1) return 2;
        return 1;
    }

    public void CalculateScores()
    {
        BallBehaviour.PlayerNumber winner;
        CheckTheDistance(out winner);
        
        int scoreP1Increment = 0;
        int scoreP2Increment = 0;

        if (winner == BallBehaviour.PlayerNumber.P1){
            scoreP1Increment += 1;
        }
        else if (winner == BallBehaviour.PlayerNumber.P2){
            scoreP2Increment += 1;
        }

        // Mettez à jour les scores affichés
        scoreP1 += scoreP1Increment;
        scoreP2 += scoreP2Increment;

        scoreP1Text.SetText($"{scoreP1}");
        scoreP2Text.SetText($"{scoreP2}");
    }

}
