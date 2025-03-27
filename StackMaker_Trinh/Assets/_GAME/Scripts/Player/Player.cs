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
        private          ObjectPool        brickVisualPool;
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
            this.brickVisualPool = new ObjectPool(this.playerBB.brickVisualPrefab.transform.GetChild(0).gameObject, 20);
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
            Debug.Log(cube.transform.localPosition);
            this.UpdatePlayerVisualHeight(brick);

        }

        private void RemoveBrick()
        {

        }

        private void ClearBricks()
        {


        }

        private void UpdatePlayerVisualHeight(BrickBlock brick)
        {
            this.playerBB.playerVisual.localPosition
                = Vector3.up * (this.brickStack.Count * brick.BrickHeight);

        }
    }
}