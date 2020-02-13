using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestChoiceButton : MonoBehaviour
{
    public UITexture uiTexture;

    private Sequence sequence;

    void Start()
    {
        sequence = DOTween.Sequence();
        // 得到各个选项的文本
        // 不能选择的用不能选的图片渲染，能选的检查mark 是否有已经选过的，选过的用别的选项图片渲染
        UIEventListener.Get(uiTexture.gameObject).needsActiveCollider = false;
        UIEventListener.Get(uiTexture.gameObject).onHover += OnMyHover;
        UIEventListener.Get(uiTexture.gameObject).onClick += OnMyClick;
    }

    void OnMyHover(GameObject gameObject, bool isHovering) {
        print(isHovering + Time.time.ToString());
    }

    void OnMyClick(GameObject gameObject) {
        UITexture texture = gameObject.GetComponent<UITexture>();
        Tweener tweener = DoTextureAlpha(texture, 1f, 0f);
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        // 给每个选项添加回调函数 要有mark和script信息，还要返回到LastState,还要处理choiceItemList
        sequence.Join(tweener);
        sequence.OnComplete(() => {
            Debug.Log("MarkSet!");
            Debug.Log("LoadScript!");
            Debug.Log("ChoiceHide!");
            Debug.Log("StateChange!");
            Debug.Log("Clear choiceItemList!");
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
}
