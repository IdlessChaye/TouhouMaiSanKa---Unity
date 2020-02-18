using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public abstract class BaseRenderManager : MonoBehaviour {
        public GameObject root;

        protected PachiGrimoire pachiGrimoire;
        protected ConstData constData;
        protected Config config;
        protected StateMachineManager stateMachine;
        protected ResourceManager resourceManager;
        protected MusicManager musicManager;
        protected StageRenderManager renderManager;
        protected ConfigManager configManager;

        protected UIPanel panel;
        protected bool isShow;
        protected bool isWorking;
        protected bool isOtherShow;
        protected BaseRenderManager fromRenderManager;

        protected Sequence sequence;
        protected System.Action action;

        private void Awake() {
            pachiGrimoire = PachiGrimoire.I;
            constData = pachiGrimoire.constData;
            config = pachiGrimoire.ConfigManager.Config;
            stateMachine = pachiGrimoire.StateMachine;
            resourceManager = pachiGrimoire.ResourceManager;
            musicManager = pachiGrimoire.MusicManager;
            renderManager = pachiGrimoire.StageRenderManager;
            configManager = pachiGrimoire.ConfigManager;

            panel = root.GetComponent<UIPanel>();
            root.SetActive(false);

            Initilize();
        }

        protected abstract void Initilize();


        #region Update
        private void FixedUpdate() {
            if (!isShow) {
                return;
            }
            if (isOtherShow)
                return;

            if (!config.HasAnimationEffect) {
                if (sequence.IsPlaying() == true) {
                    CompleteAnimate();
                }
            }
            if (sequence.IsPlaying() == true && Input.GetMouseButtonDown(0)) {
                CompleteAnimate();
            }

            if (!isWorking)
                return;

            ThenUpdateWhat();
        }


        protected abstract void ThenUpdateWhat();

        protected void UpdateInput() {
            if (Input.GetMouseButtonDown(0)) {
                OnMouseLeftDown();
            } else if (Input.GetMouseButtonDown(1)) {
                OnMouseRightDown();
            } else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                OnMouseScrollWheelZoomOut();
            } else if (Input.GetAxis("Mouse ScrollWheel") > 0) {
                OnMouseScrollWheelZoomIn();
            } else if (Input.GetKeyDown(constData.KeyConfirm)) {
                OnKeyConfirmDown();
            }
        }

        protected abstract void OnMouseLeftDown();
        protected abstract void OnMouseRightDown();
        protected abstract void OnMouseScrollWheelZoomOut();
        protected abstract void OnMouseScrollWheelZoomIn();
        protected abstract void OnKeyConfirmDown();
        #endregion


        #region Tween
        protected void JoinTween(Tweener tweener) {
            if (tweener == null) {
                Debug.LogWarning("JoinTween null.");
                return;
            }
            if (sequence == null || sequence.IsPlaying() == false) {
                sequence = DOTween.Sequence();
                action = null;
            }
            sequence.Join(tweener);
        }


        protected void CompleteAnimate() {
            sequence.Complete();
        }

        protected Tweener DoPanelAlpha(UIPanel uiPanel, float fromValue, float toValue, float duration = 0.5f) {
            if (uiPanel == null) {
                return null;
            }
            uiPanel.alpha = fromValue;
            float value = fromValue;
            Tweener tweener = DOTween.To(() => value, (x) => value = x, toValue, duration)
                .OnUpdate(() => uiPanel.alpha = value);
            return tweener;
        }

        protected void PauseAnimate() {
            sequence?.Pause();
        }

        protected void PlayAnimate() {
            sequence?.Play();
        }
        #endregion


        #region RenderSwitch
        public void OnShow(BaseRenderManager fromRenderManager) {
            if(fromRenderManager == null) {
                Debug.LogWarning("ShowThis from null.");
            } else {
                this.fromRenderManager = fromRenderManager;
            }
            isShow = true;
            isWorking = false;
            root.SetActive(true);

            LoadData();

            panel.alpha = 0f;
            Tweener tweener = DoPanelAlpha(panel, 0f, 1f);
            JoinTween(tweener);
            sequence.OnComplete(() => action.Invoke());
            action += () => isWorking = true;
        }

        protected abstract void LoadData();

        public void OnHide() {
            isWorking = false;
            panel.alpha = 1f;
            Tweener tweener = DoPanelAlpha(panel, 1f, 0f);
            JoinTween(tweener);

            sequence.OnComplete(() => action.Invoke());
            action += () => {
                isShow = false;
                UnloadData();
                root.SetActive(false);
                fromRenderManager?.OnOtherHide();
            };
        }

        protected abstract void UnloadData();

        public void OnOtherShow(BaseRenderManager whoShow) {
            DoOnOtherShow();
            isOtherShow = true;
            whoShow.OnShow(this);
        }
        protected abstract void DoOnOtherShow();

        public void OnOtherHide() {
            isOtherShow = false;
            DoOnOtherHide();
        }
        protected abstract void DoOnOtherHide();

        #endregion
    }
}