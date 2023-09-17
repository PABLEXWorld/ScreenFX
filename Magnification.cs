using System;
using System.Runtime.InteropServices;

namespace ScreenFX
{
    static class Magnification
    {
        [DllImport("MAGNIFICATION.dll")]
        internal static extern bool MagInitialize();
        [DllImport("MAGNIFICATION.dll")]
        internal static extern bool MagUninitialize();
        [DllImport("MAGNIFICATION.dll")]
        internal static extern bool MagSetFullscreenTransform(float magLevel, int xOffset, int yOffset);
        [DllImport("MAGNIFICATION.dll", EntryPoint = "#1")]
        private static extern long MagSetFullscreenUseBitmapSmoothingPtr(int value);
        internal static long MagSetFullscreenUseBitmapSmoothing(int value)
        {
            if (IsBitmapSmoothingSupported())
            {
                return MagSetFullscreenUseBitmapSmoothingPtr(value);
            }
            else
            {
                return -1;
            }
        }
        internal static bool IsBitmapSmoothingSupported()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT & Environment.OSVersion.Version.Major >= 10 & Environment.OSVersion.Version.Build >= 16215)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
