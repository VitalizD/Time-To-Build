using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Buildings
{
    public class AdjacentBuildings : MonoBehaviour
    {
        private const float checkRadius = 0.1f;

        private readonly Dictionary<Direction, BuildingArea> _adjacentBuildings = new();
        private readonly Direction[] _sidesFour = { Direction.Left, Direction.Right, Direction.Top, Direction.Bottom };

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

        private void CreateAdjacentBuildingDictionary()
        {
            var pos = transform.position;
            _adjacentBuildings.Clear();
            _adjacentBuildings.Add(Direction.Left, GetBuildingArea(new Vector3(pos.x - BuildingArea.CellSize, pos.y, pos.z)));
            _adjacentBuildings.Add(Direction.Right, GetBuildingArea(new Vector3(pos.x + BuildingArea.CellSize, pos.y, pos.z)));
            _adjacentBuildings.Add(Direction.Top, GetBuildingArea(new Vector3(pos.x, pos.y, pos.z + BuildingArea.CellSize)));
            _adjacentBuildings.Add(Direction.Bottom, GetBuildingArea(new Vector3(pos.x, pos.y, pos.z - BuildingArea.CellSize)));
            _adjacentBuildings.Add(Direction.LeftTop, GetBuildingArea(new Vector3(pos.x - BuildingArea.CellSize, pos.y, pos.z + BuildingArea.CellSize)));
            _adjacentBuildings.Add(Direction.LeftBottom, GetBuildingArea(new Vector3(pos.x - BuildingArea.CellSize, pos.y, pos.z - BuildingArea.CellSize)));
            _adjacentBuildings.Add(Direction.RightTop, GetBuildingArea(new Vector3(pos.x + BuildingArea.CellSize, pos.y, pos.z + BuildingArea.CellSize)));
            _adjacentBuildings.Add(Direction.RightBottom, GetBuildingArea(new Vector3(pos.x + BuildingArea.CellSize, pos.y, pos.z - BuildingArea.CellSize)));
        }

        private BuildingArea GetBuildingArea(Vector3 point)
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
    }
}