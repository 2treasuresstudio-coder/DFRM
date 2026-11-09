using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BorderlessWindow : MonoBehaviour
{
    public static BorderlessWindow Instance;

    public enum DesktopScale
    {
        Small = 0,
        Medium = 1,
        Large = 2
    }

    [Header("Current Scale")]
    public DesktopScale currentScale =
        DesktopScale.Medium;

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR

    private const int GWL_STYLE = -16;

    private const int WS_BORDER = 0x00800000;
    private const int WS_CAPTION = 0x00C00000;
    private const int WS_THICKFRAME = 0x00040000;
    private const int WS_MINIMIZEBOX = 0x00020000;
    private const int WS_MAXIMIZEBOX = 0x00010000;
    private const int WS_SYSMENU = 0x00080000;

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(
        IntPtr hWnd,
        int nIndex);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(
        IntPtr hWnd,
        int nIndex,
        int dwNewLong);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int X,
        int Y,
        int cx,
        int cy,
        uint uFlags);

#endif

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        currentScale =
            (DesktopScale)
            PlayerPrefs.GetInt(
                "DesktopScale",
                (int)DesktopScale.Medium);

        ApplyWindowSize();
    }

    public void SetSmall()
    {
        currentScale =
            DesktopScale.Small;

        SaveScale();

        ApplyWindowSize();
    }

    public void SetMedium()
    {
        currentScale =
            DesktopScale.Medium;

        SaveScale();

        ApplyWindowSize();
    }

    public void SetLarge()
    {
        currentScale =
            DesktopScale.Large;

        SaveScale();

        ApplyWindowSize();
    }

    private void SaveScale()
    {
        PlayerPrefs.SetInt(
            "DesktopScale",
            (int)currentScale);

        PlayerPrefs.Save();
    }

    private int GetWindowHeight()
    {
        switch (currentScale)
        {
            case DesktopScale.Small:
                return 350;

            case DesktopScale.Large:
                return 540;

            default:
                return 420;
        }
    }

    public void ApplyWindowSize()
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR

        Screen.fullScreen = false;

        int monitorWidth =
            Display.main.systemWidth;

        int monitorHeight =
            Display.main.systemHeight;

        int windowHeight =
            GetWindowHeight();

        int x = 0;

        int y =
            monitorHeight -
            windowHeight;

        IntPtr hWnd =
            GetActiveWindow();

        int style =
            GetWindowLong(
                hWnd,
                GWL_STYLE);

        style &=
            ~WS_BORDER &
            ~WS_CAPTION &
            ~WS_THICKFRAME &
            ~WS_MINIMIZEBOX &
            ~WS_MAXIMIZEBOX &
            ~WS_SYSMENU;

        SetWindowLong(
            hWnd,
            GWL_STYLE,
            style);

        SetWindowPos(
            hWnd,
            IntPtr.Zero,
            x,
            y,
            monitorWidth,
            windowHeight,
            0);

        Debug.Log(
            $"Window Applied | " +
            $"Scale:{currentScale} | " +
            $"Size:{monitorWidth}x{windowHeight}");

#endif
    }
}