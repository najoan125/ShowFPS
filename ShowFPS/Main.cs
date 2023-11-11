using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;

namespace ShowFPS
{
    public static class Main
    {
        public static UnityModManager.ModEntry.ModLogger Logger;
        public static Harmony harmony;
        public static Setting setting;
        public static Text text;
        public static Color color;
        public static bool IsEnabled = false;
        private static float delta = 0.0f;
        private static float timer = 0f;

        public static void Setup(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            modEntry.OnToggle = OnToggle;
            modEntry.OnUpdate = OnUpdate;
            setting = new Setting();
            setting = UnityModManager.ModSettings.Load<Setting>(modEntry);
            color = HexToColor(setting.color);
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(RDString.language == SystemLanguage.Korean ? "FPS 텍스트 : " : "FPS Text : ");
            GUILayout.BeginHorizontal();
            String fps = setting.text;
            String fpstxt = GUILayout.TextField(fps, GUILayout.Width(300));
            if (fpstxt != fps)
            {
                setting.text = fpstxt;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(RDString.language == SystemLanguage.Korean ? "색깔(hex 코드) : " : "Color(hex code) : ");
            GUILayout.BeginHorizontal();
            String color = setting.color;
            String colortxt = GUILayout.TextField(color, GUILayout.Width(300));
            if (colortxt != color)
            {
                setting.color = colortxt;
                Main.color = HexToColor(colortxt);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(RDString.language == SystemLanguage.Korean ? "폰트 크기(기본값: 40) : " : "Font Size(Default: 40) : ");
            String font = setting.fontsize.ToString();
            try
            {
                GUILayout.BeginHorizontal();
                String text = GUILayout.TextField(font);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                if (text != font)
                {
                    int result = Int32.Parse(text);
                    setting.fontsize = result;
                }
            }
            catch (FormatException)
            {
                setting.fontsize = 0;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(RDString.language == SystemLanguage.Korean ? "업데이트 간격(초) : " : "Update interval (seconds) : ");
            GUILayout.BeginHorizontal();
            String interval = setting.updateInterval.ToString();
            if (setting.updateInterval == 0) interval = "0.00";
            String intervaltxt = GUILayout.TextField(interval, GUILayout.Width(300));
            if (intervaltxt != interval)
            {
                try
                {
                    double result = double.Parse(intervaltxt);
                    setting.updateInterval = result;
                }
                catch (FormatException)
                {
                    setting.updateInterval = 0.05f;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();

            GUILayout.Label(" ");
            GUILayout.Label(RDString.language == SystemLanguage.Korean ? "좌표 설정" : "Coordinate Setting");
            GUILayout.BeginHorizontal();
            GUILayout.Label(RDString.language == SystemLanguage.Korean ? "X좌표 : " : "X coordinate : ");
            GUILayout.BeginHorizontal();
            float newx = GUILayout.HorizontalSlider(setting.x, 0, Screen.width, GUILayout.Width(300));
            GUILayout.Label(setting.x.ToString(), GUILayout.Width(300));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(RDString.language == SystemLanguage.Korean ? "Y좌표 : " : "Y coordinate : ");
            GUILayout.BeginHorizontal();
            float newy = GUILayout.HorizontalSlider(setting.y, 0, Screen.height, GUILayout.Width(300));
            GUILayout.Label(setting.y.ToString(), GUILayout.Width(300));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();
            if (newx != setting.x)
            {
                setting.x = newx;
            }
            if (newy != setting.y)
            {
                setting.y = newy;
            }
        }

        private static Color HexToColor(string hex)
        {
            Color color = Color.white;

            if (ColorUtility.TryParseHtmlString(hex, out color))
            {
                return color;
            }

            Debug.LogError($"Failed to convert hex code {hex} to Color.");
            return Color.white;
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            setting.Save(modEntry);
        }

        private static void OnUpdate(UnityModManager.ModEntry modentry, float deltaTime)
        {
            delta += (Time.unscaledDeltaTime - delta) * 0.1f;
            timer += deltaTime;
            if (timer > setting.updateInterval)
            {
                timer = 0;
                int fps = Mathf.RoundToInt(1.0f / delta);
                Text.Content = string.Concat(new String[]
                {
                setting.text,
                fps.ToString()
                });
            }
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            IsEnabled = value;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

            if (value)
            {
                harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                Main.text = new GameObject().AddComponent<Text>();
                UnityEngine.Object.DontDestroyOnLoad(Main.text);
            }
            else
            {
                harmony.UnpatchAll(modEntry.Info.Id);
                UnityEngine.Object.DestroyImmediate(Main.text);
                Main.text = null;
            }
            return true;
        }
    }
}
