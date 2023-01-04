using System;
using UnityEngine;

namespace Service.BuildingStorage
{
    [Serializable]
    public class Building
    {
        public BuildingType Type;
        public ZoneType Zone;
        public GameObject Prefab;
        public Sprite Icon;
        public int Cost;
    }
}