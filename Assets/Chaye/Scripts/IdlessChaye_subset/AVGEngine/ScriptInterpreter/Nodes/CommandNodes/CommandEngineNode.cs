using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class CommandEngineNode : BaseInterpreterNode {
        private List<BaseInterpreterNode> nodeList = new List<BaseInterpreterNode>();

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("Engine");
            while (true) {
                string funcToken = context.CurrentToken;
                if (funcToken == null)
                    break;

                if (!CanParse(funcToken, context))
                    break;
            }
        }

        public override void Execute() {
            for (int i = 0; i < nodeList.Count; i++) {
                BaseInterpreterNode node = nodeList[i];
                node.Execute();
            }
        }

        private bool CanParse(string token, ScriptSentenceContext scriptSentenceContext) {
            bool canParse = true;

            BaseInterpreterNode node = null;
            if (token.Equals("ScriptReplace")) {
                node = new EngineScriptReplaceAddNode();
            } else if (token.Equals("ScriptIfThenElse")) {
                node = new EngineScriptIfThenElseNode();
            } else if (token.Equals("ScriptLoadFile")) {
                node = new EngineScriptLoadFileNode();
            } else {
                canParse = false;
            }

            if (canParse) {
                nodeList.Add(node);
                node.Interpret(scriptSentenceContext);
            }

            return canParse;
        }

    }
}