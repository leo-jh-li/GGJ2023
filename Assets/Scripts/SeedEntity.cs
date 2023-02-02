using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedEntity : MonoBehaviour {

    // Camera cache
    private Camera m_cam;

    private void Awake() {
        m_cam = Camera.main;
    }

    private void Update() {
        transform.position = Utils.GetMouseWorldPos();
        if (Input.GetMouseButtonUp(0)) {
            Debug.Log("drop seed");
            DropSeed();
        }
    }

    private void DropSeed() {
        // TODO: replace with Utils.GetMouseWorldPos()?
        RaycastHit2D hit = Physics2D.Raycast(Utils.GetMouseWorldPos(), Vector2.zero, 1 << LayerMask.NameToLayer("GardenBeds"));
        if (hit.collider != null) {
            GardenBed bed = hit.transform.GetComponent<GardenBed>();
            Debug.Log ("Dropped on: " + hit.collider.gameObject.name);
            Debug.Log ("Dropped at: " + transform.position);
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
        // TODO: return seed to collection, i.e., increase seed quantity by 1
        Destroy(gameObject);
    }
}
