using Gameplay.Buildings;
using UnityEngine;

namespace Gameplay.Bounds
{
    [RequireComponent(typeof(Field))]
    public class FieldHandler : MonoBehaviour
    {
        private Field _field;

        private void Awake()
        {
            _field = GetComponent<Field>();
        }

        private void OnEnable()
        {
            AdjacentBuildings.PointBeyond += _field.Beyond;
        }

        private void OnDisable()
        {
            AdjacentBuildings.PointBeyond -= _field.Beyond;
        }
    }
}
