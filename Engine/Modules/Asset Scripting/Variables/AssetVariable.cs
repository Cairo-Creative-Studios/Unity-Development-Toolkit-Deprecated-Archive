using System;
namespace CairoEngine
{
    [Serializable]
    public class AssetVariable<T>
    {
        public string name;
        public T value;
    }
}
