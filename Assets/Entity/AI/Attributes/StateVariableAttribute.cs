using System;
using UnityEngine;

namespace HTN
{
    public class StateVariableAttribute : PropertyAttribute
    {
        public Type TargetType { get; private set; }

        public StateVariableAttribute(Type targetType = null)
        {
            TargetType = targetType;
        }
    }
}
