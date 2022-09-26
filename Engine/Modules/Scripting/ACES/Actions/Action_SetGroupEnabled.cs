//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using UnityEngine;

namespace CairoEngine.Scripting
{
    /// <summary>
    /// Moves the Selected Objects forward by the given Amount
    /// </summary>
    public class Action_SetGroupEnabled : ACES_Base
    {
        public override void Init()
        {
            types.Add(typeof(System));

            parameters.Add("Group Name", "");
            parameters.Add("State", true);

            base.Init();
        }

        public void Perform()
        {
            eventSheet.groups[(string)parameters["Group Name"]] = (bool)parameters["State"];
        }
    }
}
