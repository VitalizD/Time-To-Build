using Gameplay.Buildings;
using System;
using UnityEngine;

namespace Service
{
    public class BuildingSiteSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _buildingSite;

        public static event Action<BuildingArea> AddBuildingSite;

        public void Spawn()
        {
            var buildingSite = Instantiate(_buildingSite, transform.position, Quaternion.identity, transform.parent);
            AddBuildingSite?.Invoke(buildingSite.GetComponent<BuildingArea>());
        }
    }
}
