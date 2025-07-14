using UnityEngine;

public class VoyagerInitializer : MonoBehaviour
{
    private void Awake()
    {
        var rb = GetComponent<Rigidbody>();
        rb.AddForceAtPosition(Vector3.forward * .15f, transform.position + Vector3.up *.1f, ForceMode.VelocityChange);
        rb.AddTorque(Vector3.left * .3f + Vector3.up*.5f, ForceMode.VelocityChange);;
    }
}
