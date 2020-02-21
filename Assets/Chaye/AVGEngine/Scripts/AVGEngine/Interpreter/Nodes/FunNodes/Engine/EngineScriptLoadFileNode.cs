using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineScriptLoadFileNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("ScriptLoadFile");
            InterpretPart(context);
        }


        protected override void OnUpdateStageContext() {
            if (paraList.Count != 1)
                throw new System.Exception("EngineScriptLoadFileNode");
            string fileIndex = paraList[0]; // "SC_ScriptName"
            string scriptName = fileIndex.Substring(fileIndex.IndexOf('_') + 1);
            string scriptContext = PachiGrimoire.I.ResourceManager.Get<string>(fileIndex);
            PachiGrimoire.I.ScriptManager.LoadScriptFile(scriptName,scriptContext);
        }

    }
}