using Service;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Gameplay.Buildings
{
    public class AdjacentBuildings : MonoBehaviour
    {
        private const float checkRadius = 0.1f;

        [SerializeField] private GameObject _buildingSiteSpawner;

        private readonly Dictionary<Direction, BuildingArea> _adjacentBuildings = new();
        private readonly Direction[] _sidesFour = { Direction.Left, Direction.Right, Direction.Top, Direction.Bottom };
        private Dictionary<Direction, Vector3> _pointsByDirection;

        public static event Func<Vector3, bool> PointBeyond;

        public static BuildingArea GetBuildingArea(Vector3 point)
        {
            Collider[] colliders = new Collider[5];
            Physics.OverlapSphereNonAlloc(point, checkRadius, colliders);

            foreach (var collider in colliders)
            {
                if (collider == null)
                    continue;
                if (collider.TryGetComponent<BuildingArea>(out var building))
                    return building;
            }
            return null;
        }

        public Dictionary<Direction, BuildingArea> Get8Sides()
        {
            CreateAdjacentBuildingDictionary();
            return _adjacentBuildings;
        }

        public Dictionary<Direction, BuildingArea> Get4Sides()
        {
            CreateAdjacentBuildingDictionary();
            var result = new Dictionary<Direction, BuildingArea>();
            foreach (var adjacent in _adjacentBuildings)
            {
                if (Array.Exists(_sidesFour, element => element == adjacent.Key))
                    result.Add(adjacent.Key, adjacent.Value);
            }
            return result;
        }

        public void CreateBuildingSites()
        {
            var adjacents4 = Get4Sides();
            foreach (var adjacent in adjacents4)
            {
                var spawnPoint = _pointsByDirection[adjacent.Key];
                var beyond = PointBeyond?.Invoke(spawnPoint);
                if (adjacent.Value == null && !beyond.GetValueOrDefault())
                    CreateBuildingArea(spawnPoint);
            }
        }

        private void Awake()
        {
            _pointsByDirection = new Dictionary<Direction, Vector3>
            {
                [Direction.Left] = new(transform.position.x - BuildingArea.CellSize, transform.position.y, transform.position.z),
                [Direction.Right] = new(transform.position.x + BuildingArea.CellSize, transform.position.y, transform.position.z),
                [Direction.Top] = new(transform.position.x, transform.position.y, transform.position.z + BuildingArea.CellSize),
                [Direction.Bottom] = new(transform.position.x, transform.position.y, transform.position.z - BuildingArea.CellSize)
            };
        }

        private void CreateBuildingArea(Vector3 point)
        {
            var spawner = Instantiate(_buildingSiteSpawner, point, Quaternion.identity, transform.parent);
            spawner.GetComponent<BuildingSiteSpawner>().Spawn();
            Destroy(spawner);
        }

        private void CreateAdjacentBuildingDictionary()
        {
            var pos = transform.position;
            _adjacentBuildings.Clear();
            _adjacentBuildings.Add(Direction.Left, GetBuildingArea(_pointsByDirection[Direction.Left]));
            _adjacentBuildings.Add(Direction.Right, GetBuildingArea(_pointsByDirection[Direction.Right]));
            _adjacentBuildings.Add(Direction.Top, GetBuildingArea(_pointsByDirection[Direction.Top]));
            _adjacentBuildings.Add(Direction.Bottom, GetBuildingArea(_pointsByDirection[Direction.Bottom]));
            _adjacentBuildings.Add(Direction.LeftTop, GetBuildingArea(new Vector3(pos.x - BuildingArea.CellSize, pos.y, pos.z + BuildingArea.CellSize)));
            _adjacentBuildings.Add(Direction.LeftBottom, GetBuildingArea(new Vector3(pos.x - BuildingArea.CellSize, pos.y, pos.z - BuildingArea.CellSize)));
            _adjacentBuildings.Add(Direction.RightTop, GetBuildingArea(new Vector3(pos.x + BuildingArea.CellSize, pos.y, pos.z + BuildingArea.CellSize)));
            _adjacentBuildings.Add(Direction.RightBottom, GetBuildingArea(new Vector3(pos.x + BuildingArea.CellSize, pos.y, pos.z - BuildingArea.CellSize)));
        }
    }
}