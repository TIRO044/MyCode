using UnityEngine;

public static class SceneBaseEditorUtil
{
    private static GUIStyle _boxGuiStyle;
    public static GUIStyle BoxStyle
    {
        get
        {
            if (_boxGuiStyle == null)
            {
                _boxGuiStyle = new(GUI.skin.box);
            }

            return _boxGuiStyle;
        }
    }

    private static GUIStyle _textAreaStyle;
    public static GUIStyle TextStyle
    {
        get
        {
            if (_textAreaStyle == null)
            {
                _textAreaStyle = new(GUI.skin.textArea) { alignment = TextAnchor.MiddleCenter };
            }

            return _textAreaStyle;
        }
    }

    private static GUIStyle _buttonStyle;
    public static GUIStyle ButtonStyle
    {
        get
        {
            if (_buttonStyle == null)
            {
                _buttonStyle = new(GUI.skin.button);
            }

            return _buttonStyle;
        }
    }

}
