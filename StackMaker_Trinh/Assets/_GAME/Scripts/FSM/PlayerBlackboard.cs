namespace _GAME.Scripts.FSM
{
    using System;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class PlayerBlackboard : MonoBehaviour
    {
        public CharacterController playerController;
        public float moveSpeed = 5f;

        public SwipeDirection currentSwipeDirection => InputManager.Instance.CurrentSwipeDirection;

        public void ResetSwipe()
        {
            InputManager.Instance.ResetSwipeDirection();
        }
    }
}