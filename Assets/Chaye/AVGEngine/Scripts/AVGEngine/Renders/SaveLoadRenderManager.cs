using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class SaveLoadRenderManager : BaseRenderManager {

        public GameObject confirmRoot;
        public UISprite confirmSprite;


        private bool isConfirmShow;
        private bool isConfirmWorking;
        private string selectedName;

        protected override void Initilize() {
            confirmRoot.SetActive(false);
        }


        #region Input

        protected override void ThenUpdateWhat() {
            UpdateInput();
        }

        protected override void OnMouseLeftDown() {

        }

        protected override void OnMouseRightDown() {
            if(isWorking) { 
                OnHide();
            } else if(isConfirmWorking){
                OnConfirmNOHide();
            }
        }
        protected override void OnMouseScrollWheelZoomOut() {
        }

        protected override void OnMouseScrollWheelZoomIn() {
        }

        protected override void OnKeyConfirmDown() {

        }

        #endregion




        private Tweener DoSpriteAlpha(UISprite uiPanel, float fromValue, float toValue, float duration = 0.5f) {
            if (uiPanel == null) {
                return null;
            }
            uiPanel.alpha = fromValue;
            float value = fromValue;
            Tweener tweener = DOTween.To(() => value, (x) => value = x, toValue, duration)
                .OnUpdate(() => uiPanel.alpha = value);
            return tweener;
        }



        protected override void LoadData() {
        }
        protected override void UnloadData() {
        }
        protected override void DoOnOtherShow() {
        }
        protected override void DoOnOtherHide() {
        }





        #region Callbacks
        public void OnConfirmShow() {
            isConfirmShow = true;
            isConfirmWorking = false;
            confirmRoot.SetActive(true);
            confirmSprite.alpha = 0f;

            Tweener tweener = DoSpriteAlpha(confirmSprite, 0f, 1f);
            JoinTween(tweener);

            sequence.OnComplete(() => action.Invoke());
            action += () => isConfirmWorking = true;
        }

        public void OnConfirmNOHide() {
            isConfirmWorking = false;
            confirmSprite.alpha = 1f;
            Tweener tweener = DoSpriteAlpha(confirmSprite, 1f, 0f);
            JoinTween(tweener);

            sequence.OnComplete(() => action.Invoke());
            action += () => {
                isConfirmShow = false;
                confirmRoot.SetActive(false);
            };
        }

        public void OnConfirmYESHide() {
            isConfirmWorking = false;
            confirmSprite.alpha = 1f;
            Tweener tweener = DoSpriteAlpha(confirmSprite, 1f, 0f);
            JoinTween(tweener);

            sequence.OnComplete(() => {
                isConfirmShow = false;
                confirmRoot.SetActive(false);

                isWorking = false;
                panel.alpha = 1f;
                sequence = DOTween.Sequence();
                sequence.Join(DoPanelAlpha(panel, 1f, 0f));
                sequence.OnComplete(() => {
                    isShow = false;
                    UnloadData();
                    root.SetActive(false);
                    fromRenderManager?.OnOtherHide();     
                });
            });
        }

        private int GetSelectedNumber() {
            int number = -1;
            number = int.Parse(selectedName);
            return number;
        }
        #endregion

    }
}