using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStackJson : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        List<int> a = new List<int>();
        a.Add(1);
        a.Add(2);
        a.Reverse();
        Stack<int> b = new Stack<int>(a);
        print(b.Pop());
        print(b.Pop());
    }
    void C() {
        List<int> a = new List<int>();
        a.Add(1);
        a.Add(2);
        ListWrapper b = new ListWrapper();
        b.vs = a;
        string json = JsonUtility.ToJson(b);
        print(json);
        List<int> c = new List<int>();
        JsonUtility.FromJsonOverwrite(json, c);

    }
    void B() {
        Stack<int> a = new Stack<int>();
        a.Push(1);
        a.Push(2);
        var b = a.ToArray();
        string json = JsonUtility.ToJson(b);
        print(json);
        JsonUtility.FromJsonOverwrite(json, b);
        print(b);
    }
    void A() {
        Stack<int> s = new Stack<int>();
        s.Push(1);
        s.Push(2);
        var a = s.ToArray();
        foreach (var d in a) {
            print(d);
        }
        s = new Stack<int>(a);
        print(s.Pop());
        print(s.Pop());
    }
    // Update is called once per frame
    void Updatea() {
        Stack<int> si = new Stack<int>();
        si.Push(1);
        si.Push(2);
        Stack<string> vs = new Stack<string>();
        vs.Push("aaa");
        vs.Push("bbb");
        var a = new StackWrapper();
        a.si = si;
        a.ss = vs;
        string json = JsonUtility.ToJson(a);
        print(a);
        var w = new StackWrapper();
        JsonUtility.FromJsonOverwrite(json, w);
        print(w.si);
        print(w.ss);
    }
    [System.Serializable]
    public class StackWrapper {
        public Stack<int> si;
        public Stack<string> ss;
    }

    [System.Serializable]
    public class ListWrapper {
        public List<int> vs;
    }
}
