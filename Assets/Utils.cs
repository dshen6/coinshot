using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

    public static bool checkBoundedInDirection(Vector2 directionToCheck, float distanceToHit, Vector2 origin) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, directionToCheck);
        foreach (RaycastHit2D hit in hits) {
            if (!hit.collider.CompareTag("Stage")) {
                continue;
            }
            if (hit.distance < distanceToHit) {
                return true;
            }
        }
        return false;
    }
}
