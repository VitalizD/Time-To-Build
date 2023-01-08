
using System;
using UI.InformationWindow;
using UnityEngine;

namespace Service.BuildingStorage
{
    [Serializable]
    public class Building
    {
        public BuildingType Type;
        public BuildingLevel Level;
        public ZoneType Zone;
        public GameObject Prefab;
        public Sprite Icon;
        public int Cost;
        public float BuildingTime;
        public int Reserve = 1;
        public int DaysForRefill = 1;
        public BonusInfo[] InstantBonuses;
        public Property[] Properties;
    }
}