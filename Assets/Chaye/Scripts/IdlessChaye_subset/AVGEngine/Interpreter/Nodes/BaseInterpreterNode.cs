using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public abstract class BaseInterpreterNode {
        public abstract void Interpret(ScriptSentenceContext context);
        public abstract void Execute();
    }
}
