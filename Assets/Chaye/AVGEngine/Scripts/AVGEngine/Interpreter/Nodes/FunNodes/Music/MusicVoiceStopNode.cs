using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class MusicVoiceStopNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("VoiceStop");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() {
            if (paraList.Count != 0)
                throw new System.Exception("MusicVoicePlayNode");

            StateBuff stateBuff = PachiGrimoire.I.StateMachine.StateBuff;
            if (stateBuff == StateBuff.Next) {
                PachiGrimoire.I.MusicManager.VoiceStop(false);
            } else {
                PachiGrimoire.I.MusicManager.VoiceStop();
            }
        }


    }
}