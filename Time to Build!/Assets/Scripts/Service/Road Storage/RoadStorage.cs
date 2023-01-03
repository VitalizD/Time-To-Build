using System.Collections.Generic;
using UnityEngine;

namespace Service.RoadStorage
{
    public class RoadStorage : MonoBehaviour
    {
        [SerializeField] private Road[] _roads;

        private Dictionary<RoadType, GameObject[]> _roadDictionary = new();

        public GameObject GetRoad(RoadType type)
        {
            if (_roadDictionary.ContainsKey(type))
            {
                var roads = _roadDictionary[type];
                return roads[Random.Range(0, roads.Length)];
            }
            throw new System.Exception($"ƒорога типа {type} отсутствует в хранилище");
        }

        private void Awake()
        {
            foreach (var road in _roads)
                _roadDictionary.Add(road.Type, road.Prefabs);
        }
    }
}
