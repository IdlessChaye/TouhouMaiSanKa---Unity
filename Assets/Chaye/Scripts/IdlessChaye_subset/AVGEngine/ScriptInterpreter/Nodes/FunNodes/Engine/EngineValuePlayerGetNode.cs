using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineValuePlayerGetNode : FunNode {

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("ValuePlayerGet");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() {
            if (paraList.Count != 1)
                throw new System.Exception("EngineValuePlayerGetNode");

            string name = paraList[0];
            PachiGrimoire.I.MarkManager.ValuePlayerGet(name);
        }


    }
}