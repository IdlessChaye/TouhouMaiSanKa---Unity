using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    [System.Serializable]
    public class StoryRecord {
        public string currentStateName;
        public string LastStateName;

        public List<string> markStoryList = new List<string>();
        public List<string> varStoryNameList = new List<string>();
        public List<float> varStoryValueList = new List<float>();
    }
}
