using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

    // Returns the world position the mouse is on
    public static Vector3 GetMouseWorldPos() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    // Peforms a Fisher-Yates-Durstenfeld shuffle on the given list
    public static void Shuffle<T>(this IList<T> list) {
        int len = list.Count;
        for (int i = 0; i < len - 1; i++) {
            int swapIndex = Random.Range(i, len);
            var temp = list[i];
            list[i] = list[swapIndex];
            list[swapIndex] = temp;
        }
    }
}