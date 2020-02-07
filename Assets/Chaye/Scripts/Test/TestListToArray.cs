using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestListToArray : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<string> a = new List<string>();
        a.Add("所应国足三潜艇");
        a.Add("二五风月也无情");
        string[] b = a.ToArray();
        string[] c = b;
        a.Clear();
        a.Add("烟笼寒水月笼沙");
        a[0] = "烟笼寒水月笼      ;";
        print(b[0]);
        b[0] = "??";
        print(c[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
