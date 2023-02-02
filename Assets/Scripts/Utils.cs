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
}