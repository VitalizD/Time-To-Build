namespace UI.InformationWindow
{
    public class PropertyInfo
    {
        public BonusInfo[] Bonuses { get; }
        public string Text { get; }

        public PropertyInfo(BonusInfo[] bonuses, string text)
        {
            Bonuses = bonuses;
            Text = text;
        }
    }
}
