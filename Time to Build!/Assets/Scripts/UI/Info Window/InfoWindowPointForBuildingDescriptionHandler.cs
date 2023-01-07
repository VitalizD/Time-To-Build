using Gameplay.Buildings;
using UnityEngine;

public class InfoWindowPointForBuildingDescriptionHandler : MonoBehaviour
{
    private void OnEnable()
    {
        BuildingArea.GetInfoWindowPoint += GetPosition;
    }

    private void OnDisable()
    {
        BuildingArea.GetInfoWindowPoint -= GetPosition;
    }

    private Vector2 GetPosition() => transform.position;
}
