using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestToggleAndSlider : MonoBehaviour
{
    private bool isTrue;
    public UISlider UISlider;
    public UIToggle UIToggle;
    public UIToggle UIToggle2;

    private void Start() {
        UIToggle.value = true;
        UIToggle2.value = true;
    }
    public void SetTrue() {
        isTrue = true;
        print(isTrue);
    }

    public void SetFalse() {
        isTrue = false;
        print(isTrue);
    }

    public void SetValue() {
        float va = UISlider.value;
        print(va);
    }
}
