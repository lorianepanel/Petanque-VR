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

    public UnityEvent whenStateChanged;

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

    private BallsManager ballsManager;

    private bool isRoundRestarting = false;
    

    void Start()
    {

        ballsManager = FindObjectOfType<BallsManager>();
        terrainInfo = FindObjectOfType<TerrainInfo>();
        scoreManager = FindObjectOfType<ScoreManager>();

        if (ballsManager == null || terrainInfo == null || scoreManager == null)
        {
            Debug.LogError("One or more managers are not assigned!");
        }
        else
        {
            state = GameState.WaitForGoal;
            previousState = state;
            ballsManager.CreateGoal();
            ballsManager.CreateBall(1);
        }
    }


    void Update()
    {
        UpdateState();
    }



    private void UpdateWaitForGoal()
    {
        if(terrainInfo.IsIn(ballsManager.playingGoal)){
            state = GameState.GoalLaunched;
        }
    }

    private void UpdateGoalLaunched()
    {
        if(terrainInfo.IsStable(ballsManager.playingGoal))
        {
            state = GameState.WaitForP1;
        }

    }

    private void UpdateWaitForP1()
    {

        if(terrainInfo.IsIn(ballsManager.playingBall) && terrainInfo.IsStable(ballsManager.playingBall))
        {
            scoreManager.UpdateCheckTheDistance();
            state = GameState.P1HasPlayed;
        }
    }

    private void UpdateP1HasPlayed()
    {

        // Debug.Log($"The looser is: {scoreManager.GetTheLooser()}");

        //condition a : si P1 et P2 n'ont plus de balles : état RoundFinished
        if (!ballsManager.PlayerStillHaveShoot(1) && !ballsManager.PlayerStillHaveShoot(2))
        {
            scoreManager.UpdateCheckTheDistance();
            scoreManager.CalculateScores();
            state = GameState.RoundFinished;
        }

        // condition b : si P1 a encore des balles et si P1 est perdant ou si P2 n'a plus de balles => P1 rejoue
        else if(ballsManager.PlayerStillHaveShoot(1) && (scoreManager.GetTheLooser() == 1 || !ballsManager.PlayerStillHaveShoot(2)))
        {
            // Debug.Log($"P1 TURN");
            state = GameState.WaitForP1;
            ballsManager.CreateBall(1);
            
        }
        
        // condition c : si P2 a encore des balles et si P2 est perdant ou qi P1 n'a plus de balles => au tour de P2
        else if(ballsManager.PlayerStillHaveShoot(2) && (scoreManager.GetTheLooser() == 2 || !ballsManager.PlayerStillHaveShoot(1))){
            // Debug.Log($"P2 TURN");
            state = GameState.WaitForP2;
            ballsManager.CreateBall(2);
        }
    }

    private void UpdateWaitForP2()
    {
        
        if(terrainInfo.IsIn(ballsManager.playingBall) && terrainInfo.IsStable(ballsManager.playingBall))
        {
            scoreManager.UpdateCheckTheDistance();
            state = GameState.P2HasPlayed;
        }
        else return;
    }

    private void UpdateP2HasPlayed()
    {
        // Debug.Log($"Player1 still have shoot: {PlayerStillHaveShoot(1)}");
        // Debug.Log($"Player2 still have shoot: {PlayerStillHaveShoot(2)}");
        Debug.Log($"The looser is: {scoreManager.GetTheLooser()}");

        if (!terrainInfo.IsIn(ballsManager.playingBall) || !terrainInfo.IsStable(ballsManager.playingBall))
        {
            // Debug.Log($"Is in : {terrainInfo.IsIn(currentBall)}, Is stable : {terrainInfo.IsStable(currentBall)}.");
            return;
        }

        //condition a' : si P1 et P2 n'ont plus de balles : état RoundFinished
        else if (!ballsManager.PlayerStillHaveShoot(1) && !ballsManager.PlayerStillHaveShoot(2))
        {
            scoreManager.UpdateCheckTheDistance();
            scoreManager.CalculateScores();
            state = GameState.RoundFinished;   
        }

        // condition b' : si P2 a encore des balles et si P2 est perdant ou si P1 n'a plus de balles => P2 rejoue
        else if(ballsManager.PlayerStillHaveShoot(2) && (scoreManager.GetTheLooser() == 2 || !ballsManager.PlayerStillHaveShoot(1)))
        {
            state = GameState.WaitForP2;
            ballsManager.CreateBall(2);
        }
        
        // condition c' : si P1 a encore des balles et si P1 est perdant ou si P2 n'a plus de balles => au tour de P1
        else if(ballsManager.PlayerStillHaveShoot(1) && (scoreManager.GetTheLooser() == 1 || !ballsManager.PlayerStillHaveShoot(2))){
            state = GameState.WaitForP1;
            ballsManager.CreateBall(1);
        }
    }

    // private void UpdateRoundFinished()
    // {
    //     Debug.Log("Update Round finished");

    //     if(scoreManager.scoreP1 == scoreManager.winningScore || scoreManager.scoreP2 == scoreManager.winningScore)
    //     {
    //         Debug.Log("Scene des scores démarre");
    //     }
    //     else if (scoreManager.scoreP1 < scoreManager.winningScore || scoreManager.scoreP2 < scoreManager.winningScore)
    //     {
    //         Debug.Log("Resetting round...");
    //         ballsManager.RemoveAllBalls();
    //         state = GameState.WaitForGoal;
    //         ballsManager.ResetRound();
    //     }
    // }

    private void UpdateRoundFinished()
    {
        Debug.Log("Update Round finished");

        if(scoreManager.scoreP1 == scoreManager.winningScore || scoreManager.scoreP2 == scoreManager.winningScore)
        {
            Debug.Log("Scene des scores démarre");
        }
        else if (scoreManager.scoreP1 < scoreManager.winningScore || scoreManager.scoreP2 < scoreManager.winningScore)
        {
            Debug.Log("Resetting round...");

            
            
            

            // Vérifier si le round est déjà en train de se réinitialiser
            if (!isRoundRestarting)
            {
                // Démarrer la Coroutine pour gérer le délai avant de réinitialiser le round
                StartCoroutine(RestartRoundCoroutine());
            }

            // ballsManager.RemoveAllBalls();
            state = GameState.WaitForGoal;
        }
    }

    private IEnumerator RestartRoundCoroutine()
    {
        // Indiquer que le round est en cours de réinitialisation
        isRoundRestarting = true;

        // Attendre pendant un certain délai (par exemple, 3 secondes)
        float delay = 3f;
        yield return new WaitForSeconds(delay);

        // Réinitialiser le round
        ballsManager.ResetRound();

        // Réinitialiser l'indicateur
        isRoundRestarting = false;
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



}
