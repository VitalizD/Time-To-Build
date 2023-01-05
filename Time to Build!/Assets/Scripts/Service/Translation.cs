using System.Collections.Generic;

namespace Service
{
    public static class Translation
    {
        private readonly static Dictionary<ZoneType, string> _zoneNames = new()
        {
            [ZoneType.Resident] = "жилой район",
            [ZoneType.Commercial] = "коммерческий район",
            [ZoneType.Industrial] = "промышленный район",
            [ZoneType.Social] = "общественный район",
        };

        private readonly static Dictionary<BuildingType, string> _buildingNames = new()
        {
            [BuildingType.Road] = "Дорога",
            [BuildingType.House] = "Домишко",
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
            [ResourceType.Money] = "Главный ресурс для развития города. Чтобы получить больше, повышайте доход и озеленяйте город лесами.",
            [ResourceType.Population] = "Привлекайте население в Ваш город, поддерживая репутацию на высоком уровне. Наберите как можно больше населения, чтобы победить!",
            [ResourceType.Income] = "Количество монет, которые поступят в казну в конце текущего дня.",
            [ResourceType.Reputation] = "Значение населения, которое прибудет в город в конце текущего дня.",
        };

        public static string GetZoneName(ZoneType zoneType) => _zoneNames.ContainsKey(zoneType) ? _zoneNames[zoneType] : null;

        public static string GetBuildingName(BuildingType buildingType) => _buildingNames.ContainsKey(buildingType) ? _buildingNames[buildingType] : null;

        public static string GetBuildingDescription(BuildingType buildingType) => _buildingDescriptions.ContainsKey(buildingType) ? _buildingDescriptions[buildingType] : null;

        public static string GetResourceName(ResourceType resourceType) => _resourceNames.ContainsKey(resourceType) ? _resourceNames[resourceType] : null;

        public static string GetResourceDescription(ResourceType resourceType) => _resourceDescriptions.ContainsKey(resourceType) ? _resourceDescriptions[resourceType] : null;
    }
}