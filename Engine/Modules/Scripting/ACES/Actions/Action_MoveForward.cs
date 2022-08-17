namespace CairoEngine.Scripting
{
    public class Action_MoveForward : ACES_Base
    {
        public void Init()
        {
            parameters.Add("Distance", 0);
        }

        public void Perform()
        {
            foreach (CObject instance in block.selectedObjects)
            {
                instance.transform.position += instance.transform.forward * (float)parameters["Distance"];
            }
        }
    }
}
