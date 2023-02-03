using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DroppableTool : MonoBehaviour {

    private void Update() {
        transform.position = Utils.GetMouseWorldPos();
        if (Input.GetMouseButtonUp(0)) {
            DropItem();
        }
    }

    protected virtual void DropItem() {
        GardenBed bed = GetGardenBedAtDropPoint();
        if (bed == null) {
            ReturnItem();
        } else {
            InteractWithGardenBed(bed);
        }
    }

    // Returns the GardenBed where the tool was used, or null if there isn't one
    protected virtual GardenBed GetGardenBedAtDropPoint() {
        // RaycastHit2D hit = Physics2D.Raycast(Utils.GetMouseWorldPos(), Vector2.zero, 1 << LayerMask.NameToLayer("GardenBeds"));
        RaycastHit2D hit = Physics2D.Raycast(Utils.GetMouseWorldPos(), Vector2.zero);
        if (hit.collider != null) {
            GardenBed bed = hit.transform.GetComponent<GardenBed>();
            Debug.Log ("Dropped on: " + hit.collider.gameObject.name);
            Debug.Log ("Dropped at: " + transform.position);
            return bed;
        }
        return null;
    }

    protected virtual void InteractWithGardenBed(GardenBed bed) {
        
    }

    // Return item if it was dropped on an invalid area
    protected virtual void ReturnItem() {
        
    }
}
