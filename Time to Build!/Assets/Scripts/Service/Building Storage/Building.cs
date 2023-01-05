using System;
using UI.InformationWindow;
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
        public float BuildingTime;
        public BonusInfo[] InstantBonuses;
        public Property[] Properties;
    }
}