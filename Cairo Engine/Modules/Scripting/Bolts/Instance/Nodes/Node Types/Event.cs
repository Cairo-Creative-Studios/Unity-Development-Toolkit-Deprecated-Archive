using System;
using System.Collections.Generic;

namespace CairoEngine.Scripting
{
    [Serializable]
    public class Event : Node
    {
        /// <summary>
        /// Whether this is an Or block or not
        /// </summary>
        public bool or = false;
        /// <summary>
        /// The conditions to check against to call the Action
        /// </summary>
        public List<Condition> conditions = new List<Condition>();
        /// <summary>
        /// The Actions that are to be called if the Conditions are met
        /// </summary>
        public List<Action> actions = new List<Action>();
        /// <summary>
        /// The Properties that are being used for the Event
        /// </summary>
        public List<object> properties = new List<object>();

        public override void Init(Script script)
        {
            base.Init(script);
        }

        public override void Run()
        {
            int conditionsTrue = 0;

            foreach(Condition condition in conditions)
            {
                if (condition.Check())
                    conditionsTrue++;
            }

            if (conditionsTrue > 0)
            {
                if (or == true)
                {
                    RunActions();
                    base.Run();
                }
                else
                {
                    if (conditionsTrue == conditions.Count)
                    {
                        RunActions();
                        base.Run();
                    }
                }
            }
        }

        private void RunActions()
        {
            foreach (Action action in actions)
            {
                action.Perform();
            }
        }
    }
}
