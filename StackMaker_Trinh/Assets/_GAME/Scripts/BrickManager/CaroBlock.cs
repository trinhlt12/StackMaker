using System;
using System.Collections;
using System.Collections.Generic;
using _GAME.Scripts;
using _GAME.Scripts.GameManager;
using UnityEngine;

public class CaroBlock : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            if(player == null)
            {
                return;
            }
            GameManager.Instance.SetGameState(GameState.Win);
        }
    }
}