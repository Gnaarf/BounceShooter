using SFML.Window;
using System.Collections.Generic;

/// <summary>
/// Wrapper Class for the SFML.Window.Keyboard. Better usability.
/// </summary>
public static class KeyboardInputManager
{
    static bool isInitialized = false;

    private static int keyCount;
    private static bool[] previousKeyIsPressed;
    private static bool[] currentKeyIsPressed;

    private static void Initialize()
    {
        keyCount = (int)Keyboard.Key.KeyCount;
        previousKeyIsPressed = new bool[keyCount];
        currentKeyIsPressed = new bool[keyCount];

        isInitialized = true;
    }

    public static void Update()
    {
        if (!isInitialized) { Initialize(); }

        for (int i = 0; i < keyCount; ++i)
        {
            previousKeyIsPressed[i] = currentKeyIsPressed[i];
            currentKeyIsPressed[i] = Keyboard.IsKeyPressed((Keyboard.Key)i);
        }
    }

    public static bool IsPressed(Keyboard.Key key)
    {
        return currentKeyIsPressed[(int)key];
    }

    /// <summary>returns true, if the key is pressed this frame and was not pressed previous frame</summary>
    /// <param name="key">Key to be evaluated</param>
    /// <returns>returns true, if the key is pressed this frame and was not pressed previous frame</returns>
    public static bool Downward(Keyboard.Key key)
    {
        if (!isInitialized) { Initialize(); }

        return !previousKeyIsPressed[(int)key] && currentKeyIsPressed[(int)key];
    }

    /// <summary>returns true, if the key is not pressed this frame and was pressed previous frame</summary>
    /// <param name="key">Key to be evaluated</param>
    /// <returns>returns true, if the key is not pressed this frame and was pressed previous frame</returns>
    public static bool Upward(Keyboard.Key key)
    {
        if (!isInitialized) { Initialize(); }

        return previousKeyIsPressed[(int)key] && !currentKeyIsPressed[(int)key];
    }
}
