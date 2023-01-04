using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BuildingPanel
{
    [RequireComponent(typeof(Toggle))]
    public class BuildingPanelTab : MonoBehaviour
    {
        [SerializeField] private GameObject _buildingsList;

        private Toggle _toggle;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void Start()
        {
            if (_toggle.isOn)
                OnValueChanged(true);
        }

        private void OnValueChanged(bool value)
        {
            _buildingsList.SetActive(value);
            _toggle.interactable = !value;
        }
    }
}