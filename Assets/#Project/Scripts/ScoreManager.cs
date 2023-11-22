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

    private float smallestDistance;

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
    void Start(){

    }

    void Update(){
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
    }

    // public void UpdateCheckTheDistance()
    // {
    //     if(goal == null || balls.Count == 0) return;

    //     BallBehaviour.PlayerColor currentPlayerColor;
    //     float distance = CheckTheDistance(out currentPlayerColor);
    //     distanceText.SetText($"Closest player : {currentPlayerColor} <br>Distance from goal : {distance:f}m");
        
        
    //     scoreP1 = 0;
    //     scoreP2 = 0;
    //     BallBehaviour.PlayerColor memoColor = currentPlayerColor;

    //     if(memoColor == currentPlayerColor)
    //     {
    //         if(currentPlayerColor == BallBehaviour.PlayerColor.Blue)
    //         {
    //             scoreP1++;
    //         } 
    //         else if(currentPlayerColor == BallBehaviour.PlayerColor.Red)
    //         {
    //             scoreP2++;
    //         } 
    //         CheckTheDistance(out currentPlayerColor);
    //     } 

    // }

    // public void AddPoints()
    // {
    //     scoreP1Text.SetText($"P1 : {scoreP1} points");
    //     scoreP2Text.SetText($"P2 : {scoreP2} points");
    // }




    // methode qui check la distance entre toutes les boules et le goal, out la couleur de la ball la plus près, return la plus petite distance
    public float CheckTheDistance(out BallBehaviour.PlayerColor playerColor)
    {

        playerColor = BallBehaviour.PlayerColor.None;
        smallestDistance = Mathf.Infinity;

        foreach (GameObject ball in balls)
        {
            float distanceFromGoal = Vector3.Distance(ball.transform.position, goal.position);
            
            
            if(distanceFromGoal < smallestDistance)
            {
                smallestDistance = distanceFromGoal;
                playerColor = ball.GetComponent<BallBehaviour>().playerColor;              
            }
        }
        return smallestDistance;
    }

    

    public int GetTheLooser(){

        BallBehaviour.PlayerColor playerColor = BallBehaviour.PlayerColor.None;
        CheckTheDistance(out playerColor);  
        Debug.Log($"Player distance: {playerColor}"); 


        int nPlayer = 0;
        bool ok = false;
        foreach (BallBehaviour.PlayerColor ballColor in BallBehaviour.playerColors){
            nPlayer++;
            if(ballColor == playerColor) {
                ok = true;
                break;
            }
        }
        
        if(!ok)
            return -1;

        if(nPlayer == 1) return 2;
        return 1;
    }

}
