using UnityEditor;
using UnityEngine;

namespace AzipaWorks.AutoSave
{
    internal sealed class AutoSaveWindow : EditorWindow
    {
        private static readonly string[] IntervalLabels = { "10s", "30s", "60s", "Custom" };

        private const float HeaderHeight = 106f;
        private const float HeaderPadding = 16f;
        private const float IconSize = 60f;
        private const float TitleFontSize = 18f;
        private const float SubtitleFontSize = 12f;
        private static Texture2D _headerIcon;

        [MenuItem("Tools/Azipa Tools/Auto Save", false, 913)]
        private static void Open()
        {
            var window = GetWindow<AutoSaveWindow>();
            window.titleContent = new GUIContent("Auto Save");
            window.minSize = new Vector2(330f, 205f);
            window.maxSize = new Vector2(430f, 600f);
            window.Show();
        }

        private void OnGUI()
        {
            UpdateMinimumSize();
            DrawHeader();
            EditorGUILayout.Space(10f);
            DrawSettings();
        }

        private void UpdateMinimumSize()
        {
            var requiredHeight = AutoSaveSettings.IntervalMode == AutoSaveIntervalMode.Custom
                ? 287f
                : 237f;
            minSize = new Vector2(330f, requiredHeight);
        }

        private static void DrawHeader()
        {
            var headerBackground = new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset((int)HeaderPadding, (int)HeaderPadding, 10, 10),
                margin = new RectOffset(0, 0, 0, 0)
            };

            using (new EditorGUILayout.VerticalScope(headerBackground, GUILayout.Height(HeaderHeight)))
            {
                var titleStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = (int)TitleFontSize,
                    fixedHeight = 24f
                };
                var subtitleStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = (int)SubtitleFontSize,
                    fixedHeight = 18f
                };
                subtitleStyle.normal.textColor = EditorGUIUtility.isProSkin
                    ? new Color(0.72f, 0.72f, 0.72f, 1f)
                    : new Color(0.28f, 0.28f, 0.28f, 1f);

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (_headerIcon == null)
                    {
                        _headerIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(
                            "Packages/com.azipaworks.auto-save/Editor/auto_save_icon.png");
                    }

                    var icon = _headerIcon != null
                        ? _headerIcon
                        : EditorGUIUtility.IconContent("SaveAs").image;
                    if (icon != null)
                        GUILayout.Label(icon, GUIStyle.none, GUILayout.Width(IconSize), GUILayout.Height(IconSize));

                    GUILayout.Space(10f);
                    using (new EditorGUILayout.VerticalScope(GUILayout.Height(IconSize), GUILayout.ExpandWidth(true)))
                    {
                        EditorGUILayout.LabelField("Auto Save", titleStyle);
                        EditorGUILayout.Space(4f);
                        EditorGUILayout.LabelField("変更が止まった後にSceneを自動保存します。", subtitleStyle);
                    }
                }

                GUILayout.FlexibleSpace();
                var footerStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
                {
                    alignment = TextAnchor.MiddleRight
                };
                EditorGUILayout.LabelField("Auto Save  v1.0.1", footerStyle,
                    GUILayout.ExpandWidth(true), GUILayout.Height(14f));
            }
        }

        private void DrawSettings()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Auto Save", EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();

                    var enabled = AutoSaveSettings.Enabled;
                    var nextEnabled = GUILayout.Toggle(enabled, enabled ? "ON" : "OFF",
                        EditorStyles.miniButton, GUILayout.Width(72f), GUILayout.Height(22f));
                    if (nextEnabled != enabled)
                    {
                        AutoSaveSettings.Enabled = nextEnabled;
                        AutoSaveManager.ResetTimer();
                    }
                }

                EditorGUILayout.Space(10f);
                EditorGUILayout.LabelField("Save Delay", EditorStyles.boldLabel);
                EditorGUILayout.Space(3f);

                using (new EditorGUI.DisabledScope(!AutoSaveSettings.Enabled))
                {
                    var currentMode = AutoSaveSettings.IntervalMode;
                    var nextMode = (AutoSaveIntervalMode)GUILayout.Toolbar(
                        (int)currentMode, IntervalLabels, GUILayout.Height(24f));
                    if (nextMode != currentMode)
                    {
                        AutoSaveSettings.IntervalMode = nextMode;
                        AutoSaveManager.ResetTimer();
                    }

                    EditorGUILayout.LabelField("Sceneの変更が止まってから保存するまでの時間です。",
                        EditorStyles.miniLabel);

                    if (AutoSaveSettings.IntervalMode == AutoSaveIntervalMode.Custom)
                    {
                        EditorGUILayout.Space(10f);
                        EditorGUILayout.LabelField("Custom Interval", EditorStyles.boldLabel);
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            var currentMilliseconds = AutoSaveSettings.CustomIntervalMilliseconds;
                            var nextMilliseconds = EditorGUILayout.DelayedIntField(currentMilliseconds);
                            GUILayout.Label("ms", GUILayout.Width(22f));
                            if (nextMilliseconds != currentMilliseconds)
                            {
                                AutoSaveSettings.CustomIntervalMilliseconds = nextMilliseconds;
                                AutoSaveManager.ResetTimer();
                            }
                        }
                    }

                }
            }
        }

    }
}
