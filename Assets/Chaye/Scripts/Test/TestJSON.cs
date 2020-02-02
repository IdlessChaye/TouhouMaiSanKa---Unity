using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestJSON : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        Record record = new Record();
        record.recordList = new List<string>(new string[] { "1", "2" });

        string json = JsonUtility.ToJson(record);
        print(json);
        Record newRecord = new Record();
        JsonUtility.FromJsonOverwrite(json, newRecord);
        foreach(string s in newRecord.recordList) {
            print(s);
        }
    }
}

[Serializable]
public class Record {
    public List<string> recordList;
    
    
}


[Serializable]
public class Serialization<T> {
    [SerializeField]
    private List<T> target;
    public List<T> ToList() {
        return target;
    }
    public Serialization(List<T> target) {
        this.target = target;
    }
}