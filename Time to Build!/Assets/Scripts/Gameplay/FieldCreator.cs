using Gameplay.Buildings;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class FieldCreator : MonoBehaviour
    {
        [SerializeField] private Vector2 _size;
        [SerializeField]
        [Range(0f, 1f)] private float _obstacleChance;
        [SerializeField] private int _obstacleCount;
        [SerializeField] private Transform _startGenPoint;
        [SerializeField] private GameObject _buildingAreaPrefab;
        [SerializeField] private Transform[] _cityCenterPoints;
        [SerializeField] private GameObject[] _obstaclePrefabs;

        public static event System.Func<int, List<BuildingArea>> GetRandomBuildingSites;

        private void Start()
        {
            GenerateCityCenter();
        }

        private void GenerateCityCenter()
        {
            var point = _cityCenterPoints[Random.Range(0, _cityCenterPoints.Length)];
            var buildingArea = Instantiate(_buildingAreaPrefab, point.position, Quaternion.identity)
                .GetComponent<BuildingArea>();
            buildingArea.Build(BuildingType.Road);
            GenerateRoads(Random.Range(2, 4));
            GenerateBuildings(new[] { BuildingType.LittleHouse, BuildingType.Factory, BuildingType.Park });
        }

        private void GenerateRoads(int count)
        {
            for (var i = 0; i < count; ++i)
            {
                var buildingSite = GetRandomBuildingSites?.Invoke(1)[0];
                buildingSite.Build(BuildingType.Road);
            }
            
            //for (var i = 0; i < 3; ++i)
            //{
            //    var buildingArea = Instantiate(_buildingAreaPrefab, point.position, Quaternion.identity)
            //        .GetComponent<BuildingArea>();
            //    buildingArea.Build(BuildingType.Road);
            //    var state = Random.Range(0, 4);
            //    if (state == 0 || state == 1)
            //    {
            //        point.position = new Vector3(point.position.x + (state == 0 ? -BuildingArea.CellSize : BuildingArea.CellSize),
            //            point.position.y, point.position.z);
            //    }
            //    else if (state == 2 || state == 3)
            //    {
            //        point.position = new Vector3(point.position.x,
            //            point.position.y, point.position.z + (state == 2 ? -BuildingArea.CellSize : BuildingArea.CellSize));
            //    }
            //}
        }

        private void GenerateBuildings(BuildingType[] buildingTypes)
        {
            var buildingSites = GetRandomBuildingSites?.Invoke(buildingTypes.Length);
            for (var i = 0; i < buildingTypes.Length; ++i)
            {
                buildingSites[i].Build(buildingTypes[i]);
            }
        }
    }
}