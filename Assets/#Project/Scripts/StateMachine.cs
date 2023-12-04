using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.XR.CoreUtils;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class StateMachine : MonoBehaviour
{
    [HideInInspector] public GameObject goal;

    public GameObject goalPrefab;

    public GameObject playingBall;

    public GameObject[] ballPrefabs;


    public int[] numberOfShoots = new int[2] {3,3};
    

    public UnityEvent whenStateChanged;

    [SerializeField] Transform stockArea;

    [SerializeField] Transform spawnArea;


    public enum GameState{
        WaitForGoal,
        GoalLaunched,
        WaitForP1,
        P1HasPlayed,
        WaitForP2,
        P2HasPlayed,
        RoundFinished
    }

    public GameState state;
    public GameState previousState;

    [SerializeField]
    private TMP_Text currentStateText;

    private TerrainInfo terrainInfo;

    private ScoreManager scoreManager;


    

    // Start is called before the first frame update
    void Start()
    {

        if(terrainInfo == null){
            terrainInfo = FindObjectOfType<TerrainInfo>();
        }

        if(scoreManager == null){
            scoreManager = FindObjectOfType<ScoreManager>();
        }

        state = GameState.WaitForGoal;
        previousState = state;

    }


    void Update()
    {
        UpdateState();
    }



    private void UpdateWaitForGoal()
    {
        if (goal == null){
            SpawnGoal();
        }


        if(terrainInfo.IsIn(goal)){
            // si goal est sur le terrain : état suivant
            state = GameState.GoalLaunched;
        }
    }

    private void UpdateGoalLaunched()
    {
        if(terrainInfo.IsStable(goal))
        {
            // si goal est stable : état suivant et ballP1 apparait
            state = GameState.WaitForP1;
            
            SpawnBall(1);      
        }
    }

    private void UpdateWaitForP1()
    {
        if(terrainInfo.IsIn(playingBall) && terrainInfo.IsStable(playingBall))
        {
            // si BallP1 est sur le terrain et stable : état suivant
            // Debug.Log($"Is in : {terrainInfo.IsIn(currentBall)}, Is stable : {terrainInfo.IsStable(currentBall)}.");
            scoreManager.UpdateCheckTheDistance();
            state = GameState.P1HasPlayed;
        }
    }

    private void UpdateP1HasPlayed()
    {
        
        // Debug.Log($"Player1 still have shoot: {PlayerStillHaveShoot(1)}");
        // Debug.Log($"Player2 still have shoot: {PlayerStillHaveShoot(2)}");
        Debug.Log($"The looser is: {scoreManager.GetTheLooser()}");

        //condition a : si P1 et P2 n'ont plus de balles : état RoundFinished
        if (!PlayerStillHaveShoot(1) && !PlayerStillHaveShoot(2))
        {
            scoreManager.UpdateCheckTheDistance();
            scoreManager.CalculateScores();
            state = GameState.RoundFinished;
        }

        // condition b : si P1 a encore des balles et si P1 est perdant ou si P2 n'a plus de balles => P1 rejoue
        else if(PlayerStillHaveShoot(1) && (scoreManager.GetTheLooser() == 1 || !PlayerStillHaveShoot(2)))
        {
            // Debug.Log($"P1 TURN");
            state = GameState.WaitForP1;
            SpawnBall(1);
        }
        
        // condition c : si P2 a encore des balles et si P2 est perdant ou qi P1 n'a plus de balles => au tour de P2
        else if(PlayerStillHaveShoot(2) && (scoreManager.GetTheLooser() == 2 || !PlayerStillHaveShoot(1))){
            // Debug.Log($"P2 TURN");
            state = GameState.WaitForP2;
            SpawnBall(2);
        }
    }

    private void UpdateWaitForP2()
    {
        if(terrainInfo.IsIn(playingBall) && terrainInfo.IsStable(playingBall))
        {
            scoreManager.UpdateCheckTheDistance();
            // si BallP2 est sur le terrain et stable : état suivant
            state = GameState.P2HasPlayed;
        }
        else return;
    }

    private void UpdateP2HasPlayed()
    {
        // Debug.Log($"Player1 still have shoot: {PlayerStillHaveShoot(1)}");
        // Debug.Log($"Player2 still have shoot: {PlayerStillHaveShoot(2)}");
        Debug.Log($"The looser is: {scoreManager.GetTheLooser()}");

        if (!terrainInfo.IsIn(playingBall) || !terrainInfo.IsStable(playingBall))
        {
            // Debug.Log($"Is in : {terrainInfo.IsIn(currentBall)}, Is stable : {terrainInfo.IsStable(currentBall)}.");
            return;
        }

        //condition a' : si P1 et P2 n'ont plus de balles : état RoundFinished
        else if (!PlayerStillHaveShoot(1) && !PlayerStillHaveShoot(2))
        {
            scoreManager.UpdateCheckTheDistance();
            scoreManager.CalculateScores();
            state = GameState.RoundFinished;   
        }

        // condition b' : si P2 a encore des balles et si P2 est perdant ou si P1 n'a plus de balles => P2 rejoue
        else if(PlayerStillHaveShoot(2) && (scoreManager.GetTheLooser() == 2 || !PlayerStillHaveShoot(1)))
        {
            state = GameState.WaitForP2;
            SpawnBall(2);
        }
        
        // condition c' : si P1 a encore des balles et si P1 est perdant ou si P2 n'a plus de balles => au tour de P1
        else if(PlayerStillHaveShoot(1) && (scoreManager.GetTheLooser() == 1 || !PlayerStillHaveShoot(2))){
            state = GameState.WaitForP1;
            SpawnBall(1);
        }
    }

    private void UpdateRoundFinished()
    {
        Debug.Log("Update Round finished");

        RemoveAllBalls();


        if(scoreManager.scoreP1 == scoreManager.winningScore || scoreManager.scoreP2 == scoreManager.winningScore)
        {
            Debug.Log("Scene des scores démarre");
        }
        else RestartTheGame();
    }



    public void UpdateState()
    {
        if(state != previousState){
            whenStateChanged?.Invoke();
        }
        previousState = state;
        switch (state)
        {
            case GameState.WaitForGoal:
            currentStateText.SetText("P1 : Throw the goal");
            UpdateWaitForGoal();
            break;

            case GameState.GoalLaunched:
            currentStateText.SetText("Goal launched !");
            UpdateGoalLaunched();
            break;

            case GameState.WaitForP1:
            currentStateText.SetText("P1 : Your turn");
            UpdateWaitForP1();
            break;

            case GameState.P1HasPlayed:
            currentStateText.SetText("P1 has played");
            UpdateP1HasPlayed();
            break;

            case GameState.WaitForP2:
            currentStateText.SetText("P2 : Your turn");
            UpdateWaitForP2();
            break;

            case GameState.P2HasPlayed:
            currentStateText.SetText("P2 has played");
            UpdateP2HasPlayed();
            break;

            case GameState.RoundFinished:
            currentStateText.SetText("Round finished");
            UpdateRoundFinished();
            break;
        }

        
    }




    private void SpawnGoal()
    {
        goal = Instantiate(goalPrefab, stockArea.position, transform.rotation);

        if(goal.GetComponent<Rigidbody>().position != spawnArea.position)
        {
            goal.GetComponent<Rigidbody>().position = spawnArea.position;
        }
    }



    private void SpawnBall(int playerNumber)
    {
        int index  = playerNumber - 1;

        playingBall = Instantiate(ballPrefabs[index], stockArea.position, transform.rotation);

        if(playingBall.GetComponent<Rigidbody>().position != spawnArea.position)
        {
            playingBall.GetComponent<Rigidbody>().position = spawnArea.position;
        }

        numberOfShoots[index] -= 1;
        
    }

    private void RemoveAllBalls()
    {
        if (goal != null && goal.GetComponent<Rigidbody>().position != spawnArea.position)
        {
            Destroy(goal);
        }


        GameObject[] allBalls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in allBalls)
        {
        if (ball != null && ball.GetComponent<Rigidbody>() != null)
            {
                Destroy(ball);
            }
        }

    }

    private void RestartTheGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    private bool PlayerStillHaveShoot(int playerNumber){
        int index  = playerNumber - 1;
        return numberOfShoots[index]> 0;
    }

}
