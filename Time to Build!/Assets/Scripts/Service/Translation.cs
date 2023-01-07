using System;
using System.Collections.Generic;
using UnityEngine;

namespace Service
{
    public static class Translation
    {
        private readonly static Dictionary<ZoneType, string> _zoneNames = new()
        {
            [ZoneType.Resident] = "жилой",
            [ZoneType.Commercial] = "коммерческий",
            [ZoneType.Industrial] = "промышленный",
            [ZoneType.Social] = "общественный",
        };

        private readonly static Dictionary<BuildingType, string> _buildingNames = new()
        {
            [BuildingType.Road] = "Дорога",
            [BuildingType.LittleHouse] = "Маленький домик",
            [BuildingType.FastFoonRestaurant] = "Ресторан быстрого питания",
            [BuildingType.Factory] = "Комбинат",
            [BuildingType.Park] = "Парк",
            [BuildingType.Lake] = "Пруд",
        };

        private readonly static Dictionary<BuildingType, string> _buildingDescriptions = new()
        {
            [BuildingType.Road] = "Здания можно строить только рядом с дорогами."
        };

        private readonly static Dictionary<ResourceType, string> _resourceNames = new()
        {
            [ResourceType.Money] = "Монеты",
            [ResourceType.Population] = "Население",
            [ResourceType.Income] = "Доход",
            [ResourceType.Reputation] = "Репутация",
        };

        private readonly static Dictionary<ResourceType, string> _resourceDescriptions = new()
        {
            [ResourceType.Money] = "Главный ресурс для развития города. Чтобы получить больше, повышайте доход и стройте озёра.\n\n<color=red>Если Вы утопаете в долгах, горожане будут покидать город!</color>",
            [ResourceType.Population] = "Привлекайте население в Ваш город, поддерживая репутацию на высоком уровне. Наберите как можно больше населения, чтобы победить!\n\n<color=red>С ростом населения доход и репутация уменьшаются.</color>",
            [ResourceType.Income] = "Количество монет, которые поступят в казну в конце текущего дня.",
            [ResourceType.Reputation] = "Население, которое прибудет в город в конце текущего дня.",
        };

        public static event Func<ZoneType, Color> GetZoneColor;

        public static string GetPropertyDescription(PropertyType propertyType, ZoneType[] zoneTypes)
        {
            switch (propertyType)
            {
                case PropertyType.Adjacents:
                    {
                        var result = "за каждый соседний";
                        foreach (var zone in zoneTypes)
                            result += $" <color=#{ColorUtility.ToHtmlStringRGB(GetZoneColor(zone))}>{Translation.GetZoneName(zone)}</color>,";
                        result = result.TrimEnd(',');
                        result += " район";
                        return result;
                    }
                case PropertyType.Each:
                    {
                        var result = "за каждый ваш";
                        foreach (var zone in zoneTypes)
                            result += $" <color=#{ColorUtility.ToHtmlStringRGB(GetZoneColor(zone))}>{Translation.GetZoneName(zone)}</color>,";
                        result = result.TrimEnd(',');
                        result += " район";
                        return result;
                    }
                case PropertyType.NoReduction:
                    {
                        return "каждый раз при убыли доходов и репутации";
                    }
            }
            return "";
        }

        public static string GetZoneName(ZoneType zoneType) => _zoneNames.ContainsKey(zoneType) ? _zoneNames[zoneType] : null;

        public static string GetBuildingName(BuildingType buildingType) => _buildingNames.ContainsKey(buildingType) ? _buildingNames[buildingType] : null;

        public static string GetBuildingDescription(BuildingType buildingType) => _buildingDescriptions.ContainsKey(buildingType) ? _buildingDescriptions[buildingType] : null;

        public static string GetResourceName(ResourceType resourceType) => _resourceNames.ContainsKey(resourceType) ? _resourceNames[resourceType] : null;

        public static string GetResourceDescription(ResourceType resourceType) => _resourceDescriptions.ContainsKey(resourceType) ? _resourceDescriptions[resourceType] : null;
    }
}