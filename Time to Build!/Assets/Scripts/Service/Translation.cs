using System.Collections.Generic;

namespace Service
{
    public static class Translation
    {
        private readonly static Dictionary<ZoneType, string> _zoneNames = new Dictionary<ZoneType, string>
        {
            [ZoneType.Resident] = "����� �����",
            [ZoneType.Commercial] = "������������ �����",
            [ZoneType.Industrial] = "������������ �����",
            [ZoneType.Social] = "������������ �����",
        };

        private readonly static Dictionary<BuildingType, string> _buildingNames = new Dictionary<BuildingType, string>
        {
            [BuildingType.Road] = "������",
            [BuildingType.House] = "����� ���",
        };

        private readonly static Dictionary<BuildingType, string> _buildingDescriptions = new Dictionary<BuildingType, string>
        {
            [BuildingType.Road] = "������ ����� ������� ������ ����� � ��������."
        };

        public static string GetZoneName(ZoneType zoneType) => _zoneNames.ContainsKey(zoneType) ? _zoneNames[zoneType] : null;

        public static string GetBuildingName(BuildingType buildingType) => _buildingNames.ContainsKey(buildingType) ? _buildingNames[buildingType] : null;

        public static string GetBuildingDescription(BuildingType buildingType) => _buildingDescriptions.ContainsKey(buildingType) ? _buildingDescriptions[buildingType] : null;
    }
}