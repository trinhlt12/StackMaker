namespace _GAME.Scripts.BrickManager
{
    using System;
    using UnityEngine;

    public class BridgeBlock : MonoBehaviour
    {
        private GameObject bridgeBlock;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                bridgeBlock = this.gameObject;
                var player = other.gameObject.GetComponent<Player>();
                player.RemoveBrick(bridgeBlock);
            }
        }
    }
}