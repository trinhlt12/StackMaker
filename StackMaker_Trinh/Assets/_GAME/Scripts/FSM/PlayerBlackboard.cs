namespace _GAME.Scripts.FSM
{
    using System;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class PlayerBlackboard : MonoBehaviour
    {
        public Transform           playerVisual;
        public Transform           brickStackRoot;
        public GameObject          brickVisualPrefab;
        public float               moveSpeed = 5f;
        public LayerMask           groundLayer;
        public bool                canMove = true;

        public SwipeDirection currentSwipeDirection =>
            InputManager.Instance.CanAcceptInput ? InputManager.Instance.CurrentSwipeDirection : SwipeDirection.None;

        public void ResetSwipe()
        {
            InputManager.Instance.ResetSwipeDirection();
        }
    }
}