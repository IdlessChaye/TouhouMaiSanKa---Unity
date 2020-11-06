using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCoroutine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(A());
        StartCoroutine(B());
    }

    // Update is called once per frame
    IEnumerator A() {
        while(true) {
            Debug.Log("1");
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator B() {
        while (true) {
            Debug.Log("2");
            yield return new WaitForEndOfFrame();
        }
    }
}
