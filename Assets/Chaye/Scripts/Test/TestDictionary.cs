using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDictionary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, int> a = new Dictionary<string, int>();
        List<int> b = new List<int>(); ;
        b.Add(1);
        List<string> c = new List<string>();
        c.Add("asdsadsad");
        a.Add(c[0], b[0]);
        b[0] = 2;
        c[0] = "?ASD?DAD    ";
        //Debug.Log(a["?ASD?DAD    "]);
        Debug.Log(a["asdsadsad"]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
