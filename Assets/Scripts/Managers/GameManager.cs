using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private int savedGummingsToWin;
    private int savedGummings = 0;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("There is more than one instance of GameManager existing.");
            return;
        }
        instance = this;
    }

    public void AddGummingSaved() 
    {
        savedGummings++;
        CheckWinConditions();
    }

    private void CheckWinConditions()
    {
        if(savedGummings >= savedGummingsToWin)
        {
            Debug.Log("You won !!");
        }
    }
}
