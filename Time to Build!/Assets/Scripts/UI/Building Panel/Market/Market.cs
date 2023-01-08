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
            RemoveLastLot();
            CheckOnNullLots();
            var replenishTo = UnityEngine.Random.Range(_minLots, _maxLots);
            if (_lots.Count < replenishTo)
            {
                var replenishCount = replenishTo - _lots.Count;
                for (var i = 0; i < replenishCount; ++i)
                    AddOne(false);
            }
            UpdatePrices();
            if (_lots.Count > 0)
                _lots[^1].ActivateSoonDisappear();
        }

        public void AddOne(bool updatePrises)
        {
            var buildingInfo = GetNextBuildingInfo?.Invoke();
            if (buildingInfo == null)
                return;
            var lot = Instantiate(_lotPrefab, _marketLots).GetComponent<BuildingLot>();
            lot.transform.SetAsFirstSibling();
            lot.Set(buildingInfo.Type, true);
            _lots.Insert(0, lot);

            if (updatePrises)
                UpdatePrices();
        }

        private void UpdatePrices()
        {
            var freeLots = _freeLots;
            var currentMarkup = _markupStep;
            for (var i = _lots.Count - 1; i >= 0; --i)
            {
                if (_lots[i] == null)
                    continue;
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
            }
        }

        private void RemoveLastLot()
        {
            if (_lots.Count > 0)
            {
                if (_lots[^1] != null)
                    Destroy(_lots[^1].gameObject);
                _lots.RemoveAt(_lots.Count - 1);
            }
        }

        private void CheckOnNullLots()
        {
            for (var i = _lots.Count - 1; i >= 0; --i)
            {
                if (_lots[i] == null)
                    _lots.RemoveAt(i);
            }
        }
    }
}