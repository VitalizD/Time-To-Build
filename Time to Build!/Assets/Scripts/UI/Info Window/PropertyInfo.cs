using System;

namespace UI.InformationWindow
{
    [Serializable]
    public class PropertyInfo
    {
        public BonusInfo[] Bonuses;
        public string ConditionText;

        public PropertyInfo(BonusInfo[] bonuses, string text)
        {
            Bonuses = bonuses;
            ConditionText = text;
        }
    }
}
