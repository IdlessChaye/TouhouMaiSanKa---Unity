using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    [System.Serializable]
    public class StoryRecord {
        public string dateTime;

        public string currentStateName;
        public string LastStateName;
        public StateBuff StateBuff;

        public string bgmIndex;
        public string voiceIndex;
        public string voiceCharacterName;

        public string scriptPointerScriptName;
        public int scriptPointerLineNumber;
        public List<string> scriptReplaceKeys = new List<string>();
        public List<string> scriptReplaceValues = new List<string>();
        public List<string> pointerScriptNameStack = new List<string>();
        public List<int> pointerLineNumberStack = new List<int>();


        public List<string> markStoryList = new List<string>();
        public List<string> varStoryNameList = new List<string>();
        public List<float> varStoryValueList = new List<float>();
        public string chapterName;

        public string dialogContextIndex;
        public string characterName;
        public string backgroundImageIndex;
        public List<string> figureImageKeyList = new List<string>();
        public List<string> figureImageFIIndexList = new List<string>();
        public List<float> figureImagePosXList = new List<float>();
        public List<float> figureImagePosYList = new List<float>();
        public List<float> figureImageScaleXList = new List<float>();
        public List<float> figureImageScaleYList = new List<float>();
        public string smallFigureImageIndex;
        public List<ChoiceItem> choiceItemList = new List<ChoiceItem>();

        public List<BacklogItem> backlogItemList;
        public int capacity;
        public int count;
        public int head;
    }
}
