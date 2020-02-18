using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class BacklogRenderManager : MonoBehaviour {
        public GameObject backlogRoot;
        public GameObject voice2;
        public UILabel name2;
        public UILabel context2;
        public GameObject voice1;
        public UILabel name1;
        public UILabel context1;
        public GameObject voice0;
        public UILabel name0;
        public UILabel context0;

        private Sequence sequence;
        private UIPanel panel;
        private BacklogManager backlogManager;
        private bool isBacklogShow;
        private ConstData constData;
        private StageRenderManager renderManager;
        private ResourceManager resourceManager;
        private MusicManager musicManager;
        private Config config;

        private bool isWorking;
        private int count;
        private int head;
        private List<BacklogItem> backlogItemList = new List<BacklogItem>();

        private void Awake() {
            backlogManager = PachiGrimoire.I.BacklogManager;
            constData = PachiGrimoire.I.constData;
            resourceManager = PachiGrimoire.I.ResourceManager;
            musicManager = PachiGrimoire.I.MusicManager;
            renderManager = GetComponent<StageRenderManager>();
            config = PachiGrimoire.I.ConfigManager.Config;

            panel = backlogRoot.GetComponent<UIPanel>();
            backlogRoot.SetActive(false);
        }

        #region Input

        private void FixedUpdate() {
            if (!isBacklogShow)
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
            BacklogHide();
        }
        private void OnMouseScrollWheelZoomOut() {
            BacklogScrollDown(1);
        }

        private void OnMouseScrollWheelZoomIn() {
            BacklogScrollUp(1);
        }

        private void OnKeyConfirmDown() {

        }

        #endregion


        private void JoinTween(Tweener tweener) {
            if (tweener == null) {
                Debug.LogWarning("BacklogRenderManager JoinTween");
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


        public void BacklogShow() {
            isBacklogShow = true;
            isWorking = false;
            backlogRoot.SetActive(true);
            panel.alpha = 0f;

            LoadBacklogData();
            // 初始化Backlog
            Tweener tweener = DoPanelAlpha(panel, 0f, 1f);
            JoinTween(tweener);

            sequence.OnComplete(() => isWorking = true);
        }

        public void BacklogHide() {
            isWorking = false;
            panel.alpha = 1f;
            Tweener tweener = DoPanelAlpha(panel, 1f, 0f);
            JoinTween(tweener);

            sequence.OnComplete(() => {
                isBacklogShow = false;
                backlogItemList.Clear();
                backlogRoot.SetActive(false);
                renderManager.BacklogHide();
            });
        }

        private void LoadBacklogData() {
            count = backlogManager.Count;
            head = 0;
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
            context = resourceManager.Get<string>(contextIndex);
            if (viIndex == null)
                voice0.SetActive(false);
            else
                voice0.SetActive(true);
            name0.text = name == null || name.Equals(constData.ChoiceBacklogItemName) ? "" : name;
            if (name != null && name.Equals(constData.ChoiceBacklogItemName)) {
                context0.text = "<color=magenta>" + context + "</color>";
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
            context = resourceManager.Get<string>(contextIndex);
            if (viIndex == null)
                voice1.SetActive(false);
            else
                voice1.SetActive(true);
            name1.text = name == null || name.Equals(constData.ChoiceBacklogItemName) ? "" : name;
            if (name != null && name.Equals(constData.ChoiceBacklogItemName)) {
                context1.text = "<color=magenta>" + context + "</color>";
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
            context = resourceManager.Get<string>(contextIndex);
            if (viIndex == null)
                voice2.SetActive(false);
            else
                voice2.SetActive(true);
            name2.text = name == null || name.Equals(constData.ChoiceBacklogItemName) ? "" : name;
            if (name != null && name.Equals(constData.ChoiceBacklogItemName)) {
                context2.text = "<color=magenta>" + context + "</color>";
            } else {
                context2.text = context;
            }
        }


        public void BacklogScrollUp(int amount) {
            if (amount <= 0) {
                throw new System.Exception("BacklogRenderManager BacklogScrollUp");
            }

            int index = amount + head;
            if (index > count - 3)
                head = count - 3 <= 0 ? 0 : count - 3;
            else
                head = index;
            LoadBacklogData();
        }

        public void BacklogScrollDown(int amount) {
            if (amount <= 0) {
                throw new System.Exception("BacklogRenderManager BacklogScrollDown");
            }

            int index = head - amount;
            head = index <= 0 ? 0 : index;
            LoadBacklogData();
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
            AudioClip clip = resourceManager.Get<AudioClip>(index);
            musicManager.VoicePlay(characterName, clip, index);
        }
        public void VoicePlay1() {
            int count = backlogItemList.Count;
            if (count < 2)
                return;
            BacklogItem item = backlogItemList[1];
            string characterName = item.name;
            string index = item.voiceIndex;
            AudioClip clip = resourceManager.Get<AudioClip>(index);
            musicManager.VoicePlay(characterName, clip, index);
        }
        public void VoicePlay0() {
            int count = backlogItemList.Count;
            if (count < 1)
                return;
            BacklogItem item = backlogItemList[0];
            string characterName = item.name;
            string index = item.voiceIndex;
            AudioClip clip = resourceManager.Get<AudioClip>(index);
            musicManager.VoicePlay(characterName, clip, index);
        }

        #endregion
    }
}