using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace AzipaWorks.AutoSave
{
    [InitializeOnLoad]
    internal static class AutoSaveManager
    {
        private static double _lastDirtyTime;
        private static bool _pendingChange;
        private static bool _isSaving;

        static AutoSaveManager()
        {
            EditorApplication.update -= Update;
            EditorApplication.update += Update;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorSceneManager.sceneDirtied -= OnSceneDirtied;
            EditorSceneManager.sceneDirtied += OnSceneDirtied;
            EditorSceneManager.activeSceneChangedInEditMode -= OnActiveSceneChanged;
            EditorSceneManager.activeSceneChangedInEditMode += OnActiveSceneChanged;
            ResetTimer();
        }

        internal static void ResetTimer()
        {
            var now = EditorApplication.timeSinceStartup;
            SetPendingState(SceneManager.GetActiveScene(), now);
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            ResetTimer();
        }

        private static void OnSceneDirtied(Scene scene)
        {
            var activeScene = SceneManager.GetActiveScene();
            if (!scene.IsValid() || scene.handle != activeScene.handle)
                return;

            var now = EditorApplication.timeSinceStartup;
            _lastDirtyTime = now;
            _pendingChange = true;
        }

        private static void OnActiveSceneChanged(Scene previous, Scene current)
        {
            SetPendingState(current, EditorApplication.timeSinceStartup);
        }

        private static void SetPendingState(Scene scene, double now)
        {
            _pendingChange = IsSaveableDirtyScene(scene);
            _lastDirtyTime = now;
        }

        private static void Update()
        {
            if (!AutoSaveSettings.Enabled || _isSaving)
                return;

            var now = EditorApplication.timeSinceStartup;
            var activeScene = SceneManager.GetActiveScene();
            if (!_pendingChange && IsSaveableDirtyScene(activeScene))
                SetPendingState(activeScene, now);

            if (!CanSaveSafely())
                return;

            if (!_pendingChange || now - _lastDirtyTime < AutoSaveSettings.IntervalSeconds)
                return;

            SaveActiveDirtyScene(now);
        }

        private static bool CanSaveSafely()
        {
            return !EditorApplication.isPlaying &&
                   !EditorApplication.isPlayingOrWillChangePlaymode &&
                   !EditorApplication.isCompiling &&
                   !EditorApplication.isUpdating;
        }

        private static void SaveActiveDirtyScene(double now)
        {
            var activeScene = SceneManager.GetActiveScene();
            if (!IsSaveableDirtyScene(activeScene))
                return;

            _isSaving = true;
            try
            {
                if (EditorSceneManager.SaveScene(activeScene))
                {
                    _pendingChange = false;
                    _lastDirtyTime = now;
                }
            }
            finally
            {
                _isSaving = false;
            }
        }

        private static bool IsSaveableDirtyScene(Scene scene)
        {
            return scene.IsValid() &&
                   scene.isLoaded &&
                   scene.isDirty &&
                   !string.IsNullOrEmpty(scene.path) &&
                   !EditorSceneManager.IsPreviewScene(scene);
        }
    }
}
