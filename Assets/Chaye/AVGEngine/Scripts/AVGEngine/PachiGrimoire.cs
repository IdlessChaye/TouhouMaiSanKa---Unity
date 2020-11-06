using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {

    [RequireComponent(typeof(StageRenderManager))]
    [RequireComponent(typeof(MusicManager))]
    public sealed class PachiGrimoire : MonoBehaviour {
        #region Singleton
        private static PachiGrimoire instance;
        public static PachiGrimoire I {
            get {
                if (instance == null) {
                    instance = FindObjectOfType(typeof(PachiGrimoire)) as PachiGrimoire;
                    if (instance == null) {
                        GameObject go = Instantiate(Resources.Load<GameObject>("AVGEngine/PachiGrimoire"));
                        go.name = "AVGEngine";
                        go.transform.position = Vector3.zero;
                        instance = go.GetComponent<PachiGrimoire>();
                    }
                }
                return instance;
            }
        }

        public void Awake() {
            DontDestroyOnLoad(gameObject);
            if (instance == null) {
                instance = this;
            } else {
                if (instance != this) {
                    Debug.Log("????");
                    Destroy(gameObject);
                }
            }

            Initialize();
        }
        #endregion
        public bool isShutDown;


        public ConstData constData;
        #region Public Manager Properties
        public StateMachineManager StateMachine => stateMachine;
        public FileManager FileManager => fileManager;
        public ResourceManager ResourceManager => resourceManager;
        public ConfigManager ConfigManager => configManager;
        public PlayerRecordManager PlayerRecordManager => playerRecordManager;
        public StageContextManager StageContextManager => stageContextManager;
        public MarkManager MarkManager => markManager;
        public PastScriptManager PastScriptManager => pastScriptManager;
        public BacklogManager BacklogManager => backlogManager;
        public ScriptManager ScriptManager => scriptManager;
        public StageRenderManager StageRenderManager => stageRenderManager;
        public SaveLoadRenderManager SaveLoadRenderManager => saveLoadRenderManager;
        public MusicManager MusicManager => musicManager;
        #endregion
        #region Private Manager Properties
        private StateMachineManager stateMachine = new StateMachineManager(VoidState.Instance);
        private FileManager fileManager;
        private ResourceManager resourceManager = new ResourceManager();
        private ConfigManager configManager = new ConfigManager();
        private PlayerRecordManager playerRecordManager = new PlayerRecordManager();
        private MarkManager markManager = new MarkManager();
        private PastScriptManager pastScriptManager = new PastScriptManager();
        private SaveLoadRenderManager saveLoadRenderManager;
        private StageContextManager stageContextManager;
        private BacklogManager backlogManager;
        private ScriptManager scriptManager;
        private StageRenderManager stageRenderManager;
        private MusicManager musicManager;
        #endregion

        private bool isFirstInAutoBuff;
        private float autoWaitTime;
        private float autoStartTime;


        private void Initialize() {
            if (isShutDown)
                return;
            fileManager = new FileManager(configManager, playerRecordManager, resourceManager, constData);
            scriptManager = new ScriptManager(pastScriptManager);
            backlogManager = new BacklogManager(constData.BacklogCapacity);
            stageRenderManager = GetComponent<StageRenderManager>() ?? gameObject.AddComponent<StageRenderManager>();
            saveLoadRenderManager = GetComponent<SaveLoadRenderManager>() ?? gameObject.AddComponent<SaveLoadRenderManager>();
            musicManager = GetComponent<MusicManager>() ?? gameObject.AddComponent<MusicManager>();
            stageContextManager = new StageContextManager();

            #region Test
            //configManager.Config.Language = "Chinese";
            //configManager.Config.CharacterNameList = new List<string>(new string[] { "A", "B", "C", "ETC" });
            //configManager.Config.SystemVolume = 1f;
            //configManager.Config.BGMVolume = 1f;
            //configManager.Config.SEVolume = 1f;
            //configManager.Config.MessageSpeed = 1f;
            //configManager.Config.AutoMessageSpeed = 1f;
            //configManager.Config.IsReadSkipOrAllSkipNot = true;
            //configManager.Config.VoiceVolume = 1f; // Slider
            //configManager.Config.VoiceVolumeValueList = new List<float>(new float[] { 1, 1, 1, 1 }); // Sliders
            //configManager.Config.IsPlayingVoiceAfterChangeLine = false; // Toggle
            //configManager.Config.HasAnimationEffect = true; // Toggle
            //configManager.Config.AlphaOfConsole = 0.5f;
            //configManager.SaveConfigContext();
            #endregion


            StateMachine.TransferStateTo(InitState.Instance);
        }

        private void Update() {

			#region Test
			if (Input.GetKeyDown(constData.KeyConfirm))
			{
				StartGame();
			}
			else if (Input.GetKeyDown(KeyCode.T))
			{ 
				StartGameTest();
			}
			#endregion



			#region State
			BaseState state = stateMachine.CurrentState;

            if (state == RunScriptState.Instance) { // RunScript State
                bool success = scriptManager.NextSentence();
                if (success == false) {
					Debug.LogWarning("脚本执行结束");
					if (stageRenderManager.IsShow == true)
					{
						stageRenderManager.OnHide();
						FinalizeGame();
					}
				}
            } else if (state == RunWaitState.Instance) { // RunWait State
                StateBuff stateBuff = stateMachine.StateBuff;
                if (stateBuff == StateBuff.Normal) {
                    // Pass
                } else if (stateBuff == StateBuff.Auto) {  // Auto Buff
                    if (isFirstInAutoBuff == false) {
                        isFirstInAutoBuff = true;
                        float autoMessageSpeed = ConfigManager.Config.AutoMessageSpeed;
                        float highestTime = constData.AutoMessageSpeedHighest;
                        float lowestTime = constData.AutoMessageSpeedLowest;
                        autoWaitTime = (highestTime - lowestTime) * autoMessageSpeed + lowestTime;
                        autoStartTime = Time.time;
                    }
                    if (Time.time - autoStartTime >= autoWaitTime) {
                        stateMachine.TransferStateTo(RunScriptState.Instance);
                        isFirstInAutoBuff = false;
                    }
                } else if (stateBuff == StateBuff.Skip) { // Skip Buff
                    if (configManager.Config.IsReadSkipOrAllSkipNot) {
                        if (PastScriptManager.HasRead(ScriptManager.ScriptPointerScriptName, scriptManager.ScriptPointerLineNumber)) {
                            stateMachine.TransferStateTo(RunScriptState.Instance);
                        } else {
                            stateMachine.SetStateBuff(StateBuff.Normal);
                        }
                    } else {
                        stateMachine.TransferStateTo(RunScriptState.Instance);
                    }
                } else if (stateBuff == StateBuff.Next) { // Next Buff
                    stateMachine.TransferStateTo(RunScriptState.Instance);
                }
            }


            #endregion
        }



        #region AVGEngine Layer

        public void StartGame() { // 初始界面入口，初始化，还有UI管理，这个函数么有写完
            if (stageRenderManager.IsShow == false) {
                stageContextManager.InitializeStory(constData.MainScriptFileNameWithoutTXT);
                stateMachine.TransferStateTo(RunScriptState.Instance);
                stateMachine.SetStateBuff(StateBuff.Normal);
                stageRenderManager.OnShow(null);
            }
        }

		public void StartGameTest()
		{ // 测试别的故事脚本用的系统初始化代码
			if (stageRenderManager.IsShow == false)
			{
				stageContextManager.InitializeStory("GameStartTest");
				stateMachine.TransferStateTo(RunScriptState.Instance);
				stateMachine.SetStateBuff(StateBuff.Normal);
				stageRenderManager.OnShow(null);
			}
		}


		public void LoadGame() { // 初始界面读取存档入口，初始化，还有UI管理，这个函数么有写完
            BaseState currentState = stateMachine.CurrentState;
            if (currentState == IdleState.Instance) {
                if (saveLoadRenderManager.IsShow == false) {
                    stateMachine.TransferStateTo(RunScriptState.Instance);
                    //stageContextManager.LoadStoryRecord();
                    saveLoadRenderManager.IsSaveMode = false;
                    saveLoadRenderManager.OnShow(null);
                }
            }
        }

        public void FinalizeGame() { // 全系统出口，推出清理，还有UI管理，这个函数么有写完
            stageContextManager.FinalizeStory();
            stateMachine.TransferStateTo(IdleState.Instance);
        }

        #endregion



        #region Commands
        public void DebugLog(string context) {
            Debug.Log(context);
        }
        #endregion

    }
}