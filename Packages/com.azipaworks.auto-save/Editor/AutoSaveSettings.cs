using UnityEditor;

namespace AzipaWorks.AutoSave
{
    internal enum AutoSaveIntervalMode
    {
        TenSeconds = 0,
        ThirtySeconds = 1,
        SixtySeconds = 2,
        Custom = 3
    }

    internal static class AutoSaveSettings
    {
        private const string PrefsPrefix = "AzipaWorks.AutoSave.";
        private const string EnabledKey = PrefsPrefix + "Enabled";
        private const string IntervalModeKey = PrefsPrefix + "IntervalMode";
        private const string CustomIntervalKey = PrefsPrefix + "CustomInterval";

        internal const int MinimumCustomIntervalMilliseconds = 500;
        internal const int DefaultCustomIntervalMilliseconds = 5000;

        internal static bool Enabled
        {
            get { return EditorPrefs.GetBool(EnabledKey, true); }
            set { EditorPrefs.SetBool(EnabledKey, value); }
        }

        internal static AutoSaveIntervalMode IntervalMode
        {
            get
            {
                var storedValue = EditorPrefs.GetInt(IntervalModeKey, (int)AutoSaveIntervalMode.TenSeconds);
                if (storedValue < (int)AutoSaveIntervalMode.TenSeconds ||
                    storedValue > (int)AutoSaveIntervalMode.Custom)
                {
                    return AutoSaveIntervalMode.TenSeconds;
                }

                return (AutoSaveIntervalMode)storedValue;
            }
            set { EditorPrefs.SetInt(IntervalModeKey, (int)value); }
        }

        internal static int CustomIntervalMilliseconds
        {
            get
            {
                return ClampCustomInterval(
                    EditorPrefs.GetInt(CustomIntervalKey, DefaultCustomIntervalMilliseconds));
            }
            set { EditorPrefs.SetInt(CustomIntervalKey, ClampCustomInterval(value)); }
        }

        internal static double IntervalSeconds
        {
            get
            {
                switch (IntervalMode)
                {
                    case AutoSaveIntervalMode.TenSeconds:
                        return 10d;
                    case AutoSaveIntervalMode.ThirtySeconds:
                        return 30d;
                    case AutoSaveIntervalMode.SixtySeconds:
                        return 60d;
                    case AutoSaveIntervalMode.Custom:
                        return CustomIntervalMilliseconds / 1000d;
                    default:
                        return 60d;
                }
            }
        }

        internal static int ClampCustomInterval(int milliseconds)
        {
            return milliseconds < MinimumCustomIntervalMilliseconds
                ? MinimumCustomIntervalMilliseconds
                : milliseconds;
        }
    }
}
