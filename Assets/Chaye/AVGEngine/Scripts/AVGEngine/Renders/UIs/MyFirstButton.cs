using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class MyFirstButton : MonoBehaviour {
        public UISprite sprite;
        public NGUIAtlas atlas;
        public string spriteNormalName;
        public string spriteHoverName;
        public string spritePressedName;

        [HideInInspector]
        public UIEventListener eventListener;

        public Action onButtonSelect;
        public Action onButtonRelease;
        public Func<bool> onJudgeEnable;


        private void Awake() {
            sprite.atlas = atlas;
            sprite.spriteName = spriteNormalName;
            eventListener = UIEventListener.Get(gameObject);
            eventListener.onPress += OnMyPress;
            eventListener.onHover += OnMyHover;
            onJudgeEnable = () => {
                BaseState state = PachiGrimoire.I.StateMachine.CurrentState;
                if (state == SleepState.Instance) {
                    return false;
                } else {
                    return true;
                }
            };
        }



        private void OnMyPress(GameObject go, bool isPress) {
            if (onJudgeEnable.Invoke() == false)
                return;
            if (isPress == false) {
                return;
            }
            if (string.IsNullOrEmpty(spritePressedName)) {
                if (sprite.spriteName == spriteHoverName) {
                    if (onButtonSelect != null) {
                        onButtonSelect.Invoke();
                    }
                }
                sprite.spriteName = spriteNormalName;
            } else {
                if (sprite.spriteName == spritePressedName) {
                    if (onButtonRelease != null) {
                        onButtonRelease.Invoke();
                    }
                    sprite.spriteName = spriteNormalName;
                } else {
                    if (onButtonSelect != null) {
                        onButtonSelect.Invoke();
                    }
                    sprite.spriteName = spritePressedName;
                }
            }
        }

        private void OnMyHover(GameObject go, bool isIn) {
            if (onJudgeEnable.Invoke() == false)
                return;
            if (isIn == true) {
                if (sprite.spriteName == spritePressedName) {
                    return;
                } else {
                    sprite.spriteName = spriteHoverName;
                }
            } else {
                if (sprite.spriteName == spritePressedName) {
                    return;
                } else {
                    sprite.spriteName = spriteNormalName;
                }
            }
        }

        public void SetNormal() {
            sprite.spriteName = spriteNormalName;
        }

    }
}