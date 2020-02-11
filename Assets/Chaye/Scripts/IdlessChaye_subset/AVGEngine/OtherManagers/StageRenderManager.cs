using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class StageRenderManager : MonoBehaviour{
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
        public UITexture backgroundTexture;
        private string backgroundImageIndex;
        #endregion

        #region FigureImage
        private Dictionary<string, KeyValuePair<string, UITexture>> figureImageDict = new Dictionary<string, KeyValuePair<string, UITexture>>(); // 标识符 -> <索引,对象>
        public UITexture smallFigureImageTexture;
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
        }




        private void JoinTweenAniamte(Tweener tweener) {
            if (sequenceAnimate == null || sequenceAnimate.IsPlaying() == false)
                sequenceAnimate = DOTween.Sequence();
            sequenceAnimate.Join(tweener);
        }

        public void  CompleteAnimate() {
            sequenceAnimate.Complete();
        }





        public void TextClear() {
            textContextContainer.text = "";
            textNameContainer.text = "";
            nameLabel.text = "";
            contextLabel.text = "";
            scriptContextIndex = null;
            characterName = null;
        }

        public void TextChange(string scriptContextIndex, string characterName = null) {
            if(string.IsNullOrEmpty(scriptContextIndex) ) {
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
                .OnUpdate(()=> contextLabel.text = textContextContainer.text);
            JoinTweenAniamte(tweenerContext);

            sequenceAnimate.OnComplete(() => StateQuitAnimate());
        }

        private void StateQuitAnimate() {
            stateMachine.TransferStateTo(stateMachine.LastState);
        }


    }
}