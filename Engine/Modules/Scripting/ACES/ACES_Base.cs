namespace CairoEngine.Scripting
{
    /// <summary>
    /// Base Class for all ACES Nodes
    /// </summary>
    public class ACES_Base
    {
        /// <summary>
        /// The Event sheet that contains this ACES Node
        /// </summary>
        public EventSheet eventSheet;
        /// <summary>
        /// The Parent Node Class/Event of this ACES Node
        /// </summary>
        public Block block;
        /// <summary>
        /// Asset Methods can be imported in place of Typical Conditions, Actions, and Triggers
        /// </summary>
        public AssetMethod assetMethod;
        /// <summary>
        /// The Instance of the Node, used for performing the
        /// </summary>
        public object ACEinstance;
        /// <summary>
        /// The Parameters of the Node
        /// </summary>
        public SDictionary<string, object> parameters = new SDictionary<string, object>();
    }
}
