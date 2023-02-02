using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedTool : MonoBehaviour {
    [SerializeField] private SeedEntity m_seedEntityPrefab;
    // Camera cache
    private Camera m_cam;

    private void Awake() {
        m_cam = Camera.main;
    }

    // private void OnMouseOver() {
    //     // TODO: check if player has any seeds
    //     Debug.Log("SeedTool OnMouseOver");
    //     if (Input.GetMouseButtonDown(0)) {
    //         RetrieveSeed();
    //     }
    // }

    public void RetrieveSeed() {
        // TODO: check if player has any seeds
        Instantiate(m_seedEntityPrefab, Utils.GetMouseWorldPos(), Quaternion.identity);
    }
}
