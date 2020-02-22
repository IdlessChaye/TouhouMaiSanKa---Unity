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
        public int SelectedNumber => selectedNumber;

        public GameObject confirmRoot;
        public UISprite confirmSprite;
        public UILabel labelInquiry;
        public GameObject goYes;
        public GameObject goNo;

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
            UIEventListener.Get(goYes).onPress = (GameObject go, bool isPress) => {
                if (isPress == false)
                    return;
                if (!isConfirmWorking)
                    return;
                OnConfirmYESHide();
            };
            UIEventListener.Get(goNo).onPress = (GameObject go, bool isPress) => {
                if (isPress == false)
                    return;
                if (!isConfirmWorking)
                    return;
                OnConfirmNOHide();
            };
            confirmRoot.SetActive(false);
            for (int i = 0; i < pageNumberArray.Length; i++) {
                UIEventListener.Get(pageNumberArray[i].gameObject).onClick = (GameObject go) => {
                    string selectedPageNumberName = go.name;
                    PageNumber = int.Parse(selectedPageNumberName);
                };
            }
        }


        #region Input

        protected override void ThenUpdateWhat() {
            if (isConfirmWorking) {
                if (Input.GetMouseButtonDown(1)) {
                    OnConfirmNOHide();
                }
            } else {
                UpdateInput();
            }
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
            if (isSaveMode) {
                spriteSave.enabled = true;
                spriteLoad.enabled = false;
                labelInquiry.text = constData.SaveInquiry;
            } else {
                spriteSave.enabled = false;
                spriteLoad.enabled = true;
                labelInquiry.text = constData.LoadInquiry;
            }
            PageNumber = 1;
        }
        protected override void UnloadData() {
        }
        protected override void DoOnOtherShow() {
        }
        protected override void DoOnOtherHide() {
        }


        private void LoadPageData() {
            Dictionary<int, StoryRecord> storyRecordDict = playerRecordManager.StoryRecordDict;
            int baseNumber = 10 * (_pageNumber - 1);
            for (int i = 0; i < mySaveLoadButtonArray.Length; i++) {
                int selectedNumber = baseNumber + i;
                MySaveLoadButton button = mySaveLoadButtonArray[i];
                GameObject go = button.gameObject;
                string strSelectedNumber = selectedNumber.ToString();
                button.RecordNumber = selectedNumber == 0 ? "Quick" : "NO." + (strSelectedNumber.Length == 1 ? "0" : "") + strSelectedNumber;
                if (storyRecordDict.ContainsKey(selectedNumber)) {
                    BoxCollider boxCollider = go.GetComponent<BoxCollider>();
                    if (selectedNumber == 0 && IsSaveMode == true) {
                        boxCollider.enabled = false;
                        UIEventListener.Get(go).onPress = null;
                    } else {
                        boxCollider.enabled = true;
                        UIEventListener.Get(go).onPress = OnMyPressRecord;
                    }
                    var record = storyRecordDict[selectedNumber];
                    button.Title = record.chapterName;
                    button.Date = record.dateTime;
                    if (string.IsNullOrEmpty(record.dialogContextIndex)) {
                        button.Context = "";
                    } else {
                        string context = resourceManager.Get<string>(record.dialogContextIndex);
                        button.Context = context;
                    }
                } else {
                    BoxCollider boxCollider = go.GetComponent<BoxCollider>();
                    if (IsSaveMode == true) {
                        if (selectedNumber == 0) {
                            boxCollider.enabled = false;
                            UIEventListener.Get(go).onPress = null;
                        } else {
                            boxCollider.enabled = true;
                            UIEventListener.Get(go).onPress = OnMyPressRecord;
                        }
                    } else {
                        boxCollider.enabled = false;
                        UIEventListener.Get(go).onPress = null;
                    }
                    button.ClearLabel();
                }
            }
        }


        #region Callbacks

        private void OnMyPressRecord(GameObject goo, bool isPress) {
            if (isPress == false) {
                return;
            }
            if (!isWorking)
                return;
            string selectedNumberName = goo.name.Substring("Button".Length);
            int selectedNumber = 10 * (_pageNumber - 1) + int.Parse(selectedNumberName) - 1;
            if (selectedNumber == 0 && IsSaveMode == true) {
                Debug.LogError("不能普通保存0号存档，这是快速保存存档");
                return;
            }
            this.selectedNumber = selectedNumber;
            Debug.Log("Record selectedNumber :" + selectedNumber);
            OnConfirmShow();
        }


        public void OnConfirmShow() {
            if (!isWorking)
                return;
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
            if (!isConfirmWorking)
                return;
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
            if (!isConfirmWorking)
                return;
            isConfirmWorking = false;
            confirmSprite.alpha = 1f;
            Tweener tweener = DoSpriteAlpha(confirmSprite, 1f, 0f);
            JoinTween(tweener);

            sequence.OnComplete(() => {
                isConfirmShow = false;
                confirmRoot.SetActive(false);

                isWorking = false;
                panel.alpha = 1f;
                sequence = CreateSequence();
                sequence.Join(DoPanelAlpha(panel, 1f, 0f));
                sequence.OnComplete(() => {
                    isShow = false;
                    UnloadData();
                    root.SetActive(false);
                    fromRenderManager?.OnOtherHide();
                    // ??????horse 默认SaveLoad界面只能从StageRenderManager那里来，如果不是，那一定是从StartPanel来的，这样要进行渲染系统初始化
                    if (fromRenderManager == null && IsSaveMode == false) { // ??? 等着Debug吧
                        stageContextManager.InitializeStory();
                        pachiGrimoire.StageRenderManager.OnShow(null);
                    }
                    stageContextManager.SavePlayerRecord();
                    if (PachiGrimoire.I.SaveLoadRenderManager.IsSaveMode)
                        stageContextManager.SaveStoryRecord(PachiGrimoire.I.SaveLoadRenderManager.SelectedNumber);
                    else
                        stageContextManager.LoadStoryRecord(PachiGrimoire.I.SaveLoadRenderManager.SelectedNumber);
                });
            });
        }

        #endregion

    }
}