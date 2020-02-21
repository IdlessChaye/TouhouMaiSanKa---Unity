using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class CommandTextNode : BaseInterpreterNode {
        private List<BaseInterpreterNode> nodeList = new List<BaseInterpreterNode>();

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("Text");
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
            if (token.Equals("Clear")) {
                node = new TextClearNode();
            } else if (token.Equals("Change")) {
                node = new TextChangeNode();
            } else {
                canParse = false;
            }

            if (canParse) {
                nodeList.Add(node);
                node.Interpret(scriptSentenceContext);
            } else {
                Debug.Log("CommandTextNode FALSE canParse! token :" + token);
            }

            return canParse;
        }

    }
}