using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class StageRenderManager : MonoBehaviour {
        #region Instance
        private static StageRenderManager instance;
        public static StageRenderManager I => instance;
        private void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(this);
            }
            Initialize();
        }
        #endregion

        public string DialogContextIndex => dialogContextIndex;
        public string CharacterName => characterName;
        public string BackgroundImageIndex => backgroundImageIndex;
        public string SmallFigureImageIndex => smallFigureImageIndex;
        public Dictionary<string, KeyValuePair<string, UITexture>> FigureImageDict => figureImageDict;
        public List<ChoiceItem> ChoiceItemList => choiceItemList;


        public GameObject gameRoot;
        public UITexture consoleBackgroundImageUITexture;
        public GameObject choice0;
        public GameObject choice1;
        public GameObject choice2;
        public GameObject choice3;
        private List<GameObject> choiceList;
        private PachiGrimoire pachiGrimoire;
        private ConstData constData;
        private Config config;
        private StateMachineManager stateMachine;
        private ResourceManager resourceManager;
        private MusicManager musicManager;
        private float messageSpeedLowest;
        private float messageSpeedHighest;
        private UIPanel panel;
        private bool isGameShow;
        private bool isWorking;


        #region Animate
        private Sequence sequenceAnimate;
        private System.Action actionAnimate;
        #endregion

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

        #region Backlog

        private BacklogRenderManager backlogRenderManager;
        private bool isBacklogShow;

        #endregion

        #region Config

        private ConfigRenderManager configRenderManager;
        private bool isConfigShow;

        #endregion

        #region SaveLoad

        private SaveLoadRenderManager slRenderManager;
        private bool isSLShow;

        #endregion

        private void Initialize() {
            panel = gameRoot.GetComponent<UIPanel>();
            pachiGrimoire = PachiGrimoire.I;
            constData = pachiGrimoire.constData;
            config = pachiGrimoire.ConfigManager.Config;
            stateMachine = pachiGrimoire.StateMachine;
            resourceManager = pachiGrimoire.ResourceManager;
            musicManager = pachiGrimoire.MusicManager;
            backlogRenderManager = GetComponent<BacklogRenderManager>();
            configRenderManager = GetComponent<ConfigRenderManager>();
            slRenderManager = GetComponent<SaveLoadRenderManager>();

            messageSpeedLowest = constData.MessageSpeedLowest;
            messageSpeedHighest = constData.MessageSpeedHighest;

            InitializeStory();
        }


        #region Input
        private void FixedUpdate() {
            if (isBacklogShow || isConfigShow || isSLShow) {
                return;
            }

            if (!isGameShow) { 
                return;
            }

            if(!config.HasAnimationEffect) {
                if(sequenceAnimate.IsPlaying() == true) {
                    CompleteAnimate();
                }
            }

            if (isWorking == false && sequenceAnimate.IsPlaying() == true && Input.GetMouseButtonDown(0)) {
                CompleteAnimate();
            }

            if (!isWorking)
                return;

            BaseState state = stateMachine.CurrentState;
            if (state == SleepState.Instance) {
                return;
            }

            StateBuff stateBuff = stateMachine.StateBuff; // Skip Animation
            if(stateBuff == StateBuff.Skip) {
                if(state == RunAnimateState.Instance) {
                    CompleteAnimate();
                }
            }

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
        private void OnMouseRightDown() {
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
        private void OnMouseScrollWheelZoomOut() {
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
        private void OnMouseScrollWheelZoomIn() {
            BaseState state = stateMachine.CurrentState;
            StateBuff buff = stateMachine.StateBuff;
            if (buff == StateBuff.Normal) {
                if (state == RunAnimateState.Instance || state == RunWaitState.Instance || state == ChoiceWaitState.Instance) {
                    BacklogShow();
                }
            } else if(buff == StateBuff.Auto) {
                if (state == RunAnimateState.Instance || state == RunWaitState.Instance || state == ChoiceWaitState.Instance) {
                    stateMachine.SetStateBuff(StateBuff.Normal);
                    BacklogShow();
                }
            } else if(buff==StateBuff.Skip) {
                if (state == RunAnimateState.Instance || state == RunWaitState.Instance || state == ChoiceWaitState.Instance) {
                    stateMachine.SetStateBuff(StateBuff.Normal);
                    BacklogShow();
                }
            }
        }
        private void OnKeyConfirmDown() {
            OnMouseLeftDown();
        }

        #endregion


        #region Tween
        private void JoinTweenAniamte(Tweener tweener) {
            if (tweener == null) {
                Debug.LogWarning("StageRenderManager JoinTweenAniamte");
                return;
            }
            if (sequenceAnimate == null || sequenceAnimate.IsPlaying() == false) {
                sequenceAnimate = DOTween.Sequence();
                actionAnimate = null;
            }
            sequenceAnimate.Join(tweener);
        }

        public void CompleteAnimate() {
            sequenceAnimate?.Complete();
        }

        public void PauseAnimate() {
            sequenceAnimate?.Pause();
        }

        public void PlayAnimate() {
            sequenceAnimate?.Play();
        }

        private void StateQuitAnimate() {
            sequenceAnimate?.Kill();
            stateMachine.TransferStateTo(RunScriptState.Instance);
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
            TextClear();
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
                    JoinTweenAniamte(tweenerName);
                }
                Tweener tweenerContext = textContextContainer.DOText(context, textShowTime)
                    .SetDelay(textDelayTime)
                    .OnUpdate(() => contextLabel.text = textContextContainer.text);
                JoinTweenAniamte(tweenerContext);

                sequenceAnimate.OnComplete(() => actionAnimate.Invoke());
                actionAnimate -= StateQuitAnimate;
                actionAnimate += StateQuitAnimate;
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
                JoinTweenAniamte(tweenerDisappear);
                Tweener tweenerAppear = DoTextureAlpha(backgroundUITexture, 0f, 1f);
                JoinTweenAniamte(tweenerAppear);

                sequenceAnimate.OnComplete(() => actionAnimate.Invoke());
                actionAnimate += () => {
                    backgroundUITextureTop.mainTexture = backgroundTexture2D;
                    backgroundUITextureTop.width = ConstData.TEXTURE2D_WIDTH;
                    backgroundUITextureTop.height = ConstData.TEXTURE2D_HEIGHT;
                    backgroundUITextureTop.alpha = 1f;
                    backgroundUITexture.mainTexture = null;
                    backgroundUITexture.alpha = 0f;
                };
                actionAnimate -= StateQuitAnimate;
                actionAnimate += StateQuitAnimate;
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
                JoinTweenAniamte(tweener);

                sequenceAnimate.OnComplete(() => actionAnimate.Invoke());
                actionAnimate += () => {
                    backgroundUITextureTop.mainTexture = null;
                    backgroundUITextureTop.width = ConstData.TEXTURE2D_SCREEN_WIDTH;
                    backgroundUITextureTop.height = ConstData.TEXTURE2D_SCREEN_HEIGHT;
                    backgroundUITextureTop.alpha = 1f;
                };
                actionAnimate -= StateQuitAnimate;
                actionAnimate += StateQuitAnimate;
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
            go.transform.SetParent(gameRoot.transform, false);
            KeyValuePair<string, UITexture> pair = new KeyValuePair<string, UITexture>(fiIndex, uiTexture);
            figureImageDict.Add(uiKey, pair);

            if (hasEffect) {
                uiTexture.alpha = 0f;
                Tweener tweener = DoTextureAlpha(uiTexture, 0f, 1f);
                JoinTweenAniamte(tweener);

                sequenceAnimate.OnComplete(() => actionAnimate.Invoke());
                actionAnimate -= StateQuitAnimate;
                actionAnimate += StateQuitAnimate;
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
                JoinTweenAniamte(tweener);

                sequenceAnimate.OnComplete(() => actionAnimate.Invoke());
                actionAnimate += () => {
                    uiTexture.mainTexture = null;
                    Destroy(uiTexture);
                    Destroy(go);
                };
                actionAnimate -= StateQuitAnimate;
                actionAnimate += StateQuitAnimate;
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
                JoinTweenAniamte(tweenerDisappear);
                Tweener tweenerAppear = DoTextureAlpha(smallFigureImageUITexture, 0f, 1f);
                JoinTweenAniamte(tweenerAppear);

                sequenceAnimate.OnComplete(() => actionAnimate.Invoke());
                actionAnimate += () => {
                    smallFigureImageUITextureTop.mainTexture = smallFigureTexture2D;
                    smallFigureImageUITextureTop.width = ConstData.TEXTURE2D_SMALL_FIGURE_IMAGE_WIDTH;
                    smallFigureImageUITextureTop.height = ConstData.TEXTURE2D_SMALL_FIGURE_IMAGE_HEIGHT;
                    smallFigureImageUITextureTop.alpha = 1f;
                    smallFigureImageUITexture.mainTexture = null;
                    smallFigureImageUITexture.alpha = 0f;
                };
                actionAnimate -= StateQuitAnimate;
                actionAnimate += StateQuitAnimate;
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
                JoinTweenAniamte(tweener);

                sequenceAnimate.OnComplete(() => actionAnimate.Invoke());
                actionAnimate += () => {
                    smallFigureImageUITextureTop.mainTexture = null;
                    smallFigureImageUITextureTop.width = ConstData.TEXTURE2D_WIDTH;
                    smallFigureImageUITextureTop.height = ConstData.TEXTURE2D_HEIGHT;
                    smallFigureImageUITextureTop.alpha = 1f;
                };
                actionAnimate -= StateQuitAnimate;
                actionAnimate += StateQuitAnimate;
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
                JoinTweenAniamte(tweener);
            }
            BaseState lastState = stateMachine.LastState;

            sequenceAnimate.OnComplete(()=>actionAnimate.Invoke());
            actionAnimate += () => { // 只要做出选择 要处理choiceItemList，处理Backlog ,SetActive，State
                choiceItemList.Clear();
                choiceList.Clear();
                BacklogItem backlogItem = new BacklogItem(null, choosenDLIndex, null, constData.ChoiceBacklogItemName); // 选项的backlog name是 Choice
                PachiGrimoire.I.BacklogManager.Push(backlogItem);
            };
            actionAnimate -= StateQuitAnimate;
            actionAnimate += StateQuitAnimate;
        }
        #endregion



        #region GamePanel
        public void GameShow() {
            isGameShow = true;
            isWorking = false;
            gameRoot.SetActive(true);
            panel.alpha = 0f;

            // 初始化Game
            InitGameData();
            Tweener tweener = DoPanelAlpha(panel, 0f, 1f);
            JoinTweenAniamte(tweener);

            sequenceAnimate.OnComplete(() => isWorking = true);
        }

        public void GameHide() {
            isWorking = false;
            panel.alpha = 1f;
            Tweener tweener = DoPanelAlpha(panel, 1f, 0f);
            JoinTweenAniamte(tweener);

            sequenceAnimate.OnComplete(() => {
                isGameShow = false;
                gameRoot.SetActive(false);
                //backlogItemList.Clear();
                //renderManager.BacklogHide();
            });
        }

        private void InitGameData() {
            SetAlphaOfConsole();
        }
        #endregion



        #region Backlog

        public void BacklogShow() {
            stateMachine.TransferStateTo(SleepState.Instance); // Go to Sleep
            PauseAnimate();
            isBacklogShow = true;
            backlogRenderManager.BacklogShow();
        }

        public void BacklogHide() {
            isBacklogShow = false;
            PlayAnimate();
            stateMachine.TransferStateTo(stateMachine.LastState); // Sleep Back
        }

        #endregion


        #region Config
        public void ConfigShow() {
            stateMachine.TransferStateTo(SleepState.Instance); // Go to Sleep
            PauseAnimate();
            isConfigShow = true;
            configRenderManager.ConfigShow();
        }

        public void ConfigHide() {
            isConfigShow = false;
            PlayAnimate();
            stateMachine.TransferStateTo(stateMachine.LastState); // Sleep Back
        }
        #endregion



        #region SaveLoad

        public void SLShow() {
            stateMachine.TransferStateTo(SleepState.Instance); // Go to Sleep
            PauseAnimate();
            isSLShow = true;
            slRenderManager.SLShow();
        }
        public void SLHide() {
            isSLShow = false;
            PlayAnimate();
            stateMachine.TransferStateTo(stateMachine.LastState); // Sleep Back
        }

        #endregion





        public void LoadStoryRecord(StoryRecord sr) {
            sequenceAnimate?.Pause();
            sequenceAnimate = DOTween.Sequence();

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
                    go.transform.SetParent(gameRoot.transform, false);
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

            gameRoot.SetActive(false);
            isGameShow = false;
            isWorking = false;
            sequenceAnimate = null;
            actionAnimate = null;
            dialogContextIndex = null;
            characterName = null;
            backgroundImageIndex = null;
            figureImageDict = null;
            smallFigureImageIndex = null;
            isConsoleShow = false;
            choosenDLIndex = null;
            choiceItemList = null;
            isBacklogShow = false;
            isConfigShow = false;
            isSLShow = false;
        }

        public void FinalizeStory() {
            sequenceAnimate?.Kill();
            FigureImageDataClear();
            InitializeStory();
        }
    }
}