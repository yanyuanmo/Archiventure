using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Archiventure {
    
    [System.Serializable]
    public class StructureData
    {
        public BuildingType buildingType;
        public Vector3 position;
        public int level;
    }

    [System.Serializable]
    public enum BuildingType {
        cityWall,
        decorationBuilding,
        house,
        leisureStore
    }  
}

