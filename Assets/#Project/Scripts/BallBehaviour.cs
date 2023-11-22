using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class BallBehaviour : MonoBehaviour
{

    public enum PlayerNumber
    {
        P1,
        P2,
        None
    }

    public static PlayerNumber[] playerNumbers = new PlayerNumber[2] { PlayerNumber.P1, PlayerNumber.P2};


    public PlayerNumber playerNumber;


}
