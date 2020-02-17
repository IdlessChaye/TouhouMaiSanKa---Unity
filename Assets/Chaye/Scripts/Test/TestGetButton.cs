using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TestGetButton : MonoBehaviour
{
    public GameObject go;
    public UISprite UISprite;
    Sequence sequence;
    public UITexture ui;
    void Start()
    {
        UIEventListener.Get(go).onClick += (GameObject go) => Debug.Log("Whattttt?!");
        float value = 0f;
        Tweener tweener = DOTween.To(() => value, (x) => value = x, 1f, 5f)
                 .OnUpdate(() => UISprite.alpha = value);

         sequence = DOTween.Sequence();
        sequence.Join(tweener);
        sequence.OnComplete(() => {
            sequence = DOTween.Sequence();
            sequence.Join(DOTween.To(() => value, (x) => value = x, 0f, 5f)
                 .OnUpdate(() => UISprite.alpha = value));
            sequence.OnComplete(() => Debug.Log("!!"));
        });

        float value1 = 0f;
        DOTween.To(() => value1, (x) => value1 = x, 1f, 5f)
                 .OnUpdate(() => ui.alpha = value1);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) {
            sequence.Complete();
        }
    }
}
