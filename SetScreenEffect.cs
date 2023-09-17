using System;
using System.Runtime.InteropServices;

namespace ScreenFX
{
    static class SetScreenEffect
    {
        [DllImport("SetScreenEffect32.dll", EntryPoint = "SetScreenRot")]
        private static extern void SetScreenRot32(float deg);
        [DllImport("SetScreenEffect64.dll", EntryPoint = "SetScreenRot")]
        private static extern void SetScreenRot64(float deg);
        [DllImport("SetScreenEffect32.dll", EntryPoint = "SetScreenBright")]
        private static extern void SetScreenBright32(float bright);
        [DllImport("SetScreenEffect64.dll", EntryPoint = "SetScreenBright")]
        private static extern void SetScreenBright64(float bright);
        [DllImport("SetScreenEffect32.dll", EntryPoint = "SetScreenCon")]
        private static extern void SetScreenCon32(float con);
        [DllImport("SetScreenEffect64.dll", EntryPoint = "SetScreenCon")]
        private static extern void SetScreenCon64(float con);
        [DllImport("SetScreenEffect32.dll", EntryPoint = "SetScreenSat")]
        private static extern void SetScreenSat32(float sat);
        [DllImport("SetScreenEffect64.dll", EntryPoint = "SetScreenSat")]
        private static extern void SetScreenSat64(float sat);
        [DllImport("SetScreenEffect32.dll", EntryPoint = "SetScreenOp")]
        private static extern void SetScreenOp32(float sat);
        [DllImport("SetScreenEffect64.dll", EntryPoint = "SetScreenOp")]
        private static extern void SetScreenOp64(float sat);
        internal static void SetScreenRot(float deg)
        {
            if (IntPtr.Size > 4)
                SetScreenRot64(deg);
            else
                SetScreenRot32(deg);
        }
        internal static void SetScreenBright(float bright)
        {
            if (IntPtr.Size > 4)
                SetScreenBright64(bright);
            else
                SetScreenBright32(bright);
        }
        internal static void SetScreenCon(float con)
        {
            if (IntPtr.Size > 4)
                SetScreenCon64(con);
            else
                SetScreenCon32(con);
        }
        internal static void SetScreenSat(float sat)
        {
            if (IntPtr.Size > 4)
                SetScreenSat64(sat);
            else
                SetScreenSat32(sat);
        }
        internal static void SetScreenOp(float op)
        {
            if (IntPtr.Size > 4)
                SetScreenOp64(op);
            else
                SetScreenOp32(op);
        }
    }
}
