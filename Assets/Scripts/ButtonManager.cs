using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    GameSetup gameSetup;
    GameManager gameManager;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameSetup = FindObjectOfType<GameSetup>();
    }
    public void AddPlayer()
    {
        gameSetup.Begin();
    }
    public void Deal()
    {
        StartCoroutine(gameManager.Deal());
    }
}
