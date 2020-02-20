using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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


    private void Awake() {
        sprite.atlas = atlas;
        sprite.spriteName = spriteNormalName;
        eventListener = UIEventListener.Get(gameObject);
        eventListener.onPress += OnMyPress;
        eventListener.onHover += OnMyHover;
    }



    private void OnMyPress(GameObject go,bool isPress) {
        if(isPress == false) {
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
