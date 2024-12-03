using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archiventure
{
    [System.Serializable]
    public class SaveData
    {
        public List<BuildingData> buildings = new List<BuildingData>();
    }
}