using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ConfigRenderManager : BaseRenderManager {

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

        private ConfigCharacterVolumeRenderManager configCharacterVolumeRenderManager;


        protected override void Initilize() {
            configCharacterVolumeRenderManager = GetComponent<ConfigCharacterVolumeRenderManager>();
        }

        #region Input

        protected override void ThenUpdateWhat() {
            UpdateInput();
        }

        protected override void OnMouseLeftDown() {

        }

        protected override void OnMouseRightDown() {
            OnHide();
        }
        protected override void OnMouseScrollWheelZoomOut() {
        }

        protected override void OnMouseScrollWheelZoomIn() {
        }

        protected override void OnKeyConfirmDown() {

        }

        #endregion




        protected override void LoadData() {
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
                toggleVoiceChangeLineOn.value = true;
            } else {
                toggleVoiceChangeLineOff.value = true;
            }
            if (config.HasAnimationEffect) {
                toggleHasEffectOn.value = true;
            } else {
                toggleHasEffectOff.value = true;
            }
            sliderAlphaOfConsole.value = config.AlphaOfConsole;
        }
        protected override void UnloadData() {
            configManager.SaveConfigContext();
        }
        protected override void DoOnOtherShow() {
            configManager.SaveConfigContext();
        }
        protected override void DoOnOtherHide() {

        }

        private void ConfigSetDefault() {
            configManager.LoadDefaultConfig();
            LoadData();
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
            float value = sliderAlphaOfConsole.value;
            config.AlphaOfConsole = value;
            PachiGrimoire.I.StageRenderManager.SetAlphaOfConsole(value); 
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
            OnHide();
        }

        public void OnPressedButtonSetDefault() {
            ConfigSetDefault();
        }

        public void OnPressedButtonVoiceVolumeList() {
            OnOtherShow(configCharacterVolumeRenderManager);
        }
        #endregion


    }
}