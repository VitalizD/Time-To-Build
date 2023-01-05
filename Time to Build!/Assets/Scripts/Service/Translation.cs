using System.Collections.Generic;

namespace Service
{
    public static class Translation
    {
        private readonly static Dictionary<ZoneType, string> _zoneNames = new()
        {
            [ZoneType.Resident] = "����� �����",
            [ZoneType.Commercial] = "������������ �����",
            [ZoneType.Industrial] = "������������ �����",
            [ZoneType.Social] = "������������ �����",
        };

        private readonly static Dictionary<BuildingType, string> _buildingNames = new()
        {
            [BuildingType.Road] = "������",
            [BuildingType.House] = "�������",
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
            [ResourceType.Money] = "������� ������ ��� �������� ������. ����� �������� ������, ��������� ����� � ���������� ����� ������.",
            [ResourceType.Population] = "����������� ��������� � ��� �����, ����������� ��������� �� ������� ������. �������� ��� ����� ������ ���������, ����� ��������!",
            [ResourceType.Income] = "���������� �����, ������� �������� � ����� � ����� �������� ���.",
            [ResourceType.Reputation] = "�������� ���������, ������� �������� � ����� � ����� �������� ���.",
        };

        public static string GetZoneName(ZoneType zoneType) => _zoneNames.ContainsKey(zoneType) ? _zoneNames[zoneType] : null;

        public static string GetBuildingName(BuildingType buildingType) => _buildingNames.ContainsKey(buildingType) ? _buildingNames[buildingType] : null;

        public static string GetBuildingDescription(BuildingType buildingType) => _buildingDescriptions.ContainsKey(buildingType) ? _buildingDescriptions[buildingType] : null;

        public static string GetResourceName(ResourceType resourceType) => _resourceNames.ContainsKey(resourceType) ? _resourceNames[resourceType] : null;

        public static string GetResourceDescription(ResourceType resourceType) => _resourceDescriptions.ContainsKey(resourceType) ? _resourceDescriptions[resourceType] : null;
    }
}