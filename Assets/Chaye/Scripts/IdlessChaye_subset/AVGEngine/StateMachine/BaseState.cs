using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public abstract class BaseState {
        public virtual uint StateID { get; }
        public virtual string StateName => throw new System.Exception("你还没起名呢");
        public virtual List<uint> RejectedOldStateIDList { get; }

        public virtual void OnEnter(BaseState oldState) {
            Debug.Log("OldStateID : " + oldState.StateName);
        }
        public virtual void OnExit(BaseState newState) {
            Debug.Log("NewStateID : " + newState.StateName);
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

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 1, 2, 4, 6, 7, 8, 9, 10 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;

        public override void OnEnter(BaseState oldState) {
            Debug.Log("OldState: " + oldState.StateName);
        }

        public override void OnExit(BaseState newState) {
            Debug.Log("NewState: " + newState.StateName);
        }
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

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;

        public override void OnEnter(BaseState oldState) {
            PachiGrimoire.I.FileManager.LoadConfig();
            PachiGrimoire.I.FileManager.LoadPlayerRecord();
            PachiGrimoire.I.FileManager.LoadStoryRecords();
            PachiGrimoire.I.FileManager.LoadVoiceAssetBundleConfig();
            PachiGrimoire.I.FileManager.LoadResource();
        }

        public override void OnExit(BaseState newState) {
            Debug.Log("NewState: " + newState.StateName);
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

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 1, 3, 4, 6, 7, 8, 9 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;

        public override void OnEnter(BaseState oldState) {
            Debug.Log("OldState: " + oldState.StateName);
        }

        public override void OnExit(BaseState newState) {
            Debug.Log("NewState: " + newState.StateName);
        }
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

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 1, 2, 3, 4, 6, 7, 8, 9, 10 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;

        public override void OnEnter(BaseState oldState) {
            Debug.Log("OldState: " + oldState.StateName);
        }

        public override void OnExit(BaseState newState) {
            Debug.Log("NewState: " + newState.StateName);
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

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 1, 2, 5 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;

        public override void OnEnter(BaseState oldState) {
            Debug.Log("OldState: " + oldState.StateName);
        }

        public override void OnExit(BaseState newState) {
            Debug.Log("NewState: " + newState.StateName);
        }
    }

    public class RunSkipState : BaseState {
        private static RunSkipState instance = new RunSkipState();

        public static RunSkipState Instance {
            get {
                return instance;
            }
        }

        private readonly uint stateID = 6;
        public override uint StateID => stateID;

        public override string StateName => "RunSkipState";

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 1, 2, 3, 4, 6, 7, 8, 9, 10 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;

        public override void OnEnter(BaseState oldState) {
            Debug.Log("OldState: " + oldState.StateName);
        }

        public override void OnExit(BaseState newState) {
            Debug.Log("NewState: " + newState.StateName);
        }
    }

    public class RunAutoState : BaseState {
        private static RunAutoState instance = new RunAutoState();

        public static RunAutoState Instance {
            get {
                return instance;
            }
        }

        private readonly uint stateID = 7;
        public override uint StateID => stateID;

        public override string StateName => "RunAutoState";

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 1, 2, 3, 4, 6, 7, 8, 9, 10 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;

        public override void OnEnter(BaseState oldState) {
            Debug.Log("OldState: " + oldState.StateName);
        }

        public override void OnExit(BaseState newState) {
            Debug.Log("NewState: " + newState.StateName);
        }
    }

    public class RunNextState : BaseState {
        private static RunNextState instance = new RunNextState();

        public static RunNextState Instance {
            get {
                return instance;
            }
        }

        private readonly uint stateID = 8;
        public override uint StateID => stateID;

        public override string StateName => "RunNextState";

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 1, 2, 3, 4, 6, 7, 8, 9, 10 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;

        public override void OnEnter(BaseState oldState) {
            Debug.Log("OldState: " + oldState.StateName);
        }

        public override void OnExit(BaseState newState) {
            Debug.Log("NewState: " + newState.StateName);
        }
    }

    public class RunAnimateState : BaseState {
        private static RunAnimateState instance = new RunAnimateState();

        public static RunAnimateState Instance {
            get {
                return instance;
            }
        }

        private readonly uint stateID = 9;
        public override uint StateID => stateID;

        public override string StateName => "RunAnimateState";

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 1, 2, 3, 4, 6, 7, 8, 9, 10 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;

        public override void OnEnter(BaseState oldState) {
            Debug.Log("OldState: " + oldState.StateName);
        }

        public override void OnExit(BaseState newState) {
            Debug.Log("NewState: " + newState.StateName);
        }
    }

    public class SleepState : BaseState {
        private static SleepState instance = new SleepState();

        public static SleepState Instance {
            get {
                return instance;
            }
        }

        private readonly uint stateID = 10;
        public override uint StateID => stateID;

        public override string StateName => "SleepState";

        private readonly List<uint> rejectedOldStateIDList = new List<uint> { 1, 2, 4, 6, 7, 8, 9, 10 };
        public override List<uint> RejectedOldStateIDList => rejectedOldStateIDList;

        public override void OnEnter(BaseState oldState) {
            Debug.Log("OldState: " + oldState.StateName);
        }

        public override void OnExit(BaseState newState) {
            Debug.Log("NewState: " + newState.StateName);
        }
    }
}

