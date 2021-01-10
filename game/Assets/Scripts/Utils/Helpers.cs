using UnityEngine;

public class Helpers {
    public static bool IsInRange(Vector2 v1, Vector2 v2, float ClickMargin) {
        return Vector2.Distance(v1, v2) < ClickMargin;
    }
}
