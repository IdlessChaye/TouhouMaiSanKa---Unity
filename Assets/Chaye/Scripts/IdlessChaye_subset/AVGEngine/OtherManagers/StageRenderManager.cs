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
        public string ScriptContextIndex => scriptContextIndex;
        public string CharacterName => characterName;
        public string BackgroundImageIndex => backgroundImageIndex;
        public Dictionary<string, KeyValuePair<string, UITexture>> FigureImageDict => figureImageDict;
        public string SmallFigureImageIndex => smallFigureImageIndex;


        public GameObject uiRoot;
        private PachiGrimoire pachiGrimoire;
        private ConstData constData;
        private Config config;
        private StateMachineManager stateMachine;
        private ResourceManager resourceManager;

        private float messageSpeedLowest;
        private float messageSpeedHighest;

        #region Animate
        private Sequence sequenceAnimate;
        #endregion


        #region Text
        public Text textContextContainer;
        public Text textNameContainer;
        public UILabel contextLabel;
        public UILabel nameLabel;
        private string scriptContextIndex;
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
        private string smallFigureImageIndex;
        #endregion

        #region Console
        public GameObject console;
        #endregion


        private void Initialize() {
            pachiGrimoire = PachiGrimoire.I;
            constData = pachiGrimoire.constData;
            config = pachiGrimoire.ConfigManager.Config;
            stateMachine = pachiGrimoire.StateMachine;
            resourceManager = pachiGrimoire.ResourceManager;
            messageSpeedLowest = constData.MessageSpeedLowest;
            messageSpeedHighest = constData.MessageSpeedHighest;

            textContextContainer.text = "";
            textNameContainer.text = "";
            nameLabel.text = "";
            contextLabel.text = "";
            backgroundUITexture.mainTexture = null;
        }


        #region Input
        private void FixedUpdate() {
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

        }
        private void OnMouseScrollWheelZoomOut() {

        }
        private void OnMouseScrollWheelZoomIn() {

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
            if(tweener == null) {
                Debug.LogWarning("StageRenderManager JoinTweenAniamte");
                return;
            }
            if (sequenceAnimate == null || sequenceAnimate.IsPlaying() == false)
                sequenceAnimate = DOTween.Sequence();
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
            scriptContextIndex = null;
            characterName = null;
        }

        public void TextChange(string scriptContextIndex, string characterName = null) {
            if (string.IsNullOrEmpty(scriptContextIndex)) {
                throw new System.Exception("StageRenderManager TextChange");
            }
            TextClear();
            this.scriptContextIndex = scriptContextIndex;
            this.characterName = characterName;

            string context = resourceManager.Get<string>(scriptContextIndex);
            float messageSpeed = config.MessageSpeed;
            float textShowTime = scriptContextIndex.Length / (messageSpeedHighest * messageSpeed + messageSpeedLowest);
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

            sequenceAnimate.OnComplete(() => StateQuitAnimate());
        }
        #endregion


        #region BackgroundImage
        public void BackgroundImageChange(string index) {
            if(string.IsNullOrEmpty(index)) {
                throw new System.Exception("StageRenderManager BackgroundImageChange");
            }
            backgroundImageIndex = index;

            Texture2D backgroundTexture2D = resourceManager.Get<Texture2D>(index);
            backgroundUITexture.mainTexture = backgroundTexture2D;
            backgroundUITexture.width = ConstData.TEXTURE2D_WIDTH;
            backgroundUITexture.height = ConstData.TEXTURE2D_HEIGHT;
            backgroundUITexture.alpha = 0f;

            Tweener tweenerDisappear = DoTextureAlpha(backgroundUITextureTop, 1f, 0f);
            JoinTweenAniamte(tweenerDisappear);
            Tweener tweenerAppear = DoTextureAlpha(backgroundUITexture, 0f, 1f);
            JoinTweenAniamte(tweenerAppear);

            sequenceAnimate.OnComplete(() => {
                backgroundUITextureTop.mainTexture = backgroundTexture2D;
                backgroundUITextureTop.width = ConstData.TEXTURE2D_WIDTH;
                backgroundUITextureTop.height = ConstData.TEXTURE2D_HEIGHT;
                backgroundUITextureTop.alpha = 1f;
                backgroundUITexture.mainTexture = null;
                backgroundUITexture.alpha = 0f;
                StateQuitAnimate();
            });
        }

        public void BackgroundImageChangeBlack() {
            backgroundImageIndex = null;
            if(backgroundUITextureTop.mainTexture == null) {
                Debug.LogWarning("BackgroundImageChangeBlack");
                return;
            }
            Tweener tweener = DoTextureAlpha(backgroundUITextureTop, 1f, 0f);
            JoinTweenAniamte(tweener);

            sequenceAnimate.OnComplete(() => {
                backgroundUITextureTop.mainTexture = null;
                backgroundUITextureTop.width = ConstData.TEXTURE2D_WIDTH;
                backgroundUITextureTop.height = ConstData.TEXTURE2D_HEIGHT;
                backgroundUITextureTop.alpha = 1f;
                StateQuitAnimate();
            });
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

    }
}