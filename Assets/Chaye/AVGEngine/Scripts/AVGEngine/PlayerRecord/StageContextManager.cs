using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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



		public string StoryScriptFileName => storyScriptFileName;
		private string storyScriptFileName;

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
            Debug.Log("保存玩家记录!");
            BaseState currentState = stateMachine.CurrentState;

            PlayerRecord playerRecord = new PlayerRecord();

            playerRecord.markPlayerList = markManager.MarkPlayerList;
            playerRecord.varPlayerNameList = new List<string>(markManager.ValuePlayerDict.Keys);
            playerRecord.varPlayerValueList = new List<float>(markManager.ValuePlayerDict.Values);

            var pastScriptDict = pastScriptManager.PastScriptDict;
            List<string> pastScriptNameList = new List<string>();
            List<int> pastScriptLineNumberList = new List<int>();
            foreach (string key in pastScriptDict.Keys) {
                int lineNumber = pastScriptDict[key];
                pastScriptNameList.Add(key);
                pastScriptLineNumberList.Add(lineNumber);
            }
            playerRecord.pastScriptNameList = pastScriptNameList;
            playerRecord.pastScriptLineNumberList = pastScriptLineNumberList;

            recordManager.SavePlayerRecord(playerRecord);
        }

        public void SaveStoryRecord(int indexOfRecord) {
            BaseState currentState = stateMachine.CurrentState;
            if (currentState != RunWaitState.Instance && currentState != ChoiceWaitState.Instance) { //currentState != SleepState.Instance &&
                throw new System.Exception("StageContextManager SaveStoryRecord " + currentState.StateName);
            }

            Debug.Log("保存故事记录! :" + indexOfRecord);

            StoryRecord storyRecord = new StoryRecord();

            storyRecord.dateTime = System.DateTime.Now.ToString();

            storyRecord.LastStateName = stateMachine.LastState.StateName;
            storyRecord.currentStateName = stateMachine.CurrentState.StateName;
            storyRecord.StateBuff = stateMachine.StateBuff;

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
            storyRecord.chapterName = markManager.ChapterName;

            storyRecord.dialogContextIndex = renderManager.DialogContextIndex;
            storyRecord.characterName = renderManager.CharacterName;
            storyRecord.backgroundImageIndex = renderManager.BackgroundImageIndex;
            List<string> figureImageKeyList = new List<string>();
            List<string> figureImageFIIndexList = new List<string>();
            List<float> figureImagePosXList = new List<float>();
            List<float> figureImagePosYList = new List<float>();
            List<float> figureImageScaleXList = new List<float>();
            List<float> figureImageScaleYList = new List<float>();
            Dictionary<string, KeyValuePair<string, UITexture>> figureImageDict = renderManager.FigureImageDict;
            var keyArray = figureImageDict.Keys.ToArray();
            for (int i = 0; i< keyArray.Length;i++) {
                string key = keyArray[i];
                KeyValuePair<string, UITexture> pair = figureImageDict[key];
                string fiIndex = pair.Key;
                Transform uiTextureTF = pair.Value.transform;
                Vector3 pos = uiTextureTF.localPosition;
                Vector3 scale = uiTextureTF.localScale;
                figureImageKeyList.Add(key);
                figureImageFIIndexList.Add(fiIndex);
                figureImagePosXList.Add(pos.x);
                figureImagePosYList.Add(pos.y);
                figureImageScaleXList.Add(scale.x);
                figureImageScaleYList.Add(scale.y);
            }
            storyRecord.figureImageKeyList = figureImageKeyList;
            storyRecord.figureImageFIIndexList = figureImageFIIndexList;
            storyRecord.figureImagePosXList = figureImagePosXList;
            storyRecord.figureImagePosYList = figureImagePosYList;
            storyRecord.figureImageScaleXList = figureImageScaleXList;
            storyRecord.figureImageScaleYList = figureImageScaleYList;
            storyRecord.smallFigureImageIndex = renderManager.SmallFigureImageIndex;
            storyRecord.choiceItemList = renderManager.ChoiceItemList;

            storyRecord.backlogItemList = backlogManager.BacklogItemList;
            storyRecord.capacity = backlogManager.Capacity;
            storyRecord.count = backlogManager.Count;
            storyRecord.head = backlogManager.Head;

            recordManager.SaveStoryRecord(indexOfRecord, storyRecord);
        }

        public void LoadPlayerRecord() {
            Debug.Log("读取玩家记录!");
            PlayerRecord playerRecord = recordManager.PlayerRecord;
            markManager.LoadPlayerRecord(playerRecord.markPlayerList, playerRecord.varPlayerNameList, playerRecord.varPlayerValueList);
            pastScriptManager.LoadPlayerRecord(playerRecord.pastScriptNameList, playerRecord.pastScriptLineNumberList);
        }

        public void LoadStoryRecord(int indexOfRecord) {
            Dictionary<int, StoryRecord> storyRecordDict = recordManager.StoryRecordDict;
            StoryRecord sr = null;

            if (storyRecordDict.ContainsKey(indexOfRecord)) { 
                sr = storyRecordDict[indexOfRecord];
                Debug.Log("读取故事记录! :" + indexOfRecord);
            } else {
                Debug.LogWarning("不能读取故事存档 :" + indexOfRecord);
                return;
            }

            stateMachine.LoadStoryRecord(sr.currentStateName, sr.LastStateName, sr.StateBuff);
            musicManager.LoadStoryRecord(sr.bgmIndex, sr.voiceIndex, sr.voiceCharacterName);
            scriptManager.LoadStoryRecord(sr.scriptPointerScriptName, sr.scriptPointerLineNumber,
                sr.scriptReplaceKeys, sr.scriptReplaceValues,
                sr.pointerScriptNameStack, sr.pointerLineNumberStack);
            markManager.LoadStoryRecord(sr.markStoryList, sr.varStoryNameList, sr.varStoryValueList, sr.chapterName);
            backlogManager.LoadStoryRecord(sr.backlogItemList, sr.head, sr.capacity, sr.count);
            //renderManager.LoadStoryData 负责Choice Backlog Image Text
            renderManager.LoadStoryRecord(sr);
        }


        public void InitializeStory(string storyScriptFileName = null) {
			if (storyScriptFileName != null)
				this.storyScriptFileName = storyScriptFileName;

            musicManager.InitializeStory();
            scriptManager.InitializeStory();
            markManager.InitializeStory();
            backlogManager.InitializeStory();
            //renderManager.LoadStoryData 负责Choice Backlog Image Text
            renderManager.InitializeStory();
        }

        public void FinalizeStory() {
            stateMachine.FinalizeStory();
            musicManager.FinalizeStory();
            scriptManager.FinalizeStory();
            markManager.FinalizeStory();
            backlogManager.FinalizeStory();
            renderManager.FinalizeStory();
        }
    }
}