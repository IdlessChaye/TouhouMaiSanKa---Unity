using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class SaveLoadRenderManager : BaseRenderManager {

        public bool IsSaveMode {
            get { return isSaveMode; }
            set { isSaveMode = value; }
        }
        public int PageNumber {
            get { return _pageNumber; }
            set {
                _pageNumber = value;
                LoadPageData();
            }
        }

        public GameObject confirmRoot;
        public UISprite confirmSprite;
        public UILabel labelInquiry;

        public UISprite spriteLoad;
        public UISprite spriteSave;
        public MySaveLoadButton[] mySaveLoadButtonArray;
        public GameObject[] pageNumberArray;

        private StageContextManager stageContextManager;
        private PlayerRecordManager playerRecordManager;
        private bool isConfirmShow;
        private bool isConfirmWorking;

        private int selectedNumber;
        private bool isSaveMode;
        private int _pageNumber;

        protected override void Initilize() {
            bool a = isConfirmShow; // 为了消除isConfirmShow没被用的警告
            stageContextManager = pachiGrimoire.StageContextManager;
            playerRecordManager = pachiGrimoire.PlayerRecordManager;
            confirmRoot.SetActive(false);
            for(int i = 0; i < pageNumberArray.Length;i++) {
                UIEventListener.Get(pageNumberArray[i].gameObject).onClick = (GameObject go) => {
                    string selectedPageNumberName = go.name;
                    PageNumber = int.Parse(selectedPageNumberName);
                    Debug.Log(PageNumber);
                };
            }
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
            if (isSaveMode) {
                spriteSave.enabled =true;
                spriteLoad.enabled = false;
                labelInquiry.text = constData.SaveInquiry;
            } else {
                spriteSave.enabled = false;
                spriteLoad.enabled = true;
                labelInquiry.text = constData.LoadInquiry;
            }
            confirmRoot.SetActive(false);
            PageNumber = 1;
        }
        protected override void UnloadData() {
            confirmRoot.SetActive(false);
        }
        protected override void DoOnOtherShow() {
        }
        protected override void DoOnOtherHide() {
        }


        private void LoadPageData() {
            Dictionary<int, StoryRecord> storyRecordDict = playerRecordManager.StoryRecordDict;
            int baseNumber = 10 * (_pageNumber - 1);
            for(int i = 0; i < mySaveLoadButtonArray.Length;i++) {
                int selectedNumber = baseNumber + i;
                MySaveLoadButton button = mySaveLoadButtonArray[i];
                GameObject go = button.gameObject;
                if (storyRecordDict.ContainsKey(selectedNumber)) {
                    BoxCollider boxCollider = go.GetComponent<BoxCollider>();
                    boxCollider.enabled = true;
                    UIEventListener.Get(go).onClick = (GameObject goo) => {
                        string selectedNumberName = goo.name.Substring("Button".Length);
                        this.selectedNumber = 10 * (_pageNumber - 1) + int.Parse(selectedNumberName) - 1;
                        OnConfirmShow();
                    };
                    var record = storyRecordDict[selectedNumber];
                    button.Title = record.chapterName;
                    button.Date = record.dateTime;
                    if (record.dialogContextIndex == null) { 
                        button.Context = "";
                    } else {
                        string context = resourceManager.Get<string>(record.dialogContextIndex);
                        button.Context = context;
                    }
                } else {
                    BoxCollider boxCollider = go.GetComponent<BoxCollider>();
                    boxCollider.enabled = false;
                    UIEventListener.Get(go).onClick = null;
                    button.ClearLabel();
                }
            }
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
                    // ??????horse 默认SaveLoad界面只能从StageRenderManager那里来，如果不是，那一定是从StartPanel来的，这样要进行渲染系统初始化
                    if(fromRenderManager == null) { // ??? 等着Debug吧
                        stageContextManager.InitializeStory();
                        pachiGrimoire.StageRenderManager.OnShow(null);
                    }
                    stageContextManager.SavePlayerRecord();
                    stageContextManager.LoadStoryRecord(selectedNumber);
                });
            });
        }

        #endregion

    }
}