using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ConfigCharacterVolumeRenderManager : BaseRenderManager {
        public GameObject scrollViewGridRoot;
        public GameObject prefabVoiceCharacterItem;
        public AudioClip[] characterVoiceClipArray;


        private UIGrid uiGrid;
        private List<MyVoiceConfigItem> voiceItemList;
        private bool hasInstantiated;

        protected override void Initilize() {
            var a = voiceItemList; // 为了消警报
            voiceItemList = new List<MyVoiceConfigItem>();
            uiGrid = scrollViewGridRoot.GetComponent<UIGrid>();
            hasInstantiated = false;
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
            if (hasInstantiated == false) {
                hasInstantiated = true;
                int arrayLength = characterVoiceClipArray != null ? characterVoiceClipArray.Length : -1;
                List<string> characterNameList = config.CharacterNameList;
                List<float> characterVolumeList = config.VoiceVolumeValueList;
                for (int i = 0; i < characterNameList.Count; i++) {
                    string name = characterNameList[i];
                    GameObject go = Instantiate(prefabVoiceCharacterItem) as GameObject;
                    go.transform.SetParent(scrollViewGridRoot.transform, false);
                    MyVoiceConfigItem myVoiceConfigItem = go.GetComponent<MyVoiceConfigItem>();
                    myVoiceConfigItem.Index = i;
                    myVoiceConfigItem.CharacterName = name;
                    myVoiceConfigItem.Volume = characterVolumeList[i];
                    if (i < arrayLength && characterVoiceClipArray[i] != null) {
                        myVoiceConfigItem.Clip = characterVoiceClipArray[i];
                    }
                    voiceItemList.Add(myVoiceConfigItem);
                }
                uiGrid.Reposition();
            }
        }
        protected override void UnloadData() {
            musicManager.StopCharacterVoice();
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