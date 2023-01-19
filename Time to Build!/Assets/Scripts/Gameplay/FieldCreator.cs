using UnityEngine;

namespace Gameplay
{
    public class FieldCreator : MonoBehaviour
    {
        [SerializeField] private Vector2 _size;
        [SerializeField] private Transform _startGenPoint;
        [SerializeField] private GameObject _buildingAreaPrefab;
        [SerializeField] private Transform[] _cityCenterPoints;
        [SerializeField] private GameObject[] _obstaclePrefabs;
    }
}