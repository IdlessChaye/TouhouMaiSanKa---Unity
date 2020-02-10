using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    [RequireComponent(typeof(StageRenderManager))]
    public sealed class PachiGrimoire : MonoBehaviour {
        #region Singleton
        private static PachiGrimoire instance;
        public static PachiGrimoire I {
            get {
                if (instance == null) {
                    instance = FindObjectOfType(typeof(PachiGrimoire)) as PachiGrimoire;
                    if (instance == null) {
                        instance = ((GameObject)Instantiate(Resources.Load<GameObject>("AVGEngine/PachiGrimoire"))).GetComponent<PachiGrimoire>();
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
                Destroy(gameObject);
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
        #endregion
        #region Private Manager Properties
        private StateMachineManager stateMachine = new StateMachineManager(VoidState.Instance);
        private FileManager fileManager;
        private ResourceManager resourceManager = new ResourceManager();
        private ConfigManager configManager = new ConfigManager();
        private PlayerRecordManager playerRecordManager = new PlayerRecordManager();
        private StageContextManager stageContextManager = new StageContextManager();
        private MarkManager markManager = new MarkManager();
        private PastScriptManager pastScriptManager = new PastScriptManager();
        private BacklogManager backlogManager;
        private ScriptManager scriptManager;
        private StageRenderManager stageRenderManager;
        #endregion



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
            stageRenderManager = GetComponent<StageRenderManager>() ?? gameObject.AddComponent<StageRenderManager>();
            backlogManager = new BacklogManager(constData.BacklogCapacity);

            //configManager.Config.PlayerIdentifier = "0xFFFFFFFF";
            //configManager.Config.Language = "Chinese";
            //configManager.SaveConfigContext();
            //playerRecordManager.PlayerRecord.markList = new List<string>();
            //playerRecordManager.PlayerRecord.markList.Add("呜呜呜~~");
            //playerRecordManager.SavePlayerRecord();
            StateMachine.TransferStateTo(InitState.Instance);

            //StartCoroutine(TestGetBgImage());

            if (isDebugMode) {
                text.text = configManager.Config.PlayerIdentifier;
                text.text = playerRecordManager.PlayerRecord.markList[0];
            }
        }

        private void Update() {
            #region Input
            if (Input.GetKeyDown(constData.KeyConfirm)) {
                OnKeyConfirmDown();
            }
            #endregion

            #region State
            BaseState state = stateMachine.CurrentState;
            if (state == RunScriptState.Instance) {
                scriptManager.NextSentence();
            }
            #endregion
        }

        private void OnKeyConfirmDown() {
            BaseState state = stateMachine.CurrentState;
            if (state == RunWaitState.Instance) {
                stateMachine.TransferStateTo(RunScriptState.Instance);
            }
        }




    }
}