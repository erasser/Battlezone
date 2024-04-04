using UnityEngine;

public class Fragments : MonoBehaviour
{
    public float force = 1000;
    public float rotationForce = 100;

    void Start()
    {
        float offset = .2f;

        foreach (Transform fragmentTransform in transform)
        {
            Vector3 explosionDirection = new(Random.Range(- offset, offset), Random.Range(offset / 5, offset * 10), Random.Range(- offset, offset));
            fragmentTransform.localPosition += explosionDirection;
            fragmentTransform.localRotation = Random.rotation;
            
            // Vector3 explosionDirection = fragmentTransform.position - transform.position;
            var rb = fragmentTransform.GetComponent<Rigidbody>();
            
            rb.AddRelativeForce(explosionDirection.normalized * Random.Range(force / 4, force));
            rb.AddTorque(Random.insideUnitSphere * Random.Range(0, rotationForce));


            /*
            print(fragmentTransform.name);
            Vector3 explosionDirection = new(Random.Range(-offset, offset), Random.Range(offset / 5, offset), Random.Range(-offset, offset));
            fragmentTransform.SetParent(null);
            fragmentTransform.position += explosionDirection;
            fragmentTransform.rotation = Random.rotation;

            // Vector3 explosionDirection = fragmentTransform.position - transform.position;
            var rb = fragmentTransform.GetComponent<Rigidbody>();
            
            rb.AddForce(explosionDirection.normalized * force);
            rb.AddTorque(Random.insideUnitSphere * rotationForce);

            // tr.GetComponent<Rigidbody>().AddExplosionForce(force, transform.position, 1);
            */
        }

        // Destroy(gameObject);
    }
}
