namespace _GAME.Scripts.BrickManager
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class BridgeManager : MonoBehaviour
    {
        public static BridgeManager Instance { get; private set; }

        [SerializeField] private List<BridgeBlock> bridgeBlocks = new List<BridgeBlock>();

        private void Awake()
        {
            Instance = this;
        }

        public void AddBridgeBlock(BridgeBlock bridgeBlock)
        {
            if(!bridgeBlocks.Contains(bridgeBlock))
                bridgeBlocks.Add(bridgeBlock);
        }

        public int GetPlayerIndexOnBridge(Vector3 playerPosition)
        {
            Vector2 playerXZ = new Vector2(Mathf.RoundToInt(playerPosition.x), Mathf.RoundToInt(playerPosition.z));

            for (int i = 0; i < bridgeBlocks.Count; i++)
            {
                Vector3 blockPos = bridgeBlocks[i].transform.position;
                Vector2 blockXZ  = new Vector2(Mathf.RoundToInt(blockPos.x), Mathf.RoundToInt(blockPos.z));

                if (playerXZ == blockXZ)
                    return i;
            }

            return -1;
        }


        public void LockRemainingBridgeBlocks(int startIndex)
        {
            for(int i = startIndex + 1; i < this.bridgeBlocks.Count; i++)
            {
                this.bridgeBlocks[i].SetColliderEnabled(false);
            }
        }

        public void UnlockAll()
        {
            foreach (var block in this.bridgeBlocks)
            {
                block.SetColliderEnabled(true);
            }
        }
    }
}