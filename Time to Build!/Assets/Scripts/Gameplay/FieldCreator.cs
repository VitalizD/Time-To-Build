using Gameplay.Buildings;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class FieldCreator : MonoBehaviour
    {
        [SerializeField] private Vector2 _size;
        [SerializeField]
        [Range(0.01f, 1f)] private float _obstacleChance;
        [SerializeField] private int _obstacleCount;
        [SerializeField] private Transform _startGenPoint;
        [SerializeField] private GameObject _buildingAreaPrefab;
        [SerializeField] private Transform[] _cityCenterPoints;

        public static event System.Func<int, List<BuildingArea>> GetRandomBuildingSites;

        private void Start()
        {
            GenerateCityCenter();
            GenerateObstacles(_obstacleCount);
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
        }

        private void GenerateBuildings(BuildingType[] buildingTypes)
        {
            var buildingSites = GetRandomBuildingSites?.Invoke(buildingTypes.Length);
            for (var i = 0; i < buildingTypes.Length; ++i)
            {
                buildingSites[i].Build(buildingTypes[i]);
            }
        }

        private void GenerateObstacles(int count)
        {
            var generatedObstacleCount = 0;
            while (generatedObstacleCount < count)
            {
                for (var z = _startGenPoint.position.z; z > _startGenPoint.position.z - _size.y; --z)
                {
                    for (var x = _startGenPoint.position.x; x < _startGenPoint.position.x + _size.x; ++x)
                    {
                        var spawnPoint = new Vector3(x, _startGenPoint.position.y, z);
                        var buildingArea = AdjacentBuildings.GetBuildingArea(spawnPoint);
                        if (buildingArea != null || Random.Range(0f, 1f) > _obstacleChance)
                            continue;

                        var buildingSite = Instantiate(_buildingAreaPrefab, spawnPoint, Quaternion.identity)
                            .GetComponent<BuildingArea>();
                        buildingSite.Build(BuildingType.Obstacle);
                        ++generatedObstacleCount;
                    }
                }
            }
        }
    }
}