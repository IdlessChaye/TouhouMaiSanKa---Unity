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
        public Dictionary<string, KeyValuePair<string, UITexture>> FigureImageDict => figureImageDict;
        public string SmallFigureImageIndex => smallFigureImageIndex;
        public List<ChoiceItem> ChoiceItemList => choiceItemList;


        public GameObject uiRoot;
        public GameObject choice0;
        public GameObject choice1;
        public GameObject choice2;
        public GameObject choice3;
        private List<GameObject> choiceList = new List<GameObject>();
        private PachiGrimoire pachiGrimoire;
        private ConstData constData;
        private Config config;
        private StateMachineManager stateMachine;
        private ResourceManager resourceManager;
        private float messageSpeedLowest;
        private float messageSpeedHighest;


        #region Animate
        private Sequence sequenceAnimate;
        private Sequence sequenceUI;
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
        private Dictionary<string, KeyValuePair<string, UITexture>> figureImageDict = new Dictionary<string, KeyValuePair<string, UITexture>>(); // 标识符 -> <索引,对象>
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
        private List<ChoiceItem> choiceItemList = new List<ChoiceItem>();
        #endregion

        #region Backlog

        private BacklogRenderManager backlogRenderManager;
        private bool isBacklogShow;

        #endregion


        private void Initialize() {
            pachiGrimoire = PachiGrimoire.I;
            constData = pachiGrimoire.constData;
            config = pachiGrimoire.ConfigManager.Config;
            stateMachine = pachiGrimoire.StateMachine;
            resourceManager = pachiGrimoire.ResourceManager;
            backlogRenderManager = GetComponent<BacklogRenderManager>();

            messageSpeedLowest = constData.MessageSpeedLowest;
            messageSpeedHighest = constData.MessageSpeedHighest;
            textContextContainer.text = "";
            textNameContainer.text = "";
            nameLabel.text = "";
            contextLabel.text = "";
            backgroundUITexture.mainTexture = null;
            backgroundUITextureTop.mainTexture = null;
            smallFigureImageUITexture.mainTexture = null;
            smallFigureImageUITextureTop.mainTexture = null;
        }


        #region Input
        private void FixedUpdate() {
            if (isBacklogShow) { 
                return;
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
            if (state == RunAnimateState.Instance) {
                CompleteAnimate();
            }
        }
        private void OnMouseRightDown() {
            BaseState state = stateMachine.CurrentState;
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
        }
        private void OnMouseScrollWheelZoomOut() {
            BaseState state = stateMachine.CurrentState;
            if (state == RunAnimateState.Instance) {
                CompleteAnimate();
            }
        }
        private void OnMouseScrollWheelZoomIn() {
            BaseState state = stateMachine.CurrentState;
            if(state == RunAnimateState.Instance) {
                sequenceAnimate.Pause();
                BacklogShow();
            } else if(state == RunWaitState.Instance) {
                BacklogShow();
            }
        }
        private void OnKeyConfirmDown() {
            BaseState state = stateMachine.CurrentState;
            if (state == RunWaitState.Instance) {
                stateMachine.TransferStateTo(RunScriptState.Instance);
            }
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
            sequenceAnimate.Complete();
        }

        private void StateQuitAnimate() {
            stateMachine.TransferStateTo(stateMachine.LastState);
        }


        #endregion


        #region Text
        public void TextClear() {
            textContextContainer.text = "";
            textNameContainer.text = "";
            nameLabel.text = "";
            contextLabel.text = "";
            dialogContextIndex = null;
            characterName = null;
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
            go.transform.SetParent(uiRoot.transform, false);
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
                    Destroy(go);
                };
                actionAnimate -= StateQuitAnimate;
                actionAnimate += StateQuitAnimate;
            } else {
                uiTexture.alpha = 0f;
                uiTexture.mainTexture = null;
                Destroy(go);
            }

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

        public void ChoiceCreate(List<ChoiceItem> choiceItemList) { // 不管是Run Auto Skip 总有渲染效果
            if (choiceItemList == null || choiceItemList.Count == 0) {
                throw new System.Exception("StageRenderManager ChoiceCreate");
            }
            this.choiceItemList = choiceItemList;

            int count = choiceItemList.Count;
            // 得到各个选项的文本
            // 不能选择的用不能选的图片渲染，能选的检查mark 是否有已经选过的，选过的用别的选项图片渲染
            // 给每个选项添加回调函数 要有mark和script信息
            choiceList.Clear();
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

            sequenceAnimate.OnComplete(() => { // 只要做出选择 要处理choiceItemList，处理Backlog ,SetActive，State
                choiceItemList.Clear();
                choiceList.Clear();
                BacklogItem backlogItem = new BacklogItem(null, choosenDLIndex, null, "Choice"); // 选项的backlog name是 Choice
                PachiGrimoire.I.BacklogManager.Push(backlogItem);
                stateMachine.TransferStateTo(stateMachine.LastState);
            });
        }

        #endregion


        #region Backlog
        public void BacklogShow() {
            isBacklogShow = true;
            backlogRenderManager.BacklogShow();
        }

        public void BacklogHide() {
            isBacklogShow = false;
        }

        #endregion


        public void LoadStoryData() {

        }
    }
}