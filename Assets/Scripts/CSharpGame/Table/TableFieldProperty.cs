using System;
using System.Reflection;

public class TableFieldProperty
{
    public string ItemName { get; set; }
    public Type ItemType { get; set; }
    public MethodInfo Method { get; set; }
}
