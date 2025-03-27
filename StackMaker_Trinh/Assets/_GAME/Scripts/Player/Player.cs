namespace _GAME.Scripts
{
    using System;
    using System.Collections.Generic;
    using _GAME.Scripts.FSM;
    using UnityEngine;

    public class Player : MonoBehaviour
    {
        [SerializeField] PlayerBlackboard playerBB;

        private readonly Stack<GameObject> brickStack = new Stack<GameObject>();
        private          float             brickHeight;
        private ObjectPool brickVisualPool;

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

        public void PickUpBrick(GameObject brick)
        {
            /*var newBrick = this.brickVisualPool.Get();
            newBrick.transform.SetParent(this.playerBB.brickStackRoot);
            newBrick.transform.localPosition = Vector3.up * this.brickHeight;
            newBrick.transform.localRotation = Quaternion.identity;
            this.brickStack.Push(newBrick);*/
            brick.transform.SetParent(this.playerBB.brickStackRoot);

            this.UpdatePlayerVisualHeight();

        }

        private void RemoveBrick()
        {

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