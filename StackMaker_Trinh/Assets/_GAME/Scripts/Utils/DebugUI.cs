using System;
using System.Collections;
using System.Collections.Generic;
using _GAME.Scripts.FSM;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine sm;
    [SerializeField] private TextMeshProUGUI    text;
    [SerializeField] private PlayerBlackboard blackboard;
    private void Awake()
    {
        if(sm == null) return;
    }

    private void Update()
    {
        this.text.text = this.sm._stateMachine.currentState.GetType().Name;
    }

    /*private void OnDrawGizmos()
    {
        if (this.blackboard == null)
            return;

        var playerPosition  = this.blackboard.moveDirection;
        var raycastHeight   = 0.2f;
        var raycastDistance = 1f;
        var groundLayer     = this.blackboard.groundLayer;

        for (var i = 1; i <= 50; i++)
        {
            var checkPosition = playerPosition  + this.blackboard.moveDirection * (i * 1) + Vector3.up * raycastHeight;
            var rayOrigin     = checkPosition + Vector3.up * raycastHeight;

            Gizmos.color = Color.green;
            Gizmos.DrawRay(rayOrigin, Vector3.down * raycastDistance);
            var isGroundBelow = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, groundLayer);
            if (!isGroundBelow)
            {
                Gizmos.color = Color.red;
            }
        }

        Gizmos.color = Color.blue;
    }*/
}