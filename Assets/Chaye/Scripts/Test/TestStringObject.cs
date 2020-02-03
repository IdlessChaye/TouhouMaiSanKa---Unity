using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStringObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string wtf = "WTF?!";
        SS WTF = new SS { str = wtf };
        print(WTF.str);
        Object oWTF = WTF;
        SS WWTF = oWTF as SS;
        print(WTF.str);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class SS : Object {
    public string str { get; set; }
}
