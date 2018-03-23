using UnityEngine;
using System;

public class RequireInterfaceAttribute : PropertyAttribute
{
    public Type type;

    public RequireInterfaceAttribute(Type type)
    {
        this.type = type;
    }
}
