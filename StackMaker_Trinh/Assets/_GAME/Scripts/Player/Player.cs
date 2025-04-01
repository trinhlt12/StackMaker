namespace _GAME.Scripts
{
    using System;
    using System.Collections.Generic;
    using _GAME.Scripts.BrickManager;
    using _GAME.Scripts.FSM;
    using _GAME.Scripts.GameManager;
    using _GAME.Scripts.GameManager.Audio;
    using DG.Tweening;
    using UnityEngine;

    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerBlackboard playerBB;
        [SerializeField] private AudioEventChannelSO sfxChannel;

        private readonly Stack<GameObject> brickStack = new Stack<GameObject>();
        private          float             brickHeight;

        #region UNITY CALLBACKS

        private void Awake()
        {
            this.OnInit();
        }

        private void OnEnable()
        {
            GameEvent.OnPlayerWin += HandleWin;
        }

        private void OnDisable()
        {
            GameEvent.OnPlayerWin -= HandleWin;
        }

        #endregion


        public void OnInit()
        {
            ClearBricks();
        }

        public void PickUpBrick(BrickBlock brick)
        {
            this.sfxChannel.Raise(SFXType.Pickup);

            this.brickHeight = brick.BrickHeight;

            this.brickStack.Push(brick.gameObject);

            brick.transform.SetParent(this.playerBB.brickStackRoot);

            brick.gameObject.transform.localPosition = Vector3.zero;
            brick.gameObject.transform.localRotation = Quaternion.identity;

            var cube = brick.transform.GetChild(0).gameObject;
            var yOffset = this.brickHeight * this.brickStack.Count;

            //update brick visual height:
            UpdateBrickVisualHeight(cube, yOffset);

            this.UpdatePlayerVisualHeight();

        }

        private void UpdateBrickVisualHeight(GameObject brickVisual, float offset)
        {
            brickVisual.transform.DOKill();
            float targetY = offset - this.brickHeight;
            brickVisual.transform.DOLocalMoveY(offset - this.brickHeight, 0.25f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(UpdateType.Fixed)
                .OnComplete(() =>
                {
                    var finalPos = brickVisual.transform.localPosition;
                    finalPos.y                          = targetY;
                    brickVisual.transform.localPosition = finalPos;
                });
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
        }

        private void PlaceBrick(GameObject topBrick, GameObject bridgeBlock)
        {
            this.sfxChannel.Raise(SFXType.Place);

            var brick       = topBrick.GetComponent<BrickBlock>();

            ResetBrickVisual(topBrick);

            brick.transform.SetParent(null);

            var bridgeBlockHeight = bridgeBlock.GetComponent<BoxCollider>().bounds.size.y;
            var spawnPos = bridgeBlock.transform.position + Vector3.up * bridgeBlockHeight/2;

            topBrick.gameObject.transform.position = spawnPos;

            var brickCollider = topBrick.GetComponent<BoxCollider>();
            if (brickCollider != null)
            {
                brickCollider.enabled = false;
            }

            var bridge = bridgeBlock.GetComponent<BridgeBlock>();
            if (bridge != null)
            {
                bridge.SetHasBrick(topBrick);
            }

        }

        private void ResetBrickVisual(GameObject brick)
        {
            var visual = brick.transform.GetChild(0);
            visual.DOKill();
            visual.localPosition = Vector3.zero;
        }


        public void ClearBricks()
        {
            while (this.brickStack.Count > 0)
            {
                var brick = this.brickStack.Pop();
                BlockManager.Instance.brickObjectPool.Return(brick);
            }

            this.UpdatePlayerVisualHeight();
        }

        private void UpdatePlayerVisualHeight()
        {
            var newY = this.brickStack.Count * this.brickHeight;

            this.playerBB.playerVisual.DOKill();
            this.playerBB.playerVisual.DOLocalMoveY(newY, 0.25f)
                .SetEase(ease: Ease.OutQuad)
                .SetUpdate(UpdateType.Fixed);


        }

        public bool HasBrick()
        {
            return this.brickStack.Count > 0;
        }

        private void HandleWin()
        {
            this.playerBB.playerStateMachine.ChangeToWinState();
        }
    }
}