using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Buildings
{
    [RequireComponent(typeof(AdjacentBuildings))]
    public class RoadAdapter : MonoBehaviour
    {
        private const BuildingType R = BuildingType.Road;

        private AdjacentBuildings _adjacentBuildings;
        private GameObject _road;

        public static event Func<RoadType, GameObject> GetRoad;

        private void Awake()
        {
            _adjacentBuildings = GetComponent<AdjacentBuildings>();
        }

        public void CreateAdaptRoad(Vector3 point)
        {
            var types = new Dictionary<Direction, BuildingType>();
            foreach (var adjacent in _adjacentBuildings.Get4Sides())
            {
                if (adjacent.Value == null)
                    types.Add(adjacent.Key, BuildingType.BuildingSite);
                else
                    types.Add(adjacent.Key, adjacent.Value.Type);
            }

            // End
            if (types[Direction.Left] == R && types[Direction.Right] != R && types[Direction.Top] != R && types[Direction.Bottom] != R)
                Create(RoadType.DeadEnd, 0f, point);
            else if (types[Direction.Left] != R && types[Direction.Right] == R && types[Direction.Top] != R && types[Direction.Bottom] != R)
                Create(RoadType.DeadEnd, 180f, point);
            else if (types[Direction.Left] != R && types[Direction.Right] != R && types[Direction.Top] == R && types[Direction.Bottom] != R)
                Create(RoadType.DeadEnd, 90f, point);
            else if (types[Direction.Left] != R && types[Direction.Right] != R && types[Direction.Top] != R && types[Direction.Bottom] == R)
                Create(RoadType.DeadEnd, 270f, point);

            // Square
            else if (types[Direction.Left] != R && types[Direction.Right] != R && types[Direction.Top] != R && types[Direction.Bottom] != R)
                Create(RoadType.Square, 0f, point);

            // Straight
            else if (types[Direction.Left] == R && types[Direction.Right] == R && types[Direction.Top] != R && types[Direction.Bottom] != R)
                Create(RoadType.Straight, 0f, point);
            else if (types[Direction.Left] != R && types[Direction.Right] != R && types[Direction.Top] == R && types[Direction.Bottom] == R)
                Create(RoadType.Straight, 90f, point);

            // Corner
            else if (types[Direction.Left] != R && types[Direction.Right] == R && types[Direction.Top] == R && types[Direction.Bottom] != R)
                Create(RoadType.Corner, 0f, point);
            else if (types[Direction.Left] != R && types[Direction.Right] == R && types[Direction.Top] != R && types[Direction.Bottom] == R)
                Create(RoadType.Corner, 90f, point);
            else if (types[Direction.Left] == R && types[Direction.Right] != R && types[Direction.Top] != R && types[Direction.Bottom] == R)
                Create(RoadType.Corner, 180f, point);
            else if (types[Direction.Left] == R && types[Direction.Right] != R && types[Direction.Top] == R && types[Direction.Bottom] != R)
                Create(RoadType.Corner, 270f, point);

            // T-shaped
            else if (types[Direction.Left] == R && types[Direction.Right] == R && types[Direction.Top] == R && types[Direction.Bottom] != R)
                Create(RoadType.TShaped, 0f, point);
            else if (types[Direction.Left] != R && types[Direction.Right] == R && types[Direction.Top] == R && types[Direction.Bottom] == R)
                Create(RoadType.TShaped, 90f, point);
            else if (types[Direction.Left] == R && types[Direction.Right] == R && types[Direction.Top] != R && types[Direction.Bottom] == R)
                Create(RoadType.TShaped, 180f, point);
            else if (types[Direction.Left] == R && types[Direction.Right] != R && types[Direction.Top] == R && types[Direction.Bottom] == R)
                Create(RoadType.TShaped, 270f, point);

            // Cross
            else if (types[Direction.Left] == R && types[Direction.Right] == R && types[Direction.Top] == R && types[Direction.Bottom] == R)
                Create(RoadType.Cross, 0f, point);
        }

        private void Create(RoadType type, float angle, Vector3 point)
        {
            if (_road != null)
                Destroy(_road);
            var roadPrefab = GetRoad?.Invoke(type);
            _road = Instantiate(roadPrefab, point, Quaternion.Euler(0f, angle, 0f), transform);
        }
    }
}