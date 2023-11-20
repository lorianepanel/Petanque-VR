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

    private float closestDistance;

    [SerializeField]
    private Transform _goal;
    
    public Transform goal {
        get { 
            if (_goal == null) {
                _goal = GameObject.FindWithTag("Goal")?.transform;
                }
            return _goal;}
    }

    // private int scoreP1;
    // private int scoreP2;


    void Start()
    {
        // scoreP1 = 0;
        // scoreP2 = 0;
        Debug.Log($"Nombre de balles dans la liste : {balls.Count}");
    }
    

    void Update()
    {
        if(goal == null || balls.Count == 0) return;


        BallBehaviour.PlayerColor currentPlayerColor;
        float distance = CheckTheDistance(out currentPlayerColor);
        distanceText.SetText($"Closest player : {currentPlayerColor} <br>Distance from goal : {distance:f}m");
        
        
        int scoreP1 = 0;
        int scoreP2 = 0;
        BallBehaviour.PlayerColor memoColor = currentPlayerColor;

        if(memoColor == currentPlayerColor)
        {
            if(currentPlayerColor == BallBehaviour.PlayerColor.Blue)
            {
                scoreP1++;
                scoreP1Text.SetText($"P1 : {scoreP1} points");
            } 
            else if(currentPlayerColor == BallBehaviour.PlayerColor.Red)
            {
                scoreP2++;
                scoreP2Text.SetText($"P2 : {scoreP2} points");
            } 
            CheckTheDistance(out currentPlayerColor);
        }


        Debug.Log($"Nombre de balles dans la liste : {balls.Count}");   



    }




    // methode qui check la distance entre toutes les boules et le goal, out la couleur de la ball la plus près, return la plus petite distance
    public float CheckTheDistance(out BallBehaviour.PlayerColor playerColor)
    {

        
        playerColor = BallBehaviour.PlayerColor.None;
        closestDistance = Mathf.Infinity;
        //GameObject memorizedBall = null;

        foreach (GameObject ball in balls)
        {
            float distanceFromGoal = Vector3.Distance(ball.transform.position, goal.position);
            // Debug.Log($"{ball.name} : {distanceFromGoal}");
            

            if(distanceFromGoal < closestDistance)
            {
                closestDistance = distanceFromGoal;
                playerColor = ball.GetComponent<BallBehaviour>().playerColor;
                //memorizedBall = ball;               
            }
        }
        //if (memorizedBall != null) balls.Remove(memorizedBall);
        return closestDistance;
    }

    

    public int GetTheLooser(){

        if (balls.Count == 2) return 1;

        if (balls.Count == 1) return 2;


        BallBehaviour.PlayerColor playerColor = BallBehaviour.PlayerColor.None;
        CheckTheDistance(out playerColor);   

        int nPlayer = 0;
        foreach (BallBehaviour.PlayerColor ballColor in BallBehaviour.playerColors){
            if(ballColor == playerColor) return nPlayer;
            nPlayer++;
        }
        
        return -1;
    }

}
