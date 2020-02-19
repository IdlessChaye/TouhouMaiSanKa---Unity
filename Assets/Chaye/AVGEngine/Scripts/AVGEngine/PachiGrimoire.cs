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
                if(instance != this) {
                    Debug.Log("????");
                    Destroy(gameObject);
                }
            }

            Initialize();
        }
        #endregion
        public bool isShutDown;
        public bool isDebugMode;
        public UnityEngine.UI.Text text;
        public UITexture t;


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



        IEnumerator TestGetBgImage() {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 100; i++) {
                string name = i.ToString();
                name = "BI_" + name;
                Texture2D tex = resourceManager.Get<Texture2D>(name);
                t.mainTexture = tex;
                yield return new WaitForSeconds(1f);
            }
        }

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



            //configManager.Config.PlayerIdentifier = "0xFFFFFFFF";
            //configManager.Config.Language = "Chinese";
            //configManager.SaveConfigContext();
            //playerRecordManager.PlayerRecord.markList = new List<string>();
            //playerRecordManager.PlayerRecord.markList.Add("呜呜呜~~");
            //playerRecordManager.SavePlayerRecord();
            StateMachine.TransferStateTo(InitState.Instance);

            //StartCoroutine(TestGetBgImage());

            if (isDebugMode) {
                text.text = configManager.Config.Language;
                //text.text = playerRecordManager.PlayerRecord.markPlayerList[0];
            }
        }

        private void FixedUpdate() {

            #region State


            BaseState state = stateMachine.CurrentState;

            if (state == RunScriptState.Instance) { // RunScript State
                scriptManager.NextSentence();
            } else if(state == RunWaitState.Instance) { // RunWait State
                StateBuff stateBuff = stateMachine.StateBuff;
                if(stateBuff == StateBuff.Auto) {  // Auto Buff
                    if(isFirstInAutoBuff == false) {
                        isFirstInAutoBuff = true;
                        float autoMessageSpeed = ConfigManager.Config.AutoMessageSpeed;
                        float highestTime = constData.AutoMessageSpeedHighest;
                        float lowestTime = constData.AutoMessageSpeedLowest;
                        autoWaitTime = (lowestTime - highestTime) * autoMessageSpeed + highestTime;
                        autoStartTime = Time.time;
                    }
                    if(Time.time - autoStartTime >= autoWaitTime) {
                        stateMachine.TransferStateTo(RunScriptState.Instance);
                        isFirstInAutoBuff = false;
                    }
                } else if(stateBuff == StateBuff.Skip) { // Skip Buff
                    if(configManager.Config.IsReadSkipOrAllSkipNot) { 
                        if(PastScriptManager.HasRead(ScriptManager.ScriptPointerScriptName,scriptManager.ScriptPointerLineNumber)) {
                            stateMachine.TransferStateTo(RunScriptState.Instance);
                        } else {
                            stateMachine.SetStateBuff(StateBuff.Normal);
                        }
                    } else {
                        stateMachine.TransferStateTo(RunScriptState.Instance);
                    }
                } else if(stateBuff == StateBuff.Next) { // Next Buff
                    stateMachine.TransferStateTo(RunScriptState.Instance);
                }
            }


            #endregion
        }



        #region AVGEngine Layer

        public void StartGame() { // 初始界面入口，初始化，还有UI管理，这个函数么有写完
            stateMachine.TransferStateTo(RunScriptState.Instance);
            stateMachine.SetStateBuff(StateBuff.Normal);
            stageContextManager.InitializeStory();
            stageRenderManager.OnShow(null);
        }

        public void LoadGame() { // 初始界面读取存档入口，初始化，还有UI管理，这个函数么有写完
            BaseState currentState = stateMachine.CurrentState;
            if(currentState == IdleState.Instance) {
                stateMachine.TransferStateTo(RunScriptState.Instance);
                //stageContextManager.LoadStoryRecord();
                saveLoadRenderManager.IsSaveMode = false;
                saveLoadRenderManager.OnShow(null);
            }
        }

        public void ExitGame() { // 全系统出口，推出清理，还有UI管理，这个函数么有写完
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