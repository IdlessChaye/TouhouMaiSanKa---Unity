using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public abstract class BaseRenderManager : MonoBehaviour {
        public GameObject root;
        public GameObject eventListener;
        public bool IsShow {
            get { return isShow; }
            set { isShow = value; }
        }
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

        private UIEventListener listener;

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

            listener = UIEventListener.Get(eventListener);
            listener.onPress = OnMyPress;
            listener.onScroll = OnMyScroll;

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
            if (Input.GetKeyDown(constData.KeyConfirm)) {
                OnKeyConfirmDown();
            }
        }

        protected abstract void OnMouseLeftDown();
        protected abstract void OnMouseRightDown();
        protected abstract void OnMouseScrollWheelZoomOut();
        protected abstract void OnMouseScrollWheelZoomIn();
        protected abstract void OnKeyConfirmDown();

        private void OnMyPress(GameObject go, bool isPress) {
            if (!isShow) {
                return;
            }
            if (isOtherShow)
                return;
            if (!isWorking)
                return;
            if (isPress == false) {
                return;
            }
            bool leftMousePress = Input.GetMouseButton(0);
            bool rightMousePress = Input.GetMouseButton(1);
            Debug.Log("leftMouse :" + leftMousePress);
            Debug.Log("rightMouse :" + rightMousePress);
            if (leftMousePress == false && rightMousePress == false) {
                return;
            } else if(leftMousePress == true && rightMousePress == true) {
                Debug.LogWarning("别俩一块按啊");
            } else if(leftMousePress == true && rightMousePress == false) {
                OnMouseLeftDown();
            } else if(leftMousePress == false && rightMousePress == true) {
                OnMouseRightDown();
            }
        }

        private void OnMyScroll(GameObject go,float value) {
            if (!isShow) {
                return;
            }
            if (isOtherShow)
                return;
            if (!isWorking)
                return;
            Debug.Log("Scroll " + value);
            if(value < 0) {
                OnMouseScrollWheelZoomOut();
            } else if(value > 0) {
                Debug.Log("OnMouseScrollWheelZoomIn");
                OnMouseScrollWheelZoomIn();
            } else {
                Debug.LogWarning("OnMyScroll value == 0");
            }
        }

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

        protected Tweener DoPanelAlpha(UIPanel uiPanel, float fromValue, float toValue, float duration = 1f) {
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
            if(isShow == true) { // 不能重复显示
                return;
            }
            if (fromRenderManager == null) {
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
            if(isWorking == false) { // 不能重复隐藏
                return;
            }
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
            if(isOtherShow) {
                return;
            }
            DoOnOtherShow();
            isOtherShow = true;
            whoShow.OnShow(this);
        }
        protected abstract void DoOnOtherShow();

        public void OnOtherHide() {
            if(isOtherShow == false) {
                return;
            }
            isOtherShow = false;
            DoOnOtherHide();
        }
        protected abstract void DoOnOtherHide();

        #endregion
    }
}