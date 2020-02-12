using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineValueStoryGetNode : FunNode {

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("ValueStoryGet");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() {
            if (paraList.Count != 1)
                throw new System.Exception("EngineValueStoryGetNode");

            string name = paraList[0];
            PachiGrimoire.I.MarkManager.ValueStoryGet(name);
        }


    }
}