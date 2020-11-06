﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
public class TestDoTween : MonoBehaviour {


    public UIAtlas UIAtlas;
    public Material material;
    public GameObject canvas;
    public Text text;
    public UILabel label;

    private string state = "???";
    private readonly string RunScript = "RunScript";
    private readonly string Animate = "Animate";
    Sequence sequence;
    Tweener tweener;
    UISprite prite;
    UISprite pritee;
    int index = -1;

    Action action;


    private void Start() {
		
        text.text = "";
        sequence = DOTween.Sequence();
        sequence.AppendCallback(() => Debug.Log("!!"));
        sequence.OnComplete(() => action.Invoke());
        action += () => Debug.Log("???");
        action = null;
        action -= WAWA;
        action += WAWA;
        action -= WAWA;
        action += WAWA; action -= WAWA;
        action += WAWA; action -= WAWA;
        action += WAWA; action -= WAWA;
        action += WAWA; action -= WAWA;
        action += WAWA; action += WAWA; action += WAWA; action += WAWA; action += WAWA;
        sequence.OnComplete(() => action.Invoke());
        action += () => Debug.Log("???");
        //action = null;
        sequence.OnComplete(() => action.Invoke());
        action += () => {
            for(int i = 0;i < 12;i++)
                Debug.Log("!!! " + i);
        };
        sequence.Complete();
    }

    private void WAWA() {
        Debug.Log("WAWA");
    }
    private void Update() {
        label.text = text.text;
        if (state.Equals(RunScript)) {
            index++;
            switch (index) {
                case 0:
                    pritee = SpriteAppear("14", new Vector2(400, -69),1,0);
                    prite = SpriteAppear("13", new Vector2(0, -69));
                    break;
                case 1:
                    state = Animate;
                    JoinTween(text.DOText("支持中文吗？？？", 5f).OnComplete(() => state = "???"));
                    break;
                case 2:
                    text.text = "";
                    JoinTween(DoSpriteAlpha(prite, 1, 0));
                    break;
                case 3:
                    state = Animate;
                    JoinTween(text.DOText("HelloWorld", 5f).OnComplete(() => state = "???"));
                    break;
                default:
                    break;
            }

        }
    }

    public void OnClick() {
        state = RunScript;
    }


	private Sequence _sequence;
	private Image formula;

	private void SwitchFormula()
	{
		if (_sequence != null)
			_sequence.Complete();
		_sequence = DOTween.Sequence();
		if (formula.color.a == 0)
		{ 
			var tween = formula.DOFade(1, 1f);
			_sequence.Join(tween);
		}
		else
		{
			var tween = formula.DOFade(0, 1f);
			_sequence.Join(tween);
		}
	}



	public void OnInterupt() {
        sequence.Complete();
    }

    public UISprite SpriteAppear(string spriteName, Vector2 position,float fromValue = 0f,float toValue =  1f) {
        GameObject go = new GameObject(spriteName);
        UISprite sprite = go.AddComponent<UISprite>();
        sprite.atlas = UIAtlas;
        sprite.spriteName = spriteName;
        sprite.material = material;
        var spriteData = sprite.GetSprite(spriteName);
        int width = spriteData.width * 3;
        int height = spriteData.height * 3;
        var widget = sprite.GetComponent<UIWidget>();
        widget.width = width;
        widget.height = height;
        go.transform.position = new Vector3(position.x, position.y, 0);
        go.transform.SetParent(canvas.transform, false);
        JoinTween(DoSpriteAlpha(sprite, fromValue, toValue));
        return sprite;
    }

    private void JoinTween(Tweener tweener) {
        if (sequence == null || sequence.IsPlaying() == false)
            sequence = DOTween.Sequence();
        sequence.Join(tweener);
    }

    private Tweener DoSpriteAlpha(UISprite sprite, float fromValue, float toValue, float duration = 5f) {
        state = Animate;
        sprite.alpha = fromValue;
        float value = fromValue;
        tweener = DOTween.To(() => value, (x) => value = x, toValue, duration)
            .OnUpdate(() => { sprite.alpha = value; })
            .OnComplete(() => state = RunScript);
        
        




        return tweener;



    }
}
