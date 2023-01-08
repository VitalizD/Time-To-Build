using Service.BuildingStorage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.BuildingPanel.Market
{
    public class Market : MonoBehaviour
    {
        [SerializeField] private Transform _marketLots;
        [SerializeField] private GameObject _lotPrefab;
        [SerializeField] private int _minLots;
        [SerializeField] private int _maxLots;
        [SerializeField] private int _freeLots = 2;
        [SerializeField] private int _markupStep = 2;

        private readonly List<BuildingLot> _lots = new();

        public static event Func<Building> GetNextBuildingInfo;

        public void Replenish()
        {
            if (_lots.Count > 0)
            {
                Destroy(_lots[^1].gameObject);
                _lots.RemoveAt(_lots.Count - 1);
            }
            var replenishTo = UnityEngine.Random.Range(_minLots, _maxLots);
            if (_lots.Count < replenishTo)
            {
                var replenishCount = replenishTo - _lots.Count;
                for (var i = 0; i < replenishCount; ++i)
                {
                    var buildingInfo = GetNextBuildingInfo?.Invoke();
                    if (buildingInfo == null)
                        break;
                    var lot = Instantiate(_lotPrefab, _marketLots).GetComponent<BuildingLot>();
                    lot.transform.SetAsFirstSibling();
                    lot.Set(buildingInfo.Type, true);
                    _lots.Insert(0, lot);
                }
            }
            UpdatePrices();
        }

        private void UpdatePrices()
        {
            var freeLots = _freeLots;
            var currentMarkup = _markupStep;
            for (var i = _lots.Count - 1; i >= 0; --i)
            {
                if (freeLots > 0)
                {
                    _lots[i].SetMarkup(0);
                    --freeLots;
                }
                else
                {
                    _lots[i].SetMarkup(currentMarkup);
                    currentMarkup += _markupStep;
                }
                if (i == _lots.Count - 1)
                    _lots[i].ActivateSoonDisappear();
            }
        }
    }
}