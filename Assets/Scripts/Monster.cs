using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour
{
    public bool isActivated = false;
    private AudioSource audioSource;
    public GameObject smoke;
    public UnityEvent destroyingMonster;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

     void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider has the tag "mushroom"
        if (other.CompareTag("Mushroom"))
        {
            // The collider with the "mushroom" tag entered the trigger zone
            // Your code here
            StartCoroutine(DeactivateEnemy());
        }
    }

    private bool isBeingDestroyed;
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("TV") && other.GetComponent<TvController>().isOn && !isBeingDestroyed){
            StartCoroutine(DeactivateEnemy());
        }
    }

    private IEnumerator DeactivateEnemy()
    {
        isBeingDestroyed = true;
        audioSource.Play();
        GameObject smokeInst = Instantiate(smoke, transform.position, transform.rotation);
        yield return new WaitForSeconds(2f);
        destroyingMonster.Invoke();
        Destroy(gameObject);
        Destroy(smokeInst, 2f);
    }

}
