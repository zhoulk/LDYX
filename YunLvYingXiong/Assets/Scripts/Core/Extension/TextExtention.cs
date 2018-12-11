using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TextExtention {

    /// <summary>
    /// ·µ»ØÎÄ×Ö¿í¶È
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static float TextWidth(this Text text)
    {
        Font myFont = text.font;  //chatText is my Text component
        myFont.RequestCharactersInTexture(text.text, text.fontSize, text.fontStyle);
        CharacterInfo characterInfo = new CharacterInfo();

        char[] arr = text.text.ToCharArray();

        float totalLength = 0;
        foreach (char c in arr)
        {
            myFont.GetCharacterInfo(c, out characterInfo, text.fontSize);

            totalLength += characterInfo.advance;
        }
        return totalLength;
    }
}
