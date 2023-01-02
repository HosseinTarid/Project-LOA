using System;
using UnityEngine;

namespace Utility.CommonData
{
    [Serializable]
    public struct DescriptionStats
    {
        public string name;
        public string secondaryName;

        [TextArea] public string description;

        [TextArea] public string shortDescription;
    }
}