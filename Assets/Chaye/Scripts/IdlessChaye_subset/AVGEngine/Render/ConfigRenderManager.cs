using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ConfigRenderManager : MonoBehaviour {

        public GameObject configRoot;
        public UISlider sliderSystemVolume;
        public UISlider sliderBGMVolume;
        public UISlider sliderSEVolume;
        public UISlider sliderMessageSpeed;
        public UISlider sliderAutoMessageSpeed;
        public UIToggle toggleReadSkip;
        public UIToggle toggleAllSkip;
        public UISlider sliderVoiceVolume;
        public UIButton buttonVoiceVolumeList;
        public UIToggle toggleVoiceChangeLineOn;
        public UIToggle toggleVoiceChangeLineOff;
        public UIToggle toggleHasEffectOn;
        public UIToggle toggleHasEffectOff;
        public UISlider sliderAlphaOfConsole;

        private bool isConfigShow;
        private Sequence sequence;
        private UIPanel panel;
        private ConfigManager configManager;
        private Config config;
        private ConstData constData;
        private StageRenderManager renderManager;
        private ConfigCharacterVolumeRenderManager configCharacterVolumeRenderManager;
        private bool isWorking;
        private bool isConfigCharacterShow;


        private void Awake() {
            panel = configRoot.GetComponent<UIPanel>();
            configManager = PachiGrimoire.I.ConfigManager;
            config = configManager.Config;
            constData = PachiGrimoire.I.constData;
            renderManager = GetComponent<StageRenderManager>();
            configCharacterVolumeRenderManager = GetComponent<ConfigCharacterVolumeRenderManager>();
            configRoot.SetActive(false);
        }

        #region Input

        private void FixedUpdate() {
            if (!isConfigShow)
                return;
            if (isConfigCharacterShow)
                return;
            if (!config.HasAnimationEffect) {
                if (sequence.IsPlaying() == true) {
                    CompleteAnimate();
                }
            }
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
            ConfigHide();
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
                Debug.LogWarning("ConfigRenderManager JoinTween");
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


        public void ConfigShow() {
            isConfigShow = true;
            isWorking = false;
            configRoot.SetActive(true);
            panel.alpha = 0f;
            // 初始化Config
            LoadConfigData();
            Tweener tweener = DoPanelAlpha(panel, 0f, 1f);
            JoinTween(tweener);

            sequence.OnComplete(() => isWorking = true);
        }

        public void ConfigHide() {
            configManager.SaveConfigContext();
            isWorking = false;
            panel.alpha = 1f;
            Tweener tweener = DoPanelAlpha(panel, 1f, 0f);
            JoinTween(tweener);

            sequence.OnComplete(() => {
                configRoot.SetActive(false);
                isConfigShow = false;
                renderManager.ConfigHide();
            });
        }

        private void LoadConfigData() {
            sliderSystemVolume.value = config.SystemVolume;
            sliderBGMVolume.value = config.BGMVolume;
            sliderSEVolume.value = config.SEVolume;
            sliderMessageSpeed.value = config.MessageSpeed;
            sliderAutoMessageSpeed.value = config.AutoMessageSpeed;
            if (config.IsReadSkipOrAllSkipNot) {
                toggleReadSkip.value = true;
            } else {
                toggleAllSkip.value = true;
            }
            sliderVoiceVolume.value = config.VoiceVolume;
            if (config.IsPlayingVoiceAfterChangeLine) {
                toggleHasEffectOn.value = true;
            } else {
                toggleHasEffectOff.value = true;
            }
            if (config.HasAnimationEffect) {
                toggleHasEffectOn.value = true;
            } else {
                toggleHasEffectOff.value = true;
            }
            sliderAlphaOfConsole.value = config.AlphaOfConsole;
        }


        private void ConfigSetDefault() {
            config.SystemVolume = constData.DefaultSystemVolume;
            config.BGMVolume = constData.DefaultBGMVolume;
            config.SEVolume = constData.DefaultSEVolume;
            config.MessageSpeed = constData.DefaultMessageSpeed;
            config.AutoMessageSpeed = constData.DefaultAutoMessageSpeed;
            config.IsReadSkipOrAllSkipNot = constData.DefaultIsPlayingVoiceAfterChangeLine;
            config.VoiceVolume = constData.DefaultVoiceVolume;
            config.IsPlayingVoiceAfterChangeLine = constData.DefaultIsPlayingVoiceAfterChangeLine;
            config.HasAnimationEffect = constData.DefaultHasAnimationEffect;
            config.AlphaOfConsole = constData.DefaultAlphaOfConsole;
            LoadConfigData();
            configManager.SaveConfigContext();
        }


        #region Callbacks

        public void OnValueChangedSliderSystemVolume() {
            config.SystemVolume = sliderSystemVolume.value;
        }
        public void OnValueChangedSliderBGMVolume() {
            config.BGMVolume = sliderBGMVolume.value;
        }
        public void OnValueChangedSliderSEVolume() {
            config.SEVolume = sliderSEVolume.value;
        }
        public void OnValueChangedSliderMessageSpeed() {
            config.MessageSpeed = sliderMessageSpeed.value;
        }
        public void OnValueChangedSliderAutoMessageSpeed() {
            config.AutoMessageSpeed = sliderAutoMessageSpeed.value;
        }
        public void OnValueChangedSliderVoiceVolume() {
            config.VoiceVolume = sliderVoiceVolume.value;
        }
        public void OnValueChangedSliderAlphaOfConsole() {
            config.AlphaOfConsole = sliderAlphaOfConsole.value;
            PachiGrimoire.I.StageRenderManager.SetAlphaOfConsole(); 
        }


        public void OnPressedToggleReadSkip() {
            config.IsReadSkipOrAllSkipNot = true;
        }
        public void OnPressedToggleAllSkip() {
            config.IsReadSkipOrAllSkipNot = false;
        }
        public void OnPressedToggleVoiceChangeLineOn() {
            config.IsPlayingVoiceAfterChangeLine = true;
        }
        public void OnPressedToggleVoiceChangeLineOff() {
            config.IsPlayingVoiceAfterChangeLine = false;
        }
        public void OnPressedToggleHasEffectOn() {
            config.HasAnimationEffect = true;
        }
        public void OnPressedToggleHasEffectOff() {
            config.HasAnimationEffect = false;
        }

        public void OnPressedButtonExit() {
            ConfigHide();
        }

        public void OnPressedButtonSetDefault() {
            ConfigSetDefault();
        }

        public void OnPressedButtonVoiceVolumeList() {
            ConfigCharacterShow();
        }
        #endregion


        #region CharacterVoiceConfig
        public void ConfigCharacterShow() {
            configManager.SaveConfigContext();
            isConfigCharacterShow = true;
            configCharacterVolumeRenderManager.ConfigCharacterShow();
        }

        public void ConfigCharacterHide() {
            isConfigCharacterShow = false;
        }
        #endregion

    }
}