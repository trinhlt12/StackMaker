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
}