namespace CairoEngine.Scripting
{
    /// <summary>
    /// Picks all Cairo Objects within the Level
    /// </summary>
    public class Condition_Pick_All : ACES_Base
    {
        public bool Perform()
        {
            block.selectedObjects = LevelModule.currentLevel.childInstances;
            return true;
        }
    }
}
