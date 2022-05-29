using System;

[Serializable]
public class SerializableObject
{
    public enum ObjectType
    {
        String,
        Integer,
        Float,
        Double,
    }
    public ObjectType objectType = 0;
    public string name;
    public string value;
}