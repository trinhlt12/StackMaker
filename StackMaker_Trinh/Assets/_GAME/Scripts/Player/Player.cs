namespace _GAME.Scripts
{
    using System;
    using System.Collections.Generic;
    using _GAME.Scripts.FSM;
    using UnityEngine;

    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerBlackboard playerBB;

        private readonly Stack<GameObject> brickStack = new Stack<GameObject>();
        private          float             brickHeight;

        #region UNITY CALLBACKS

        private void Awake()
        {
            this.OnInit();
        }

        #endregion


        public void OnInit()
        {
            ClearBricks();
        }

        public void PickUpBrick(BrickBlock brick)
        {
            this.brickHeight = brick.BrickHeight;

            this.brickStack.Push(brick.gameObject);

            brick.transform.SetParent(this.playerBB.brickStackRoot);

            brick.gameObject.transform.localPosition = Vector3.zero;
            brick.gameObject.transform.localRotation = Quaternion.identity;

            var cube = brick.transform.GetChild(0).gameObject;
            var yOffset = this.brickHeight * this.brickStack.Count;
            cube.transform.localPosition = new Vector3(0, yOffset - this.brickHeight/2, 0);
            this.UpdatePlayerVisualHeight();

        }

        public void RemoveBrick(GameObject bridgeBlock)
        {
            //if stack is empty, return
            if (this.brickStack.Count == 0)
            {
                return;
            }

            var topBrick = this.brickStack.Pop();

            PlaceBrick(topBrick, bridgeBlock);

            UpdatePlayerVisualHeight();


            Debug.Log(brickStack.Count);
            Debug.Log(bridgeBlock.transform.position);
        }

        private void PlaceBrick(GameObject topBrick, GameObject bridgeBlock)
        {
            var brick = topBrick.GetComponent<BrickBlock>();

            brick.transform.SetParent(null);

            var bridgeBlockHeight = bridgeBlock.GetComponent<BoxCollider>().bounds.size.y;
            var spawnPos = bridgeBlock.transform.position + Vector3.up * bridgeBlockHeight/2;

            topBrick.gameObject.transform.position = spawnPos;
            brick.transform.GetChild(0).transform.localPosition = Vector3.zero;

            //stop collision after placing the brick:
            var brickCollider = topBrick.GetComponent<BoxCollider>();
            if (brickCollider != null)
            {
                brickCollider.enabled = false;
            }
        }

        private void ClearBricks()
        {


        }

        private void UpdatePlayerVisualHeight()
        {
            this.playerBB.playerVisual.localPosition
                = Vector3.up * (this.brickStack.Count * this.brickHeight);

        }
    }
}