using System;
using UnityEngine;

namespace HTN
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SubclassSelectorAttribute : PropertyAttribute
    {
        public bool IncludeAbstract { get; private set; }

        public SubclassSelectorAttribute(bool includeAbstract = false)
        {
            IncludeAbstract = includeAbstract;
        }
    }
}
