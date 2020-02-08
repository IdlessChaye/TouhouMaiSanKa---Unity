using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ExpressionRootNode : BaseInterpreterNode {
        private List<BaseInterpreterNode> nodeList = new List<BaseInterpreterNode>();

        public override void Execute() {
            for(int i = 0;i < nodeList.Count; i++) {
                BaseInterpreterNode node = nodeList[i];
                node.Execute();
            }
        }

        public override void Interpret(ScriptSentenceContext context) {
            while(true) {
                if (context.CurrentToken == null)
                    break;
                BaseInterpreterNode node = new CommandNode();
                node.Interpret(context);
                nodeList.Add(node);
            }
        }
    }
}