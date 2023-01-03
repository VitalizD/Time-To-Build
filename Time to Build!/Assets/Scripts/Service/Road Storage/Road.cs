using System;
using UnityEngine;

namespace Service.RoadStorage
{
    [Serializable]
    public class Road
    {
        public RoadType Type;
        public GameObject[] Prefabs;
    }
}