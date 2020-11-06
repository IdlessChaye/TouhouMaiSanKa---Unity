using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    [System.Serializable]
    public enum StateBuff {
        Normal,
        Auto,
        Skip,
        Next
    }
    public abstract class BaseState {
        
        public abstract uint StateID { get; }
        public abstract string StateName { get; }
        public abstract List<uint> RejectedOldStateIDList { get; }

        public virtual void OnEnter(BaseState oldState) {
            Debug.Log("新状态进入，旧状态名 :" + oldState.StateName);
        }
        public virtual void OnExit(BaseState newState) {
            Debug.Log("旧状态退出，新状态名 :" + newState.StateName);
        }
        public virtual void OnHasEntered() {
            Debug.Log("状态转换完毕 :" + StateName);
        }
        public bool CanBeTransferedFrom(BaseState oldState) {
            return !RejectedOldStateIDList.Contains(oldState.StateID);
        }
    }
    
    public class VoidState : BaseState {
        private static VoidState instance = new VoidState();
        public static VoidState Instance {
            get {
                return instance;
            }
        }

        private readonly uint stateID = 1;
        public override uint StateID => stateID;

        public override string StateName => "VoidState";

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 1 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;

    }

    public class InitState : BaseState {
        private static InitState instance = new InitState();

        public static InitState Instance {
            get {
                return instance;
            }
        }

        private readonly uint stateID = 2;
        public override uint StateID => stateID;

        public override string StateName => "InitState";

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 2 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;

        public override void OnEnter(BaseState oldState) {
            if (oldState == VoidState.Instance) { 
                PachiGrimoire.I.FileManager.LoadConfig();
                PachiGrimoire.I.FileManager.LoadPlayerRecord();
                PachiGrimoire.I.FileManager.LoadStoryRecords();
                PachiGrimoire.I.FileManager.LoadResource();

                PachiGrimoire.I.StageContextManager.LoadPlayerRecord();
            }
        }

        public override void OnHasEntered() {
            PachiGrimoire.I.StateMachine.TransferStateTo(IdleState.Instance);
        }
    }

    public class IdleState : BaseState {
        private static IdleState instance = new IdleState();

        public static IdleState Instance {
            get {
                return instance;
            }
        }

        private readonly uint stateID = 3;
        public override uint StateID => stateID;

        public override string StateName => "IdleState";

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 1, 3 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;

 
             
        

 
             
        
    }

    public class RunScriptState : BaseState {
        private static RunScriptState instance = new RunScriptState();

        public static RunScriptState Instance {
            get {
                return instance;
            }
        }

        private readonly uint stateID = 4;
        public override uint StateID => stateID;

        public override string StateName => "RunScriptState";

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 4 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;

        public override void OnEnter(BaseState oldState) {
            if(oldState == IdleState.Instance) {
                Debug.Log("状态切换 :IdleState -> RunScriptState");

				string mainScriptContext = PachiGrimoire.I.ResourceManager.Get<string>(PachiGrimoire.I.constData.ScriptIndexPrefix + "_" + PachiGrimoire.I.StageContextManager.StoryScriptFileName);
				PachiGrimoire.I.ScriptManager.LoadScriptFile(PachiGrimoire.I.StageContextManager.StoryScriptFileName, mainScriptContext);
            }
        }


    }

    public class RunWaitState : BaseState {
        private static RunWaitState instance = new RunWaitState();

        public static RunWaitState Instance {
            get {
                return instance;
            }
        }

        private readonly uint stateID = 5;
        public override uint StateID => stateID;

        public override string StateName => "RunWaitState";

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 5};
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;


    }

    public class RunAnimateState : BaseState {
        private static RunAnimateState instance = new RunAnimateState();

        public static RunAnimateState Instance {
            get {
                return instance;
            }
        }

        private readonly uint stateID = 6;
        public override uint StateID => stateID;

        public override string StateName => "RunAnimateState";

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 6 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;


    }

    public class ChoiceWaitState : BaseState {
        private static ChoiceWaitState instance = new ChoiceWaitState();

        public static ChoiceWaitState Instance {
            get {
                return instance;
            }
        }

        private readonly uint stateID = 7;
        public override uint StateID => stateID;

        public override string StateName => "ChoiceWaitState";

        private readonly List<uint> rejectedOldStateIDList = new List<uint> {  };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;


    }

    public class SleepState : BaseState {
        private static SleepState instance = new SleepState();

        public static SleepState Instance {
            get {
                return instance;
            }
        }

        private readonly uint stateID = 8;
        public override uint StateID => stateID;

        public override string StateName => "SleepState";

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 8 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;


    }

}

