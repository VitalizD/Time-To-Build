using Gameplay.Buildings;
using UnityEngine;

public class InfoWindowPointForBuildingDescriptionHandler : MonoBehaviour
{
    private void OnEnable()
    {
        BuildingArea.GetInfoWindowPoint += GetTransform;
    }

    private void OnDisable()
    {
        BuildingArea.GetInfoWindowPoint -= GetTransform;
    }

    private Transform GetTransform() => transform;
}
