using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archiventure
{
    [System.Serializable]
    public class SaveData
    {
        public List<BuildingData> buildings = new List<BuildingData>();
        //public ResourceManager.ResourceData resources;
        public float gold = 4000f; 
        public int population = 0;  
    }
}