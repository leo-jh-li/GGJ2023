using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestedSeed : MonoBehaviour {
    [SerializeField, Tooltip("The duration of time during which this seed rests on the ground before flying to the seed collection.")]
    private float m_timeBeforeFlight;
    [SerializeField] private float m_flightSpeed;
    [SerializeField] private GameObject m_normalSpriteObj;
    [SerializeField] private GameObject m_rareSpriteObj;
    private bool m_isRare;

    private void Start() {
        StartCoroutine(FlyToSeedCollection());
    }

    // Makes this harvested seed rare
    public void SetToRare() {
        m_isRare = true;
        // Swap sprite
        m_normalSpriteObj.SetActive(false);
        m_rareSpriteObj.SetActive(true);
    }

    private IEnumerator FlyToSeedCollection() {
        yield return new WaitForSeconds(m_timeBeforeFlight);
        Vector3 destination = UIManager.instance.GetSeedToolPos(m_isRare);
        while (transform.position != destination) {
            transform.position = Vector3.MoveTowards(transform.position, destination, m_flightSpeed * Time.deltaTime);
            yield return null;
        }
        UIManager.instance.SeedToolPulse(m_isRare);
        Destroy(gameObject);
        if (m_isRare) {
            GameManager.instance.rareSeedQuantity++;
        } else {
            GameManager.instance.seedQuantity++;
        }
    }
}
