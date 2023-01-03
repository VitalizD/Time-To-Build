using UnityEngine;

namespace Service
{
    public class BuildingSiteSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _buildingSite;

        public void Spawn()
        {
            Instantiate(_buildingSite, transform.position, Quaternion.identity, transform.parent);
        }
    }
}
