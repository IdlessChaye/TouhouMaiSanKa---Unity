using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class BacklogRenderManager : BaseRenderManager {
        public GameObject voice2;
        public UILabel name2;
        public UILabel context2;
        public GameObject voice1;
        public UILabel name1;
        public UILabel context1;
        public GameObject voice0;
        public UILabel name0;
        public UILabel context0;

        private BacklogManager backlogManager;
        private int count;
        private int head;
        private List<BacklogItem> backlogItemList = new List<BacklogItem>();

        protected override void Initilize() {
            backlogManager = PachiGrimoire.I.BacklogManager;
            ResetBacklogUI();
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
            BacklogScrollDown(1);
        }

        protected override void OnMouseScrollWheelZoomIn() {
            BacklogScrollUp(1);
        }

        protected override void OnKeyConfirmDown() {

        }

        #endregion



        public void BacklogScrollUp(int amount) {
            if (amount <= 0) {
                throw new System.Exception("BacklogRenderManager BacklogScrollUp");
            }

            int index = amount + head;
            if (index > count - 3)
                head = count - 3 <= 0 ? 0 : count - 3;
            else
                head = index;
            LoadData();
        }

        public void BacklogScrollDown(int amount) {
            if (amount <= 0) {
                throw new System.Exception("BacklogRenderManager BacklogScrollDown");
            }

            int index = head - amount;
            head = index <= 0 ? 0 : index;
            LoadData();
        }


        protected override void LoadData() {
            name2.text = "";
            name1.text = "";
            name0.text = "";
            context2.text = "";
            context1.text = "";
            context0.text = "";
            voice2.SetActive(false);
            voice1.SetActive(false);
            voice0.SetActive(false);
            count = backlogManager.Count;
            backlogItemList.Clear();
            int index = head;
            string name = null, contextIndex = null, viIndex = null;
            string context = null;
            BacklogItem item = null;

            if (index >= count) {
                voice0.SetActive(false);
                name0.text = "";
                context0.text = "";
                return;
            }
            item = backlogManager.Seek(index);
            backlogItemList.Add(item);
            name = item.name;
            viIndex = item.voiceIndex;
            contextIndex = item.contextIndex;
            if (!string.IsNullOrEmpty(contextIndex))
                context = resourceManager.Get<string>(contextIndex);
            else
                context = "";
            if (string.IsNullOrEmpty(viIndex))
                voice0.SetActive(false);
            else
                voice0.SetActive(true);
            name0.text = name == null || name.Equals(constData.ChoiceBacklogItemName) ? "" : name;
            if (name != null && name.Equals(constData.ChoiceBacklogItemName)) {
                context0.text = "      > " + context + " <";
            } else {
                context0.text = context;
            }

            index++;
            if (index >= count) {
                voice1.SetActive(false);
                name1.text = "";
                context1.text = "";
                return;
            }
            item = backlogManager.Seek(index);
            backlogItemList.Add(item);
            name = item.name;
            viIndex = item.voiceIndex;
            contextIndex = item.contextIndex;
            if (!string.IsNullOrEmpty(contextIndex))
                context = resourceManager.Get<string>(contextIndex);
            else
                context = "";
            if (string.IsNullOrEmpty(viIndex))
                voice1.SetActive(false);
            else
                voice1.SetActive(true);
            name1.text = name == null || name.Equals(constData.ChoiceBacklogItemName) ? "" : name;
            if (name != null && name.Equals(constData.ChoiceBacklogItemName)) {
                context1.text = "      > " + context + " <";
            } else {
                context1.text = context;
            }

            index++;
            if (index >= count) {
                voice2.SetActive(false);
                name2.text = "";
                context2.text = "";
                return;
            }
            item = backlogManager.Seek(index);
            backlogItemList.Add(item);
            name = item.name;
            viIndex = item.voiceIndex;
            contextIndex = item.contextIndex;
            if (!string.IsNullOrEmpty(contextIndex))
                context = resourceManager.Get<string>(contextIndex);
            else
                context = "";
            if (string.IsNullOrEmpty(viIndex))
                voice2.SetActive(false);
            else
                voice2.SetActive(true);
            name2.text = name == null || name.Equals(constData.ChoiceBacklogItemName) ? "" : name;
            if (name != null && name.Equals(constData.ChoiceBacklogItemName)) {
                context2.text = "      > " + context + " <";
            } else {
                context2.text = context;
            }
        }
        protected override void UnloadData() {
            backlogItemList.Clear();
            ResetBacklogUI();
            head = 0;
        }
        protected override void DoOnOtherShow() {
            throw new System.NotImplementedException();
        }
        protected override void DoOnOtherHide() {
            throw new System.NotImplementedException();
        }




        private void ResetBacklogUI() {
            name2.text = "";
            name1.text = "";
            name0.text = "";
            context2.text = "";
            context1.text = "";
            context0.text = "";
            voice2.SetActive(false);
            voice1.SetActive(false);
            voice0.SetActive(false);
        }




        #region Callbacks

        public void BacklogScrollUp1() {
            BacklogScrollUp(1);
        }
        public void BacklogScrollUp3() {
            BacklogScrollUp(3);
        }
        public void BacklogScrollDown1() {
            BacklogScrollDown(1);
        }
        public void BacklogScrollDown3() {
            BacklogScrollDown(3);
        }
        public void VoicePlay2() {
            int count = backlogItemList.Count;
            if (count < 3)
                return;
            BacklogItem item = backlogItemList[2];
            string characterName = item.name;
            string index = item.voiceIndex;
            if(!string.IsNullOrEmpty(index)) { 
                AudioClip clip = resourceManager.Get<AudioClip>(index);
                musicManager.VoicePlay(characterName, clip, index);
            }
        }
        public void VoicePlay1() {
            int count = backlogItemList.Count;
            if (count < 2)
                return;
            BacklogItem item = backlogItemList[1];
            string characterName = item.name;
            string index = item.voiceIndex;
            if (!string.IsNullOrEmpty(index)) {
                AudioClip clip = resourceManager.Get<AudioClip>(index);
                musicManager.VoicePlay(characterName, clip, index);
            }
        }
        public void VoicePlay0() {
            int count = backlogItemList.Count;
            if (count < 1)
                return;
            BacklogItem item = backlogItemList[0];
            string characterName = item.name;
            string index = item.voiceIndex;
            if (!string.IsNullOrEmpty(index)) {
                AudioClip clip = resourceManager.Get<AudioClip>(index);
                musicManager.VoicePlay(characterName, clip, index);
            }
        }

        #endregion

    }
}