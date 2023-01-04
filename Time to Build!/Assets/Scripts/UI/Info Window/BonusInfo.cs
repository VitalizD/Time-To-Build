using UnityEngine;

namespace UI.InformationWindow
{
    public class BonusInfo
    {
        public string Text { get; }
        public Sprite Icon { get; }

        public BonusInfo(string text, Sprite icon)
        {
            Text = text;
            Icon = icon;
        }
    }
}
