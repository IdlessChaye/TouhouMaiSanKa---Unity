using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class SaveLoadRenderManager : MonoBehaviour {

        public GameObject slRoot;

        public GameObject confirmRoot;
        public UISprite confirmSprite;

        private StageRenderManager stageRenderManager;
        private Config config;

        private Sequence sequence;
        private UIPanel panel;
        private bool isSLShow;
        private bool isWorking;
        private bool isConfirmShow;
        private bool isConfirmWorking;
        private string selectedName;

        private void Awake() {
            panel = slRoot.GetComponent<UIPanel>();
            config = PachiGrimoire.I.ConfigManager.Config;
            stageRenderManager = PachiGrimoire.I.StageRenderManager;

            confirmRoot.SetActive(false);
            slRoot.SetActive(false);
        }


        #region Input

        private void FixedUpdate() {
            if (isSLShow) {    
                if (!config.HasAnimationEffect) {
                    if (sequence.IsPlaying() == true) {
                        CompleteAnimate();
                    }
                }
                if (sequence.IsPlaying() == true && Input.GetMouseButtonDown(0)) {
                    CompleteAnimate();
                }
                if (isWorking) {
                    if (Input.GetMouseButtonDown(1)) {
                        SLHide();
                    }
                }
            } else if(isConfirmShow) {
                if (!config.HasAnimationEffect) {
                    if (sequence.IsPlaying() == true) {
                        CompleteAnimate();
                    }
                }
                if (sequence.IsPlaying() == true && Input.GetMouseButtonDown(0)) {
                    CompleteAnimate();
                }
                if (isConfirmWorking) {
                    if (Input.GetMouseButtonDown(1)) {
                        ConfirmNOHide();
                    }
                }
            }
        }

        #endregion


        private void JoinTween(Tweener tweener) {
            if (tweener == null) {
                Debug.LogWarning("SaveLoadRenderManager JoinTween");
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


        public void SLShow() {
            isSLShow = true;
            isWorking = false;
            slRoot.SetActive(true);
            panel.alpha = 0f;


            // 初始化SaveLoad
            LoadSaveLoadData();
            Tweener tweener = DoPanelAlpha(panel, 0f, 1f);
            JoinTween(tweener);

            sequence.OnComplete(() => isWorking = true);
        }

        public void SLHide() {
            isWorking = false;
            panel.alpha = 1f;
            Tweener tweener = DoPanelAlpha(panel, 1f, 0f);
            JoinTween(tweener);

            sequence.OnComplete(() => {
                isSLShow = false;
                slRoot.SetActive(false);
                stageRenderManager.SLHide();
            });
        }

        public void ConfirmShow() {
            isSLShow = false;
            isWorking = false;

            isConfirmShow = true;
            isConfirmWorking = false;
            confirmRoot.SetActive(true);
            confirmSprite.alpha = 0f;


            Tweener tweener = DoSpriteAlpha(confirmSprite, 0f, 1f);
            JoinTween(tweener);

            sequence.OnComplete(() => isConfirmWorking = true);
        }

        public void ConfirmNOHide() {
            isConfirmWorking = false;
            confirmSprite.alpha = 1f;
            Tweener tweener = DoSpriteAlpha(confirmSprite, 1f, 0f);
            JoinTween(tweener);

            sequence.OnComplete(() => {
                isConfirmShow = false;
                confirmRoot.SetActive(false);
                isSLShow = true;
                isWorking = false;
            });
        }

        public void ConfirmYESHide() {
            isConfirmWorking = false;
            confirmSprite.alpha = 1f;
            Tweener tweener = DoSpriteAlpha(confirmSprite, 1f, 0f);
            JoinTween(tweener);

            sequence.OnComplete(() => {
                isConfirmShow = false;
                confirmRoot.SetActive(false);
                isSLShow = true;
                isWorking = false;
                panel.alpha = 1f;

                sequence = DOTween.Sequence();
                sequence.Join(DoPanelAlpha(panel, 1f, 0f));
                sequence.OnComplete(() => {
                    isSLShow = false;
                    slRoot.SetActive(false);
                    stageRenderManager.SLHide();
                    PachiGrimoire.I.StageContextManager.LoadStoryRecord(GetSelectedNumber());
                });
            });
        }

        private int GetSelectedNumber() {
            int number = -1;
            number = int.Parse(selectedName);
            return number;
        }

        private void LoadSaveLoadData() {
            
        }

        #region Callbacks

        #endregion

    }
}