namespace _GAME.Scripts.FSM
{
    using System;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class PlayerBlackboard : MonoBehaviour
    {
        public Player             player;
        public PlayerStateMachine playerStateMachine;
        public Transform          playerVisual;
        public Transform          brickStackRoot;
        public float              moveSpeed = 5f;
        public LayerMask          groundLayer;
        public LayerMask          bridgeLayer;
        public Animator             animator;

        public SwipeDirection currentSwipeDirection =>
            InputManager.Instance.CanAcceptInput ? InputManager.Instance.CurrentSwipeDirection : SwipeDirection.None;

        public void ResetSwipe()
        {
            InputManager.Instance.ResetSwipeDirection();
        }
    }
}