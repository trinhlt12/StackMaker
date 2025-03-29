using System;
using System.Collections;
using System.Collections.Generic;
using _GAME.Scripts.FSM;
using _GAME.Scripts.Level;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine sm;
    [SerializeField] private TextMeshProUGUI    text;
    [SerializeField] private PlayerBlackboard blackboard;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    private void Awake()
    {
        if(sm == null) return;
    }

    private void Update()
    {
        this.text.text = this.sm._stateMachine.currentState.GetType().Name;
        this.ShowCurrentLevel();
    }
    private void ShowCurrentLevel()
    {
        var currentLevel = LevelManager.Instance.GetCurrentLevel();
        this.currentLevelText.text = "Level: " + currentLevel;
    }
}