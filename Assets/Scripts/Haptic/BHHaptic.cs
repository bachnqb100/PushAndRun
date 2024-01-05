using MoreMountains.NiceVibrations;
using UnityEngine;

namespace DefaultNamespace.Haptic
{
    public static class BHHaptic
    {
        public static void Haptic(HapticTypes type, bool defaultToRegularVibrate = false, bool alsoRumble = false)
        {
            if (GameManager.Instance.GameData.setting.haptic)
            {
                MMVibrationManager.Haptic(type, defaultToRegularVibrate, alsoRumble);
            }
        }

        public static void TransientHaptic(float intensity, float sharpness, bool alsoRumble = false)
        {
            if (GameManager.Instance.GameData.setting.haptic)
            {
                MMVibrationManager.TransientHaptic(intensity, sharpness, alsoRumble);
            }
        }
    }
}