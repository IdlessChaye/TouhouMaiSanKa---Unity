using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ConfigCharacterVolumeRenderManager : MonoBehaviour {

        public GameObject configCharacterRoot;
        // To do something

        private bool isConfigCharacterShow;
        private Sequence sequence;
        private UIPanel panel;
        private ConfigManager configManager;
        private Config config;
        private ConstData constData;
        private ConfigRenderManager configRenderManager;
        private bool isWorking;


        private void Awake() {
            configCharacterRoot.SetActive(false);
            panel = configCharacterRoot.GetComponent<UIPanel>();
            configManager = PachiGrimoire.I.ConfigManager;
            config = configManager.Config;
            constData = PachiGrimoire.I.constData;
            configRenderManager = GetComponent<ConfigRenderManager>();
            
        }

        #region Input

        private void FixedUpdate() {
            if (!isConfigCharacterShow)
                return;
            // interrupt animation
            if (sequence.IsPlaying() == true && Input.GetMouseButtonDown(0)) {
                CompleteAnimate();
            }
            if (!isWorking)
                return;
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

        private void OnMouseLeftDown() {

        }

        private void OnMouseRightDown() {
            ConfigCharacterHide();
        }
        private void OnMouseScrollWheelZoomOut() {
        }

        private void OnMouseScrollWheelZoomIn() {
        }

        private void OnKeyConfirmDown() {

        }

        #endregion


        private void JoinTween(Tweener tweener) {
            if (tweener == null) {
                Debug.LogWarning("ConfigCharacterVolumeRenderManager JoinTween");
                return;
            }
            if (sequence == null || sequence.IsPlaying() == false) {
                sequence = DOTween.Sequence();
            }
            sequence.Join(tweener);
        }

        public void CompleteAnimate() {
            sequence.Complete();
        }

        private Tweener DoPanelAlpha(UIPanel uiPanel, float fromValue, float toValue, float duration = 0.5f) {
            if (uiPanel == null) {
                return null;
            }
            uiPanel.alpha = fromValue;
            float value = fromValue;
            Tweener tweener = DOTween.To(() => value, (x) => value = x, toValue, duration)
                .OnUpdate(() => uiPanel.alpha = value);
            return tweener;
        }


        public void ConfigCharacterShow() {
            isConfigCharacterShow = true;
            isWorking = false;
            configCharacterRoot.SetActive(true);
            panel.alpha = 0f;
            // 初始化Config
            LoadConfigCharacterData();
            Tweener tweener = DoPanelAlpha(panel, 0f, 1f);
            JoinTween(tweener);

            sequence.OnComplete(() => isWorking = true);
        }

        public void ConfigCharacterHide() {
            configManager.SaveConfigContext();
            isWorking = false;
            panel.alpha = 1f;
            Tweener tweener = DoPanelAlpha(panel, 1f, 0f);
            JoinTween(tweener);

            sequence.OnComplete(() => {
                isConfigCharacterShow = false;
                configCharacterRoot.SetActive(false);
                configRenderManager.ConfigCharacterHide();
            });
        }


        private void LoadConfigCharacterData() {
            // To do something
        }


        private void ConfigCharacterSetDefault() {
            config.VoiceVolumeValueList = new List<float>(constData.DefaultVoiceVolumeValueList);
            LoadConfigCharacterData();
        }




        #region Callbacks

        public void OnPressedButtonExit() {
            ConfigCharacterHide();
        }

        public void OnPressedButtonConfigSetDefault() {
            ConfigCharacterSetDefault();
        }
        #endregion
    }
}