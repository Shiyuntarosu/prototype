using UnityEngine;

[System.Serializable]
public class TextLine
{
    public string id;
    public string speaker;
    public string text;
    public string se;
    public bool cameraShake;
    public float textSpeed;
    public string textEffect;
}

[System.Serializable]
public class TextData
{
    public TextLine[] lines;
}