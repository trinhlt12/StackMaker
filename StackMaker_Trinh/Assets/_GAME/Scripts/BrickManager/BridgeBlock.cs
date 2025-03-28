namespace _GAME.Scripts.BrickManager
{
    using System;
    using UnityEngine;

    public class BridgeBlock : MonoBehaviour
    {
        private GameObject bridgeBlock;
        private bool       _hasBrick = false;

        public bool HasBrick => _hasBrick;

        public void SetHasBrick()
        {
            _hasBrick = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            if(this._hasBrick) return;

            if (other.CompareTag("Player"))
            {
                bridgeBlock = this.gameObject;
                var player = other.gameObject.GetComponent<Player>();
                player.RemoveBrick(bridgeBlock);
            }
        }
    }
}