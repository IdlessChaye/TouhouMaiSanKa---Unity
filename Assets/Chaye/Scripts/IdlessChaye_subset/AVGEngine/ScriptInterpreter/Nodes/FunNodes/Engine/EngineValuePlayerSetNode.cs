using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineValuePlayerSetNode : FunNode {

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("ValuePlayerSet");
            InterpretPart(context);
        }



        protected override void OnUpdateStageContext() {
            if (paraList.Count != 2)
                throw new System.Exception("EngineValuePlayerSetNode");

            string name = paraList[0];
            float value = float.Parse(paraList[1]);
            PachiGrimoire.I.MarkManager.ValuePlayerSet(name,value);
        }


    }
}