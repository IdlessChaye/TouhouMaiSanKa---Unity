using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class StageContextManager {
        private PlayerRecordManager recordManager;

        private StateMachineManager stateMachine;
        private ScriptManager scriptManager;
        private MarkManager markManager;
        private BacklogManager backlogManager;
        private MusicManager musicManager;
        private PastScriptManager pastScriptManager;
        private StageRenderManager renderManager;

        public StageContextManager() { 
            recordManager = PachiGrimoire.I.PlayerRecordManager;
            stateMachine = PachiGrimoire.I.StateMachine;
            scriptManager = PachiGrimoire.I.ScriptManager;
            markManager = PachiGrimoire.I.MarkManager;
            backlogManager = PachiGrimoire.I.BacklogManager;
            pastScriptManager = PachiGrimoire.I.PastScriptManager;
            musicManager = PachiGrimoire.I.MusicManager;
            renderManager = PachiGrimoire.I.StageRenderManager;
        }

        public void SavePlayerRecord() {
            BaseState currentState = stateMachine.CurrentState;

            PlayerRecord playerRecord = new PlayerRecord();

            playerRecord.markPlayerList = markManager.MarkPlayerList;
            playerRecord.varPlayerNameList = new List<string>(markManager.ValuePlayerDict.Keys);
            playerRecord.varPlayerValueList = new List<float>(markManager.ValuePlayerDict.Values);

            var pastScriptDict = pastScriptManager.PastScriptDict;
            List<string> pastScriptNameList = new List<string>();
            List<KeyValuePair<int, int>> pastScriptRangeList = new List<KeyValuePair<int, int>>();
            foreach (string key in pastScriptDict.Keys) {
                var pair = pastScriptDict[key];
                pastScriptNameList.Add(key);
                pastScriptRangeList.Add(pair);
            }
            playerRecord.pastScriptNameList = pastScriptNameList;
            playerRecord.pastScriptRangeList = pastScriptRangeList;

            recordManager.SavePlayerRecord(playerRecord);
        }

        public void SaveStoryRecord(int indexOfRecord) {
            BaseState currentState = stateMachine.CurrentState;
            if (currentState != RunWaitState.Instance && currentState != ChoiceWaitState.Instance) {
                throw new System.Exception("StageContextManager SaveStoryRecord");
            }

            StoryRecord storyRecord = new StoryRecord();

            storyRecord.LastStateName = stateMachine.LastState.StateName;
            storyRecord.currentStateName = stateMachine.CurrentState.StateName;

            storyRecord.bgmIndex = musicManager.BGMIndex;
            storyRecord.voiceIndex = musicManager.VoiceIndex;
            storyRecord.voiceCharacterName = musicManager.CharacterName;

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




            recordManager.SaveStoryRecord(indexOfRecord, storyRecord);
        }

        public void LoadPlayerRecord() {
            PlayerRecord playerRecord = recordManager.PlayerRecord;

            markManager.LoadPlayerRecord(playerRecord.markPlayerList, playerRecord.varPlayerNameList, playerRecord.varPlayerValueList);
            pastScriptManager.LoadPlayerRecord(playerRecord.pastScriptNameList, playerRecord.pastScriptRangeList);
        }

        public void LoadStoryRecord(int indexOfRecord) {
            Dictionary<int, StoryRecord> storyRecordDict = recordManager.StoryRecordDict;
            StoryRecord sr = storyRecordDict[indexOfRecord];

            stateMachine.LoadStoryRecord(sr.currentStateName, sr.LastStateName);
            musicManager.LoadStoryRecord(sr.bgmIndex, sr.voiceIndex, sr.voiceCharacterName);
            scriptManager.LoadStoryRecord(sr.scriptPointerScriptName, sr.scriptPointerLineNumber, 
                sr.scriptReplaceKeys, sr.scriptReplaceValues, 
                sr.pointerScriptNameStack, sr.pointerLineNumberStack);
            markManager.LoadStoryRecord(sr.markStoryList, sr.varStoryNameList, sr.varStoryValueList);
            backlogManager.LoadStoryRecord(sr.backlogItemList, sr.head, sr.capacity, sr.count);
            //renderManager.LoadStoryData 负责Choice Backlog Image Text
        }


    }
}