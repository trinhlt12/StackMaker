namespace _GAME.Scripts.BrickManager
{
    using System;
    using System.Collections.Generic;
    using _GAME.Scripts.FSM;
    using UnityEngine;

    public class BridgeManager : MonoBehaviour
    {
        public PlayerStateMachine playerStateMachine;
        public static BridgeManager Instance { get; private set; }

        [SerializeField] private List<BridgeBlock> bridgeBlocks = new List<BridgeBlock>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void Init()
        {
            this.ClearAllBridgeBricks();
            this.bridgeBlocks.Clear();
        }


        public void AddBridgeBlock(BridgeBlock bridgeBlock)
        {
            if (!bridgeBlocks.Contains(bridgeBlock)) bridgeBlocks.Add(bridgeBlock);
        }

        public void SortBridgeBlocks()
        {
            this.bridgeBlocks.RemoveAll(block => block == null);
            bridgeBlocks.Sort((a, b) =>
            {
                Vector3 posA = a.transform.position;
                Vector3 posB = b.transform.position;

                int compareZ = posA.z.CompareTo(posB.z);
                if (compareZ != 0)
                    return compareZ;

                return posA.x.CompareTo(posB.x);
            });
        }

        public void ClearAllBridgeBricks()
        {
            foreach (var bridge in bridgeBlocks)
            {
                if (bridge.HasBrick())
                {
                    var brick = bridge.GetBrick();
                    bridge.ClearBrick();
                    BlockManager.Instance.brickObjectPool.Return(brick);
                }
            }
        }



        public int GetPlayerIndexOnBridge(Vector3 playerPosition)
        {
            var snappedPos = new Vector3(
                Mathf.Floor(playerPosition.x) + 0.5f,
                playerPosition.y,
                Mathf.Floor(playerPosition.z) + 0.5f
            );

            for (int i = 0; i < this.bridgeBlocks.Count; i++)
            {
                var block = this.bridgeBlocks[i];
                if (block == null) continue;

                var blockPosition = block.transform.position;

                if (
                    Mathf.Approximately(snappedPos.x, blockPosition.x) &&
                    Mathf.Approximately(snappedPos.z, blockPosition.z)
                )
                {
                    return i;
                }
            }

            return -1;
        }


        public void HandlePlayerOutOfBricks(GameObject block)
        {
            this.playerStateMachine.UpdateMoveStateTarget(block.transform.position);
        }


        public GameObject GetBridgeBlockAtIndex(int index)
        {
            if (index >= 0 && index < bridgeBlocks.Count)
            {
                return bridgeBlocks[index].gameObject;
            }
            return null;
        }

        public void UnlockAll()
        {
            foreach (var block in this.bridgeBlocks)
            {
                block.SetColliderEnabled(true);
            }
        }

        public bool IsPlayerOnBridge(Vector3 playerPosition)
        {
            return GetPlayerIndexOnBridge(playerPosition) >= 0;
        }
    }
}