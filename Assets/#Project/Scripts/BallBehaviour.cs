using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class BallBehaviour : MonoBehaviour
{

    public enum PlayerColor
    {
        Red,
        Blue,
        None
    }

    public static PlayerColor[] playerColors = new PlayerColor[2] { PlayerColor.Blue, PlayerColor.Red};


    public PlayerColor playerColor;

    
    void Start()
    {

    }

    void Update()
    {

    }

}
