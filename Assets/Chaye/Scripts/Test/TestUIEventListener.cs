using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIEventListener : MonoBehaviour
{
    public GameObject go;
    private UIEventListener listener;
    void Start()
    {
        listener = UIEventListener.Get(go);
        listener.onPress += OnMyPress;
        listener.onClick += OnMyClick;
        listener.onScroll += OnScroll;
    }

    private void OnMyPress(GameObject go, bool bol) { // 点击滚轮中键也能执行，但左右键都是false
        print(go.name);
        print("左键 " + Input.GetMouseButton(0)); 
        print("右键 " + Input.GetMouseButton(1));
        print(bol); // 鼠标动的一瞬间执行，一开始必须在Collider内，不管最后出不出Collider，都执行。
    }
    private void OnMyClick(GameObject go) {
       
    }

    private void OnScroll(GameObject go,float value) {
        print(go.name);
        print("OnScroll :" + value);
    }


    public void OnClickDebug() {
        Debug.Log("OnClickDebug");
    }
}
