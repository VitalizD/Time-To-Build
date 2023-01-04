using UnityEngine;

namespace Gameplay.Bounds
{
    public class Field : MonoBehaviour
    {
        public bool Beyond(Vector3 position)
        {
            var colliders = new Collider[5];
            Physics.OverlapSphereNonAlloc(new Vector3(position.x, transform.position.y, position.z), 0.1f, colliders);
            foreach (var collider in colliders)
            {
                if (collider == null)
                    return true;
                if (collider.GetComponent<Field>() != null)
                    return false;
            }
            return true;
        }
    }
}