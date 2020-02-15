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

        private void LateUpdate() {

            #region State
            BaseState state = stateMachine.CurrentState;
            if(state != RunWaitState.Instance) {
                isFirstInAutoBuff = false;
            }
            if (state == RunScriptState.Instance) {
                scriptManager.NextSentence();
            } else if(state == RunWaitState.Instance) {
                StateBuff stateBuff = stateMachine.StateBuff;
                if(stateBuff == StateBuff.Auto) { 
                    if(isFirstInAutoBuff == false) {
                        isFirstInAutoBuff = true;
                        float autoMessageSpeed = ConfigManager.Config.AutoMessageSpeed;
                        float highestTime = constData.AutoMessageSpeedHighest;
                        float lowestTime = constData.AutoMessageSpeedLowest;
                        autoWaitTime = (lowestTime - highestTime) * autoMessageSpeed + highestTime;
                        autoStartTime = Time.time;
                    }
                    if(Time.time - autoStartTime >= autoWaitTime) {
                        scriptManager.NextSentence();
                    }
                }
            }
            #endregion
        }

        //public void OnPressedButtonStartGame() {

        //}

        //public void OnPressedButtonLoadGame() {

        //}



    }
}