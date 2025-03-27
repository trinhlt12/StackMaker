using System;
using System.Collections;
using System.Collections.Generic;
using _GAME.Scripts;
using UnityEngine;

public class BrickBlock : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            if (player == null)
            {
                return;
            }
            player.PickUpBrick(this.gameObject);
        }
    }
}