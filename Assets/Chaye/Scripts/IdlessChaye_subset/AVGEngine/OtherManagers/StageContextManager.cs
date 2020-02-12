using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class StageContextManager {
        private PlayerRecordManager recordManager;

        private StateMachineManager stateMachine;
        private ScriptManager scriptManager;
        private MarkManager markManager;
        private StageRenderManager renderManager;
        private BacklogManager backlogManager;
        private PastScriptManager pastScriptManager;

        public StageContextManager() { 
            recordManager = PachiGrimoire.I.PlayerRecordManager;
            stateMachine = PachiGrimoire.I.StateMachine;
            scriptManager = PachiGrimoire.I.ScriptManager;
            markManager = PachiGrimoire.I.MarkManager;
            renderManager = PachiGrimoire.I.StageRenderManager;
            backlogManager = PachiGrimoire.I.BacklogManager;
            pastScriptManager = PachiGrimoire.I.PastScriptManager;
        }

        public void SavePlayerRecord() {
            PlayerRecord playerRecord = new PlayerRecord();

            playerRecord.markPlayerList = markManager.MarkPlayerList;
            playerRecord.varPlayerNameList = new List<string>(markManager.ValuePlayerDict.Keys);
            playerRecord.varPlayerValueList = new List<float>(markManager.ValuePlayerDict.Values);

            recordManager.SavePlayerRecord(playerRecord);
        }

        public void SaveStoryRecord(int indexOfRecord) {
            StoryRecord storyRecord = new StoryRecord();

            storyRecord.LastStateName = stateMachine.LastState.StateName;
            storyRecord.currentStateName = stateMachine.CurrentState.StateName;

            storyRecord.scriptPointerScriptName = scriptManager.ScriptPointerScriptName;
            storyRecord.scriptPointerLineNumber = scriptManager.ScriptPointerLineNumber;
            storyRecord.scriptReplaceKeys = scriptManager.ScriptReplaceKeys;
            storyRecord.scriptReplaceValues = scriptManager.ScriptReplaceValues;
            var pointerScriptNameStack = new List<string>(scriptManager.PointerScriptNameStack.ToArray());
            pointerScriptNameStack.Reverse();
            storyRecord.pointerScriptNameStack = pointerScriptNameStack;
            var pointerLineNumberStack = new List<int>(scriptManager.PointerLineNumberStack.ToArray());
            pointerLineNumberStack.Reverse();
            storyRecord.pointerLineNumberStack = pointerLineNumberStack;


            storyRecord.markStoryList = markManager.MarkStoryList;
            storyRecord.varStoryNameList = new List<string>(markManager.ValueStoryDict.Keys);
            storyRecord.varStoryValueList = new List<float>(markManager.ValueStoryDict.Values);


            storyRecord.dialogContextIndex = renderManager.DialogContextIndex;
            storyRecord.characterName = renderManager.CharacterName;
            storyRecord.backgroundImageIndex = renderManager.BackgroundImageIndex;
            List<string> figureImageKeyList = new List<string>();
            List<string> figureImageFIIndexList = new List<string>();
            List<KeyValuePair<float, float>> figureImagePosList = new List<KeyValuePair<float, float>>();
            List<KeyValuePair<float, float>> figureImageScaleList = new List<KeyValuePair<float, float>>();
            Dictionary<string, KeyValuePair<string, UITexture>> figureImageDict = renderManager.FigureImageDict;
            foreach (string key in figureImageDict.Keys) {
                KeyValuePair<string, UITexture> pair = figureImageDict[key];
                string fiIndex = pair.Key;
                Transform uiTextureTF = pair.Value.transform;
                Vector3 pos = uiTextureTF.localPosition;
                Vector3 scale = uiTextureTF.localScale;
                figureImageKeyList.Add(key);
                figureImageFIIndexList.Add(fiIndex);
                figureImagePosList.Add(new KeyValuePair<float, float>(pos.x, pos.y));
                figureImageScaleList.Add(new KeyValuePair<float, float>(scale.x, scale.y));
            }
            storyRecord.figureImageKeyList = figureImageKeyList;
            storyRecord.figureImageFIIndexList = figureImageFIIndexList;
            storyRecord.figureImagePosList = figureImagePosList;
            storyRecord.figureImageScaleList = figureImageScaleList;
            storyRecord.smallFigureImageIndex = renderManager.SmallFigureImageIndex;
            storyRecord.choiceItemList = renderManager.ChoiceItemList;

            storyRecord.backlogItemList = backlogManager.BacklogItemList;
            storyRecord.capacity = backlogManager.Capacity;
            storyRecord.count = backlogManager.Count;
            storyRecord.head = backlogManager.Head;

            var pastScriptDict = pastScriptManager.PastScriptDict;
            List<string> pastScriptNameList = new List<string>();
            List<KeyValuePair<int, int>> pastScriptRangeList = new List<KeyValuePair<int, int>>();
            foreach(string key in pastScriptDict.Keys) {
                var pair = pastScriptDict[key];
                pastScriptNameList.Add(key);
                pastScriptRangeList.Add(pair);
            }
            storyRecord.pastScriptNameList = pastScriptNameList;
            storyRecord.pastScriptRangeList = pastScriptRangeList;
            

            recordManager.SaveStoryRecord(indexOfRecord, storyRecord);
        }

        public void LoadPlayerRecord() {
            PlayerRecord playerRecord = recordManager.PlayerRecord;

            markManager.LoadPlayerData(playerRecord.markPlayerList, playerRecord.varPlayerNameList, playerRecord.varPlayerValueList);
        }

        public void LoadStoryRecord(int indexOfRecord) {
            Dictionary<int, StoryRecord> storyRecordDict = recordManager.StoryRecordDict;
            StoryRecord storyRecord = storyRecordDict[indexOfRecord];

            stateMachine.LoadStoryRecord(storyRecord.currentStateName, storyRecord.LastStateName);

        }


    }
}