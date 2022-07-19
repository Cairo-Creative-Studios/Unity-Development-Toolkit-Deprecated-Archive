using System;
namespace CairoEngine.Scripting.Triggers
{
    public class Trigger_OnStart : Trigger
    {
        public override void Init(Script script)
        {
            base.Init(script);
            //Add the Trigger to the Script when the Script is Initialized
            script.triggers.Add("OnStart", this);
        }
    }
}
