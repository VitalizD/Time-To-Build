using System;
using System.Collections.Generic;
using UnityEngine;

namespace Service
{
    public static class Translation
    {
        private const string CATEGORY_COLOR_CODE = "8d11a8";

        private readonly static Dictionary<ZoneType, string> _zoneNames = new()
        {
            [ZoneType.Resident] = "�����",
            [ZoneType.Commercial] = "������������",
            [ZoneType.Industrial] = "������������",
            [ZoneType.Social] = "������������",
        };

        private readonly static Dictionary<BuildingType, string> _buildingNames = new()
        {
            [BuildingType.Road] = "������",
            [BuildingType.LittleHouse] = "��������� �����",
            [BuildingType.FastFoodRestaurant] = "�������� �������� �������",
            [BuildingType.Factory] = "��������",
            [BuildingType.Park] = "����",
            [BuildingType.Lake] = "����",
            [BuildingType.HomeownersAssociation] = "���������� ��������������",
            [BuildingType.ConvenienceStore] = "�������������� �������",
            [BuildingType.Freeway] = "��������������",
            [BuildingType.FancyRestaurant] = "������ ����",
            [BuildingType.Farm] = "�����",
            [BuildingType.Landfill] = "������",
            [BuildingType.Mint] = "�������� ����",
            [BuildingType.HouseTwoFloors] = "����������� ����� ���",
            [BuildingType.PublicTransportStop] = "��������� �������. ����������",
            [BuildingType.OfficeBuilding] = "������� ������",
            [BuildingType.Parking] = "��������",
            [BuildingType.AnimalFarm] = "������� ����",
            [BuildingType.WaterTower] = "������������ �����",
        };

        private readonly static Dictionary<BuildingType, string> _buildingDescriptions = new()
        {
            [BuildingType.Road] = "������ ����� ������� ������ ����� � ��������."
        };

        private readonly static Dictionary<ResourceType, string> _resourceNames = new()
        {
            [ResourceType.Money] = "������",
            [ResourceType.Population] = "���������",
            [ResourceType.Income] = "�����",
            [ResourceType.Reputation] = "���������",
        };

        private readonly static Dictionary<ResourceType, string> _resourceDescriptions = new()
        {
            [ResourceType.Money] = "������� ������ ��� �������� ������. ����� �������� ������, ��������� ����� � ������� ����.\n\n<color=red>���� �� �������� � ������, �������� ����� �������� �����!</color>",
            [ResourceType.Population] = "������� ��������� ������ ������ ����, ��� �������� �� ������.\n\n���� ������ �� ����� ������� ��������� ���� �� 1 ����, �� ����������!\n\n<color=red>� ������ ��������� ����� � ��������� �����������.</color>",
            [ResourceType.Income] = "���������� �����, ������� �������� � ����� � ����� �������� ���.",
            [ResourceType.Reputation] = "���������, ������� �������� � ����� � ����� �������� ���.",
        };

        private readonly static Dictionary<BuildingCategory, string> _buildingCategoryNames = new()
        {
            [BuildingCategory.Office] = "����",
            [BuildingCategory.Nightlife] = "������ �����",
            [BuildingCategory.Restaurant] = "��������",
            [BuildingCategory.CarDealership] = "���������",
            [BuildingCategory.PublicTransport] = "������������ ���������",
            [BuildingCategory.Skyscraper] = "��������",
            [BuildingCategory.School] = "�����",
            [BuildingCategory.Reservoir] = "�����",
        };

        public static event Func<ZoneType, Color> GetZoneColor;

        public static string GetPropertyDescription(PropertyType propertyType, ZoneType[] zoneTypes, BuildingCategory[] buildingCategories)
        {
            switch (propertyType)
            {
                case PropertyType.Adjacents:
                    {
                        var result = "�� ������ ��������";
                        foreach (var zone in zoneTypes)
                            result += $" <color=#{ColorUtility.ToHtmlStringRGB(GetZoneColor(zone))}>{Translation.GetZoneName(zone)}</color>,";
                        if (zoneTypes.Length > 0)
                        {
                            result = result.TrimEnd(',');
                            result += " �����";
                        }
                        foreach (var category in buildingCategories)
                            result += $" <color=#{CATEGORY_COLOR_CODE}>{Translation.GetCategoryName(category)}</color>,";
                        result = result.TrimEnd(',');
                        return result;
                    }
                case PropertyType.Each:
                    {
                        var result = "�� ������ ���";
                        foreach (var zone in zoneTypes)
                            result += $" <color=#{ColorUtility.ToHtmlStringRGB(GetZoneColor(zone))}>{Translation.GetZoneName(zone)}</color>,";
                        if (zoneTypes.Length > 0)
                        {
                            result = result.TrimEnd(',');
                            result += " �����";
                        }
                        foreach (var category in buildingCategories)
                            result += $" <color=#{CATEGORY_COLOR_CODE}>{Translation.GetCategoryName(category)}</color>,";
                        result = result.TrimEnd(',');
                        return result;
                    }
                case PropertyType.NoReduction:
                    {
                        return "������ ��� ��� ����� ������� � ���������";
                    }
            }
            return "";
        }

        public static string GetZoneName(ZoneType zoneType) => _zoneNames.ContainsKey(zoneType) ? _zoneNames[zoneType] : null;

        public static string GetBuildingName(BuildingType buildingType) => _buildingNames.ContainsKey(buildingType) ? _buildingNames[buildingType] : null;

        public static string GetBuildingDescription(BuildingType buildingType) => _buildingDescriptions.ContainsKey(buildingType) ? _buildingDescriptions[buildingType] : null;

        public static string GetResourceName(ResourceType resourceType) => _resourceNames.ContainsKey(resourceType) ? _resourceNames[resourceType] : null;

        public static string GetResourceDescription(ResourceType resourceType) => _resourceDescriptions.ContainsKey(resourceType) ? _resourceDescriptions[resourceType] : null;

        public static string GetCategoryName(BuildingCategory buildingCategory) => _buildingCategoryNames.ContainsKey(buildingCategory) ? _buildingCategoryNames[buildingCategory] : null;
    }
}