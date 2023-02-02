using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedEntity : MonoBehaviour {

    private void Update() {
        transform.position = Utils.GetMouseWorldPos();
        if (Input.GetMouseButtonUp(0)) {
            Debug.Log("drop seed");
            DropSeed();
        }
    }

    private void DropSeed() {
        RaycastHit2D hit = Physics2D.Raycast(Utils.GetMouseWorldPos(), Vector2.zero, 1 << LayerMask.NameToLayer("GardenBeds"));
        if (hit.collider != null) {
            GardenBed bed = hit.transform.GetComponent<GardenBed>();
            Debug.Log ("Dropped on: " + hit.collider.gameObject.name);
            Debug.Log ("Dropped at: " + transform.position);
            if (bed == null) { return; }
            if (!bed.HasPlant()) {
                bed.PlantSeed();
                Destroy(gameObject);
                return;
            }
        }
        ReturnSeed();
    }

    private void ReturnSeed() {
        Debug.Log ("Seed returned");
        // Return unused seed to collection
        GameManager.instance.seedQuantity++;
        Destroy(gameObject);
    }
}
