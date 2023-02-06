using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestedSeed : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Rigidbody2D m_rb;
    [SerializeField] private GameObject m_normalSpriteObj;
    [SerializeField] private GameObject m_rareSpriteObj;

    [Header("Physics Values")]
    [SerializeField, Tooltip("Range of seconds before this seed \"hits the ground\".")]
    private RandomRange m_timeBeforeHittingGround;
    [SerializeField, Tooltip("The duration of time during which this seed rests on the ground before flying to the seed collection.")]
    private float m_timeBeforeFlight;
    [SerializeField] private RandomRange yForce;
    [SerializeField] private float maxForceX;
    [SerializeField] private float m_maxTorque;
    private float m_startingY;
    [SerializeField, Tooltip("The maximum distance a seed can fall below the starting y before being stopped.")]
    private float m_maxFallHeight;

    [Space]

    [SerializeField] private float m_flightSpeed;
    private bool m_isRare;

    private void Start() {
        StartCoroutine(ApplyForce());
        StartCoroutine(FlyToSeedCollection());
        m_startingY = transform.position.y;
    }

    private void Update() {
        // Prevent this seed from falling too far
        if (transform.position.y < m_startingY - m_maxFallHeight) {
            StopPhysics();
        }
    }

    // Makes this harvested seed rare
    public void SetToRare() {
        m_isRare = true;
        // Swap sprite
        m_normalSpriteObj.SetActive(false);
        m_rareSpriteObj.SetActive(true);
    }

    private void StopPhysics() {
        m_rb.bodyType = RigidbodyType2D.Static;
    }

    // Apply random force and then stop it to simulate this seed hitting the ground
    private IEnumerator ApplyForce() {
        // Apply random force
        Vector2 force = Vector2.zero;
        force.y = yForce.GetRandom();
        force.x = Random.Range(-maxForceX, maxForceX);
        m_rb.AddForce(force);
        m_rb.AddTorque(Random.Range(-m_maxTorque, m_maxTorque));
        yield return new WaitForSeconds(m_timeBeforeHittingGround.GetRandom());
        StopPhysics();
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
        AudioManager.instance.Play(Constants.instance.COLLECT_SEEDS);
    }
}
