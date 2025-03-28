namespace _GAME.Scripts.BrickManager
{
    using System;
    using UnityEngine;

    public class BridgeBlock : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.gameObject.GetComponent<Player>();
                player.RemoveBrick();
            }

        }
    }
}