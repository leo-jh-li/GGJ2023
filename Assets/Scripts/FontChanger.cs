using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

#if UNITY_EDITOR
public class FontChanger : Singleton<FontChanger> {

    public List<TextMeshProUGUI> texts;
    public TMP_FontAsset newFont;

    [MenuItem("Tools/Update Font")]
    public static void UpdateFont() {
        foreach (TextMeshProUGUI text in instance.texts) {
            text.font = instance.newFont;
        }
    }
}
#endif