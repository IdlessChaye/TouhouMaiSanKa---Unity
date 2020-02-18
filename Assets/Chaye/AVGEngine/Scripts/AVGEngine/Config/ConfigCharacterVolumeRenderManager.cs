using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ConfigCharacterVolumeRenderManager : BaseRenderManager {

        protected override void Initilize() {
            
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
           
        }
        protected override void UnloadData() {
            configManager.SaveConfigContext();
        }
        protected override void DoOnOtherShow() {
            configManager.SaveConfigContext();
        }
        protected override void DoOnOtherHide() {

        }


        private void ConfigCharacterSetDefault() {
            config.VoiceVolumeValueList = new List<float>(constData.DefaultVoiceVolumeValueList);
            LoadData();
        }




        #region Callbacks

        public void OnPressedButtonExit() {
            OnHide();
        }

        public void OnPressedButtonConfigSetDefault() {
            ConfigCharacterSetDefault();
        }
        #endregion
    }
}