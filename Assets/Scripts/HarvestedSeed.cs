using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestedSeed : MonoBehaviour {
    [SerializeField, Tooltip("The duration of time during which this seed rests on the ground before flying to the seed collection.")]
    private float m_timeBeforeFlight;
    [SerializeField]
    private float m_flightSpeed;
    
    private void Start() {
        StartCoroutine(FlyToSeedCollection());
    }

    private IEnumerator FlyToSeedCollection() {
        yield return new WaitForSeconds(m_timeBeforeFlight);
        Vector3 destination = UIManager.instance.GetSeedToolPos();
        while (transform.position != destination) {
            transform.position = Vector3.MoveTowards(transform.position, destination, m_flightSpeed * Time.deltaTime);
            yield return null;
        }
        UIManager.instance.SeedToolPulse();
        Destroy(gameObject);
        GameManager.instance.seedQuantity++;
    }
}
