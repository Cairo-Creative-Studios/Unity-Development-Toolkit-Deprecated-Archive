//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using UnityEngine.SceneManagement;

namespace CairoEngine.Scripting
{
    /// <summary>
    /// Picks all Game Objects within the Scene
    /// </summary>
    public class Condition_PickAll : ACES_Base
    {
        public override void Init()
        {
            types.Add(typeof(System));
            base.Init();
        }

        public bool Perform()
        {
            block.selectedObjects.AddRange(SceneManager.GetActiveScene().GetRootGameObjects());
            return true;
        }
    }
}
