using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShowFPS
{
    public class Text : MonoBehaviour
    {
        public static string Content = " ";
        private Rect ShadowTextLocation;
        private GUIStyle ShadowTextStyle = new GUIStyle();

        private void Start()
        {
            this.ShadowTextStyle.font = RDString.GetFontDataForLanguage(RDString.language).font;
            this.ShadowTextStyle.fontSize = Main.setting.fontsize;
            this.ShadowTextStyle.fontStyle = FontStyle.Normal;
            this.ShadowTextStyle.normal.textColor = new Color(0f, 0f, 0f, 0.45f);
        }

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = Main.setting.fontsize;
            style.normal.textColor = Main.color;
            style.fontStyle = FontStyle.Normal;
            style.font = RDString.GetFontDataForLanguage(RDString.language).font;
            this.ShadowTextStyle.fontSize = Main.setting.fontsize;

            this.ShadowTextLocation = new Rect((float)(Main.setting.x + 2 + 10f), (float)(Main.setting.y - 10f + 2), 
                (float)Main.setting.x, (float)Main.setting.y - 10f);
            GUI.Label(this.ShadowTextLocation, Text.Content, this.ShadowTextStyle);
            GUI.Label(new Rect(10f + Main.setting.x, -10f + Main.setting.y, (float)Screen.width, (float)Screen.height), Text.Content, style);
        }
    }
}
