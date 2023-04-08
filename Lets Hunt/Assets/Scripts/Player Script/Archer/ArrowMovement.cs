/*using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public float arrowSpeed = 20f;
    public float arrowLifetime = 5f;
   // public GameObject hitEffectPrefab;

    private Vector3 _direction;
    public bool _hasHit;

    public void SetTarget(Transform enemy)
    {
        _direction = (enemy.position - transform.position).normalized;
        transform.LookAt(enemy);
    }

    private void Update()
    {
        if (!_hasHit)
        {
            transform.Translate(_direction * arrowSpeed * Time.deltaTime, Space.World);
            Destroy(gameObject, arrowLifetime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasHit && other.CompareTag("Enemy"))
        {
            _hasHit = true;
            //Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}*/


using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    public float arrowSpeed = 5f;

    public bool triggered = false;


    private void Update()
    {
        transform.Translate(Vector3.forward * arrowSpeed * Time.deltaTime);

        Destroy(gameObject,3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            triggered = true;
            Destroy(gameObject);
        }
        else
        {
            triggered = false;
        }
        
    }
}
