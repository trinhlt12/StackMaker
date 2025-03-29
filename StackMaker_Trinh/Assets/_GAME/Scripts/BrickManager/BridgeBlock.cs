namespace _GAME.Scripts.BrickManager
{
    using System;
    using UnityEngine;

    public class BridgeBlock : MonoBehaviour
    {
        private GameObject bridgeBlock;
        private bool       _hasBrick = false;
        private GameObject _currentBrick;

        public bool HasBrick()
        {
            return _currentBrick != null;
        }

        public GameObject GetBrick() => _currentBrick;


        private void Start()
        {
            BridgeManager.Instance.AddBridgeBlock(this);
        }

        public void ClearBrick()
        {
            _currentBrick = null;
        }

        /*public void SetHasBrick()
        {
            _hasBrick = true;
        }*/
        private void OnTriggerEnter(Collider other)
        {
            if(this._hasBrick) return;

            if (other.CompareTag("Player"))
            {
                var player = other.gameObject.GetComponent<Player>();

                if(!player.HasBrick())
                {
                    int currentIndex = BridgeManager.Instance.GetPlayerIndexOnBridge(player.transform.position);
                    Debug.Log(currentIndex);
                    var newTarget = BridgeManager.Instance.GetBridgeBlockAtIndex(currentIndex);
                    if(currentIndex >=0)
                    {
                        BridgeManager.Instance.HandlePlayerOutOfBricks(newTarget);
                    }
                    return;
                }

                bridgeBlock = this.gameObject;


                player.RemoveBrick(bridgeBlock);
            }
        }

        public void SetColliderEnabled(bool enabled)
        {
            var col = GetComponent<BoxCollider>();
            if (col != null)
            {
                col.enabled = enabled;
            }
        }
    }
}