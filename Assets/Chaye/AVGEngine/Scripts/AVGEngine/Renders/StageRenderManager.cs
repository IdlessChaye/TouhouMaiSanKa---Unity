using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class StageRenderManager : BaseRenderManager {
        private static StageRenderManager instance;
        public static StageRenderManager I => instance;


        public string DialogContextIndex => dialogContextIndex;
        public string CharacterName => characterName;
        public string BackgroundImageIndex => backgroundImageIndex;
        public string SmallFigureImageIndex => smallFigureImageIndex;
        public Dictionary<string, KeyValuePair<string, UITexture>> FigureImageDict => figureImageDict;
        public List<ChoiceItem> ChoiceItemList => choiceItemList;


        public UITexture consoleBackgroundImageUITexture;
        public GameObject choice0;
        public GameObject choice1;
        public GameObject choice2;
        public GameObject choice3;

        private List<GameObject> choiceList;
        private float messageSpeedLowest;
        private float messageSpeedHighest;


        #region Text
        public Text textContextContainer;
        public Text textNameContainer;
        public UILabel contextLabel;
        public UILabel nameLabel;
        private string dialogContextIndex;
        private string characterName;
        #endregion

        #region BackgroundImage
        public UITexture backgroundUITexture;
        public UITexture backgroundUITextureTop;
        private string backgroundImageIndex;
        #endregion

        #region FigureImage
        private Dictionary<string, KeyValuePair<string, UITexture>> figureImageDict; // 标识符 -> <索引,对象>
        public UITexture smallFigureImageUITexture;
        public UITexture smallFigureImageUITextureTop;
        private string smallFigureImageIndex;
        #endregion

        #region Console
        public GameObject console;
        private bool isConsoleShow;
        #endregion

        #region Choice
        private string choosenDLIndex;
        private List<ChoiceItem> choiceItemList;
        #endregion

        #region OtherRenders
        private BacklogRenderManager backlogRenderManager;
        private ConfigRenderManager configRenderManager;
        private SaveLoadRenderManager slRenderManager;
        #endregion

        protected override void Initilize() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(this);
                return;
            }

            backlogRenderManager = GetComponent<BacklogRenderManager>();
            configRenderManager = GetComponent<ConfigRenderManager>();
            slRenderManager = GetComponent<SaveLoadRenderManager>();

            messageSpeedLowest = constData.MessageSpeedLowest;
            messageSpeedHighest = constData.MessageSpeedHighest;

            InitializeStory();
        }


        #region Input

        protected override void ThenUpdateWhat() {
            BaseState state = stateMachine.CurrentState;
            if (state == SleepState.Instance) {
                return;
            }

            StateBuff stateBuff = stateMachine.StateBuff; // Skip Animation
            if (stateBuff == StateBuff.Skip || stateBuff == StateBuff.Next) {
                if (state == RunAnimateState.Instance) {
                    CompleteAnimate();
                }
            }

            UpdateInput();
        }

        protected override void OnMouseLeftDown() {
            BaseState state = stateMachine.CurrentState;
            StateBuff buff = stateMachine.StateBuff;
            if(buff == StateBuff.Normal) { 
                if (state == RunAnimateState.Instance) {
                    CompleteAnimate();
                } else if (state == RunWaitState.Instance) {
                    stateMachine.TransferStateTo(RunScriptState.Instance);
                }
            } else if(buff == StateBuff.Auto) {
                stateMachine.SetStateBuff(StateBuff.Normal);
            } else if(buff == StateBuff.Skip) {
                stateMachine.SetStateBuff(StateBuff.Normal);
            }
        }
        protected override void OnMouseRightDown() {
            BaseState state = stateMachine.CurrentState;
            StateBuff buff = stateMachine.StateBuff;
            if(buff == StateBuff.Normal) { 
                if (state == RunWaitState.Instance) {
                    if (isConsoleShow) {
                        ConsoleHide();
                    } else {
                        ConsoleShow();
                    }
                } else if (state == ChoiceWaitState.Instance) {
                    if (isConsoleShow) {
                        ConsoleHide();
                    } else {
                        ConsoleShow();
                    }
                }
            } else if(buff == StateBuff.Auto) {
                stateMachine.SetStateBuff(StateBuff.Normal);
            } else if(buff == StateBuff.Skip) {
                stateMachine.SetStateBuff(StateBuff.Normal);
            }
        }
        protected override void OnMouseScrollWheelZoomOut() {
            BaseState state = stateMachine.CurrentState;
            StateBuff buff = stateMachine.StateBuff;
            if (buff == StateBuff.Normal) {
                if (state == RunAnimateState.Instance) {
                    CompleteAnimate();
                } else if(state == RunWaitState.Instance) {
                    stateMachine.TransferStateTo(RunScriptState.Instance);
                }
            } else if(buff == StateBuff.Auto) {
                if (state == RunAnimateState.Instance) {
                    CompleteAnimate();
                }
            }
        }
        protected override void OnMouseScrollWheelZoomIn() {
            BaseState state = stateMachine.CurrentState;
            StateBuff buff = stateMachine.StateBuff;
            if (buff == StateBuff.Normal) {
                if (state == RunAnimateState.Instance || state == RunWaitState.Instance || state == ChoiceWaitState.Instance) {
                    OnOtherShow(backlogRenderManager);
                }
            } else if(buff == StateBuff.Auto) {
                if (state == RunAnimateState.Instance || state == RunWaitState.Instance || state == ChoiceWaitState.Instance) {
                    stateMachine.SetStateBuff(StateBuff.Normal);
                    OnOtherShow(backlogRenderManager);
                }
            } else if(buff==StateBuff.Skip) {
                if (state == RunAnimateState.Instance || state == RunWaitState.Instance || state == ChoiceWaitState.Instance) {
                    stateMachine.SetStateBuff(StateBuff.Normal);
                    OnOtherShow(backlogRenderManager);
                }
            }
        }
        protected override void OnKeyConfirmDown() {
            OnMouseLeftDown();
        }

        #endregion


        #region Tween
        

        private void StateQuitAnimate() {
            sequence?.Kill();
            stateMachine.TransferStateTo(RunScriptState.Instance);
        }

        

        #endregion


        #region Text
        public void TextClear(bool hasEffect = true) {
            textContextContainer.text = "";
            textNameContainer.text = "";
            nameLabel.text = "";
            contextLabel.text = "";
            dialogContextIndex = null;
            characterName = null;

            if(!config.IsPlayingVoiceAfterChangeLine) {
                musicManager.VoiceStop(hasEffect);
            }
        }

        public void TextChange(string dialogContextIndex, string characterName = null, bool hasEffect = true) {
            if (string.IsNullOrEmpty(dialogContextIndex)) {
                throw new System.Exception("StageRenderManager TextChange");
            }
            textContextContainer.text = "";
            textNameContainer.text = "";
            nameLabel.text = "";
            contextLabel.text = "";
            this.dialogContextIndex = dialogContextIndex;
            this.characterName = characterName;

            if (hasEffect) {
                string context = resourceManager.Get<string>(dialogContextIndex);
                float messageSpeed = config.MessageSpeed;
                float textShowTime = dialogContextIndex.Length / (messageSpeedHighest * messageSpeed + messageSpeedLowest);
                float textDelayTime = 0;
                if (characterName != null) {
                    textDelayTime = characterName.Length / (messageSpeedHighest * messageSpeed + messageSpeedLowest);
                    Tweener tweenerName = textNameContainer.DOText(characterName, textDelayTime).
                        OnUpdate(() => nameLabel.text = textNameContainer.text);
                    JoinTween(tweenerName);
                }
                Tweener tweenerContext = textContextContainer.DOText(context, textShowTime)
                    .SetDelay(textDelayTime)
                    .OnUpdate(() => contextLabel.text = textContextContainer.text);
                JoinTween(tweenerContext);

                sequence.OnComplete(() => action.Invoke());
                action -= StateQuitAnimate;
                action += StateQuitAnimate;
            }
        }
        #endregion


        #region BackgroundImage
        public void BackgroundImageChange(string index, bool hasEffect = true) {
            if (string.IsNullOrEmpty(index)) {
                throw new System.Exception("StageRenderManager BackgroundImageChange");
            }
            backgroundImageIndex = index;

            if (hasEffect) {
                Texture2D backgroundTexture2D = resourceManager.Get<Texture2D>(index);
                backgroundUITexture.mainTexture = backgroundTexture2D;
                backgroundUITexture.width = ConstData.TEXTURE2D_WIDTH;
                backgroundUITexture.height = ConstData.TEXTURE2D_HEIGHT;
                backgroundUITexture.alpha = 0f;

                Tweener tweenerDisappear = DoTextureAlpha(backgroundUITextureTop, 1f, 0f);
                JoinTween(tweenerDisappear);
                Tweener tweenerAppear = DoTextureAlpha(backgroundUITexture, 0f, 1f);
                JoinTween(tweenerAppear);

                sequence.OnComplete(() => action.Invoke());
                action += () => {
                    backgroundUITextureTop.mainTexture = backgroundTexture2D;
                    backgroundUITextureTop.width = ConstData.TEXTURE2D_WIDTH;
                    backgroundUITextureTop.height = ConstData.TEXTURE2D_HEIGHT;
                    backgroundUITextureTop.alpha = 1f;
                    backgroundUITexture.mainTexture = null;
                    backgroundUITexture.alpha = 0f;
                };
                action -= StateQuitAnimate;
                action += StateQuitAnimate;
            }
        }

        public void BackgroundImageClear(bool hasEffect = true) {
            backgroundImageIndex = null;

            if (hasEffect) {
                if (backgroundUITextureTop.mainTexture == null) {
                    Debug.LogWarning("StageRenderManager BackgroundImageClear");
                    return;
                }
                Tweener tweener = DoTextureAlpha(backgroundUITextureTop, 1f, 0f);
                JoinTween(tweener);

                sequence.OnComplete(() => action.Invoke());
                action += () => {
                    backgroundUITextureTop.mainTexture = null;
                    backgroundUITextureTop.width = ConstData.TEXTURE2D_SCREEN_WIDTH;
                    backgroundUITextureTop.height = ConstData.TEXTURE2D_SCREEN_HEIGHT;
                    backgroundUITextureTop.alpha = 1f;
                };
                action -= StateQuitAnimate;
                action += StateQuitAnimate;
            }
        }

        private Tweener DoTextureAlpha(UITexture uiTexture, float fromValue, float toValue, float duration = 0.5f) {
            if (uiTexture == null) {
                return null;
            }
            uiTexture.alpha = fromValue;
            float value = fromValue;
            Tweener tweener = DOTween.To(() => value, (x) => value = x, toValue, duration)
                .OnUpdate(() => uiTexture.alpha = value);
            return tweener;
        }
        #endregion


        #region FigureImage
        public void FigureImageAdd(string uiKey, string fiIndex,
            float pos_x, float pos_y,
            float scale_x = 1f, float scale_y = 1f, bool hasEffect = true) {
            if (string.IsNullOrEmpty(uiKey) || string.IsNullOrEmpty(fiIndex) || figureImageDict.ContainsKey(uiKey)) {
                throw new System.Exception("StageRenderManager FigureImageAdd");
            }

            Texture2D texture2D = resourceManager.Get<Texture2D>(fiIndex);
            float width = texture2D.width;
            float height = texture2D.height;

            GameObject go = new GameObject(uiKey);
            UITexture uiTexture = go.AddComponent<UITexture>();
            uiTexture.mainTexture = texture2D;
            uiTexture.width = (int)(width * scale_x);
            uiTexture.height = (int)(height * scale_y);
            go.transform.position = new Vector3(pos_x, pos_y, 0);
            go.transform.SetParent(root.transform, false);
            KeyValuePair<string, UITexture> pair = new KeyValuePair<string, UITexture>(fiIndex, uiTexture);
            figureImageDict.Add(uiKey, pair);

            if (hasEffect) {
                uiTexture.alpha = 0f;
                Tweener tweener = DoTextureAlpha(uiTexture, 0f, 1f);
                JoinTween(tweener);

                sequence.OnComplete(() => action.Invoke());
                action -= StateQuitAnimate;
                action += StateQuitAnimate;
            } else {
                uiTexture.alpha = 1f;
            }
        }

        public void FigureImageRemove(string uiKey, bool hasEffect = true) {
            if (string.IsNullOrEmpty(uiKey) || !figureImageDict.ContainsKey(uiKey)) {
                throw new System.Exception("StageRenderManager FigureImageRemove");
            }

            UITexture uiTexture = figureImageDict[uiKey].Value;
            GameObject go = uiTexture.gameObject;
            figureImageDict.Remove(uiKey);

            if (hasEffect) {
                uiTexture.alpha = 1f;
                Tweener tweener = DoTextureAlpha(uiTexture, 1f, 0f);
                JoinTween(tweener);

                sequence.OnComplete(() => action.Invoke());
                action += () => {
                    uiTexture.mainTexture = null;
                    Destroy(uiTexture);
                    Destroy(go);
                };
                action -= StateQuitAnimate;
                action += StateQuitAnimate;
            } else {
                uiTexture.alpha = 0f;
                uiTexture.mainTexture = null;
                Destroy(uiTexture);
                Destroy(go);
            }

        }

        public void FigureImageClear(bool hasEffect = true) {
            if(hasEffect) {
                foreach(string key in figureImageDict.Keys) {
                    FigureImageRemove(key);
                }
            } else {
                FigureImageDataClear();
            }
        }

        private void FigureImageDataClear() {
            if(figureImageDict == null) {
                figureImageDict = new Dictionary<string, KeyValuePair<string, UITexture>>();
                return;
            }
            foreach (var pair in figureImageDict.Values) {
                UITexture texture = pair.Value;
                GameObject go = texture.gameObject;
                texture.alpha = 0f;
                texture.mainTexture = null;
                Destroy(go);
            }
            figureImageDict.Clear();
        }

        public void SmallFigureImageChange(string index, bool hasEffect = true) {
            if (string.IsNullOrEmpty(index)) {
                throw new System.Exception("StageRenderManager SmallFigureImageChange");
            }
            smallFigureImageIndex = index;

            if (hasEffect) {
                Texture2D smallFigureTexture2D = resourceManager.Get<Texture2D>(index);
                smallFigureImageUITexture.mainTexture = smallFigureTexture2D;
                smallFigureImageUITexture.width = ConstData.TEXTURE2D_SMALL_FIGURE_IMAGE_WIDTH;
                smallFigureImageUITexture.height = ConstData.TEXTURE2D_SMALL_FIGURE_IMAGE_HEIGHT;
                smallFigureImageUITexture.alpha = 0f;

                Tweener tweenerDisappear = DoTextureAlpha(smallFigureImageUITextureTop, 1f, 0f);
                JoinTween(tweenerDisappear);
                Tweener tweenerAppear = DoTextureAlpha(smallFigureImageUITexture, 0f, 1f);
                JoinTween(tweenerAppear);

                sequence.OnComplete(() => action.Invoke());
                action += () => {
                    smallFigureImageUITextureTop.mainTexture = smallFigureTexture2D;
                    smallFigureImageUITextureTop.width = ConstData.TEXTURE2D_SMALL_FIGURE_IMAGE_WIDTH;
                    smallFigureImageUITextureTop.height = ConstData.TEXTURE2D_SMALL_FIGURE_IMAGE_HEIGHT;
                    smallFigureImageUITextureTop.alpha = 1f;
                    smallFigureImageUITexture.mainTexture = null;
                    smallFigureImageUITexture.alpha = 0f;
                };
                action -= StateQuitAnimate;
                action += StateQuitAnimate;
            }
        }


        public void SmallFigureImageClear(bool hasEffect = true) {
            smallFigureImageIndex = null;

            if (hasEffect) {
                if (smallFigureImageUITextureTop.mainTexture == null) {
                    Debug.LogWarning("StageRenderManager SmallFigureImageClear");
                    return;
                }
                Tweener tweener = DoTextureAlpha(smallFigureImageUITextureTop, 1f, 0f);
                JoinTween(tweener);

                sequence.OnComplete(() => action.Invoke());
                action += () => {
                    smallFigureImageUITextureTop.mainTexture = null;
                    smallFigureImageUITextureTop.width = ConstData.TEXTURE2D_WIDTH;
                    smallFigureImageUITextureTop.height = ConstData.TEXTURE2D_HEIGHT;
                    smallFigureImageUITextureTop.alpha = 1f;
                };
                action -= StateQuitAnimate;
                action += StateQuitAnimate;
            }
        }


        #endregion


        #region Console
        public void ConsoleShow(bool hasEffect = true) {
            if (isConsoleShow) {
                return;
            }
            isConsoleShow = true;
            if (hasEffect)
                console.transform.position = Vector3.zero;
        }

        public void ConsoleHide(bool hasEffect = true) {
            if (!isConsoleShow) {
                return;
            }
            isConsoleShow = false;
            if (hasEffect)
                console.transform.position = Vector3.up * -1000;
        }

        public void SetAlphaOfConsole() {
            float alpha = config.AlphaOfConsole;
            consoleBackgroundImageUITexture.alpha = alpha;
        }

        public void ChoiceCreate(List<ChoiceItem> choiceItemList) { // 不管是Run Auto Skip 总有渲染效果
            if (choiceItemList == null || choiceItemList.Count == 0) {
                throw new System.Exception("StageRenderManager ChoiceCreate");
            }
            this.choiceItemList = choiceItemList;

            int count = choiceItemList.Count;
            // 得到各个选项的文本
            // 不能选择的用不能选的图片渲染，能选的检查mark 是否有已经选过的，选过的用别的选项图片渲染
            // 给每个选项添加回调函数 要有mark和script信息
            choiceList = new List<GameObject>();
            choiceList.Add(choice0);
            choiceList.Add(choice1);
            if (count >= 3) {
                choiceList.Add(choice2);
                if (count >= 4) {
                    choiceList.Add(choice3);
                    if (count > 4) {
                        throw new System.Exception("Only 4 choices!");
                    }
                }
            }
            for (int i = 0; i < choiceList.Count; i++) {
                ChoiceItem choiceItem = choiceItemList[i];
                string dlIndex = choiceItem.DLIndex;
                bool canBeSelected = choiceItem.CanBeSelected;
                string choiceContext = resourceManager.Get<string>(dlIndex);
                GameObject choice = choiceList[i];
                choice.SetActive(true);
                if (count <= 3) {
                    if (i == 0)
                        choice.transform.position = new Vector3(constData.ChoicePosX_0of23, constData.ChoicePosY_0of23, 0f);
                    else if (i == 1)
                        choice.transform.position = new Vector3(constData.ChoicePosX_1of23, constData.ChoicePosY_1of23, 0f);
                    else if (i == 2)
                        choice.transform.position = new Vector3(constData.ChoicePosX_2of23, constData.ChoicePosY_2of23, 0f);
                } else if (count == 4) {
                    if (i == 0)
                        choice.transform.position = new Vector3(constData.ChoicePosX_0of4, constData.ChoicePosY_0of4, 0f);
                    else if (i == 1)
                        choice.transform.position = new Vector3(constData.ChoicePosX_1of4, constData.ChoicePosY_1of4, 0f);
                    else if (i == 2)
                        choice.transform.position = new Vector3(constData.ChoicePosX_2of4, constData.ChoicePosY_2of4, 0f);
                    else if (i == 3)
                        choice.transform.position = new Vector3(constData.ChoicePosX_3of4, constData.ChoicePosY_3of4, 0f);
                }
                UIButton uiButton = choice.GetComponent<UIButton>();
                UILabel uiLabel = choice.GetComponentInChildren<UILabel>();
                UIEventListener listener = UIEventListener.Get(choice);

                uiLabel.text = choiceContext; // 文本
                if (canBeSelected) { // 颜色 和 点击事件
                    uiButton.hover = uiButton.pressed;
                    listener.onClick += OnChoiceButtonClick;
                } else {
                    uiButton.hover = uiButton.disabledColor;
                }
            }
        }

        private void OnChoiceButtonClick(GameObject go) { // 不知道怎么编能更好
            string name = go.name;
            int number = int.Parse(name[6].ToString());
            ChoiceItem choiceItem = choiceItemList[number];
            choosenDLIndex = choiceItem.DLIndex;
            string mark = choiceItem.Mark;
            string scriptContext = choiceItem.OnSelectedScirptContext;
            PachiGrimoire.I.MarkManager.MarkStorySet(mark);
            PachiGrimoire.I.MarkManager.MarkPlayerSet(mark);
            PachiGrimoire.I.ScriptManager.LoadScriptContext(scriptContext);

            OnChoiceButtonClickHelper();
        }

        private void OnChoiceButtonClickHelper() {
            for (int i = 0; i < choiceList.Count; i++) { // 还原Choice Gameobjects 状态
                GameObject go = choiceList[i];
                UIButton button = go.GetComponent<UIButton>();
                button.onClick = null;
                UITexture texture = go.GetComponent<UITexture>();
                UILabel label = go.GetComponentInChildren<UILabel>();
                Tweener tweener = DoTextureAlpha(texture, 1f, 0f).OnComplete(() => {
                    button.hover = button.pressed;
                    label.text = "";
                    texture.alpha = 1f;
                    go.SetActive(false);
                });
                JoinTween(tweener);
            }
            BaseState lastState = stateMachine.LastState;

            sequence.OnComplete(()=>action.Invoke());
            action += () => { // 只要做出选择 要处理choiceItemList，处理Backlog ,SetActive，State
                choiceItemList.Clear();
                choiceList.Clear();
                BacklogItem backlogItem = new BacklogItem(null, choosenDLIndex, null, constData.ChoiceBacklogItemName); // 选项的backlog name是 Choice
                PachiGrimoire.I.BacklogManager.Push(backlogItem);
            };
            action -= StateQuitAnimate;
            action += StateQuitAnimate;
        }
        #endregion



        #region RenderSwitch
        protected override void LoadData() {
            SetAlphaOfConsole();
        }

        protected override void UnloadData() {
            
        }

        protected override void DoOnOtherShow() {
            stateMachine.TransferStateTo(SleepState.Instance); // Go to Sleep
            PauseAnimate();
        }

        protected override void DoOnOtherHide() {
            PlayAnimate();
            stateMachine.TransferStateTo(stateMachine.LastState); // Sleep Back
        }
        #endregion





        public void LoadStoryRecord(StoryRecord sr) {
            sequence?.Pause();
            sequence = DOTween.Sequence();

            dialogContextIndex = sr.dialogContextIndex;
            characterName = sr.characterName;
            backgroundImageIndex = sr.backgroundImageIndex;
            smallFigureImageIndex = sr.smallFigureImageIndex;
            List<string> figureImageKeyList = new List<string>(sr.figureImageKeyList);
            List<string> figureImageFIIndexList = new List<string>(sr.figureImageFIIndexList);
            List<KeyValuePair<float, float>> figureImagePosList = new List<KeyValuePair<float, float>>(sr.figureImagePosList);
            List<KeyValuePair<float, float>> figureImageScaleList = new List<KeyValuePair<float, float>>(sr.figureImageScaleList);
            choiceItemList = new List<ChoiceItem>(sr.choiceItemList);

            if (dialogContextIndex != null) {
                string context = resourceManager.Get<string>(dialogContextIndex);
                contextLabel.text = context;
            } else {
                contextLabel.text = "";
            }

            nameLabel.text = characterName == null ? "" : characterName;

            if (backgroundImageIndex != null) {
                Texture2D backgroundTexture2D = resourceManager.Get<Texture2D>(backgroundImageIndex);
                backgroundUITextureTop.mainTexture = backgroundTexture2D;
                backgroundUITextureTop.width = ConstData.TEXTURE2D_WIDTH;
                backgroundUITextureTop.height = ConstData.TEXTURE2D_HEIGHT;
                backgroundUITextureTop.alpha = 1f;
            } else {
                backgroundUITextureTop.mainTexture = null;
            }
            backgroundUITexture.mainTexture = null;
            backgroundUITexture.width = ConstData.TEXTURE2D_WIDTH;
            backgroundUITexture.height = ConstData.TEXTURE2D_HEIGHT;
            backgroundUITexture.alpha = 1f;

            if (smallFigureImageIndex != null) {
                Texture2D smallFigureTexture2D = resourceManager.Get<Texture2D>(smallFigureImageIndex);
                smallFigureImageUITextureTop.mainTexture = smallFigureTexture2D;
                smallFigureImageUITextureTop.width = ConstData.TEXTURE2D_SMALL_FIGURE_IMAGE_WIDTH;
                smallFigureImageUITextureTop.height = ConstData.TEXTURE2D_SMALL_FIGURE_IMAGE_HEIGHT;
                smallFigureImageUITextureTop.alpha = 1f;
            } else {
                smallFigureImageUITextureTop.mainTexture = null;
            }
            smallFigureImageUITexture.mainTexture = null;
            smallFigureImageUITexture.width = ConstData.TEXTURE2D_SMALL_FIGURE_IMAGE_WIDTH;
            smallFigureImageUITexture.height = ConstData.TEXTURE2D_SMALL_FIGURE_IMAGE_HEIGHT;
            smallFigureImageUITexture.alpha = 1f;

            FigureImageDataClear();
            if (figureImageKeyList.Count != 0) {
                for (int i = 0; i < figureImageKeyList.Count; i++) {
                    string uiKey = figureImageKeyList[i];
                    string fiIndex = figureImageFIIndexList[i];
                    float scale_x = figureImageScaleList[i].Key;
                    float scale_y = figureImageScaleList[i].Value;
                    float pos_x = figureImagePosList[i].Key;
                    float pos_y = figureImagePosList[i].Value;
                    Texture2D texture2D = resourceManager.Get<Texture2D>(fiIndex);
                    float width = texture2D.width;
                    float height = texture2D.height;

                    GameObject go = new GameObject(uiKey);
                    UITexture uiTexture = go.AddComponent<UITexture>();
                    uiTexture.mainTexture = texture2D;
                    uiTexture.width = (int)(width * scale_x);
                    uiTexture.height = (int)(height * scale_y);
                    go.transform.position = new Vector3(pos_x, pos_y, 0);
                    go.transform.SetParent(root.transform, false);
                    KeyValuePair<string, UITexture> pair = new KeyValuePair<string, UITexture>(fiIndex, uiTexture);
                    figureImageDict.Add(uiKey, pair);
                }
            }

            if (choiceItemList.Count != 0) {
                ChoiceCreate(choiceItemList);
            }
        }

        public void InitializeStory() {
            textContextContainer.text = "";
            textNameContainer.text = "";
            nameLabel.text = "";
            contextLabel.text = "";
            backgroundUITexture.mainTexture = null;
            backgroundUITextureTop.mainTexture = null;
            smallFigureImageUITexture.mainTexture = null;
            smallFigureImageUITextureTop.mainTexture = null;
            choice0.SetActive(false);
            choice1.SetActive(false);
            choice2.SetActive(false);
            choice3.SetActive(false);

            root.SetActive(false);
            isShow = false;
            isWorking = false;
            sequence = null;
            action = null;
            dialogContextIndex = null;
            characterName = null;
            backgroundImageIndex = null;
            figureImageDict = null;
            smallFigureImageIndex = null;
            isConsoleShow = false;
            choosenDLIndex = null;
            choiceItemList = null;
            isOtherShow = false;
    }

        public void FinalizeStory() {
            sequence?.Kill();
            FigureImageDataClear();
            InitializeStory();
        }
    }
}