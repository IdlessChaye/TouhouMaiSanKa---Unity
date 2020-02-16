using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class MusicBGMStopNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("MusicStop");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() {
            if (paraList.Count != 0)
                throw new System.Exception("EngineScriptLoadFileNode");

            StateBuff stateBuff = PachiGrimoire.I.StateMachine.StateBuff;
            if (stateBuff == StateBuff.Next) {
                PachiGrimoire.I.MusicManager.BGMStop(false);
            } else {
                PachiGrimoire.I.MusicManager.BGMStop();
            }
        }



    }
}