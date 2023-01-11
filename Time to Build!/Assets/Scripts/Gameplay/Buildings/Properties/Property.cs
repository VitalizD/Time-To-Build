using System;
using UI.InformationWindow;

[Serializable]
public class Property
{
    public PropertyType Type;
    public BonusInfo[] Bonuses;
    public ZoneType[] Zones;
    public BuildingCategory[] Categories;
}
