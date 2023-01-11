using Service.Sounds;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.BuildingPanel
{
    [RequireComponent(typeof(Toggle))]
    public class BuildingPanelTab : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private GameObject _buildingsList;

        private Toggle _toggle;

        public static event Action UpdateBuildingLotColors;

        public void OnPointerDown(PointerEventData eventData)
        {
            SoundManager.Instance.Play(Sound.Click, null);
        }

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
            StartCoroutine(UpdateBuildingLotColorsAfterFrame());
        }

        private IEnumerator UpdateBuildingLotColorsAfterFrame()
        {
            yield return new WaitForEndOfFrame();
            UpdateBuildingLotColors?.Invoke();
        }
    }
}