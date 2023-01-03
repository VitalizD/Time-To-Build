using Gameplay.Buildings;
using UnityEngine;

namespace Service.RoadStorage
{
    [RequireComponent(typeof(RoadStorage))]
    public class RoadStorageHandler : MonoBehaviour
    {
        private RoadStorage _roadStorage;

        private void Awake()
        {
            _roadStorage = GetComponent<RoadStorage>();
        }

        private void OnEnable()
        {
            RoadAdapter.GetRoad += _roadStorage.GetRoad;
        }

        private void OnDisable()
        {
            RoadAdapter.GetRoad -= _roadStorage.GetRoad;
        }
    }
}
