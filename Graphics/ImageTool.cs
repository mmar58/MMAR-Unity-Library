
using System;
using UnityEngine;

public class ImageTool
{
    public static string ConvertTextureToString(Texture2D texture)
    {
        byte[] bytes=texture.EncodeToPNG();
        return Convert.ToBase64String(bytes);
    }

    public static Texture2D ConvertStringToTexture(string textureText)
    {
        byte[] bytes=Convert.FromBase64String(textureText);
        Texture2D texture = new(1,1);
        texture.LoadImage(bytes);
        return texture;
    }
}
