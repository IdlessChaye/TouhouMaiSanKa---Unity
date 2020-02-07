using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSubString : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string a = "牧师龙横飞将在";
        print(a.Substring(0, 0).Equals(string.Empty));
        print(a.Substring(0, 1));
        print(a.Substring(-1, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
