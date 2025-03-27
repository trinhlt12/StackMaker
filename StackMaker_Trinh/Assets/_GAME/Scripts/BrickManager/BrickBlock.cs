using System;
using System.Collections;
using System.Collections.Generic;
using _GAME.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class BrickBlock : MonoBehaviour
{
    public float BrickHeight { get; private set; }

    private void Start()
    {
        this.CalculateBrickHeight();
    }

    private void CalculateBrickHeight()
    {
        this.BrickHeight = this.gameObject.GetComponent<BoxCollider>().bounds.size.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            if (player == null)
            {
                return;
            }
            player.PickUpBrick(this);
        }
    }
}