using UnityEngine;

public class WeaponSwayRotation : MonoBehaviour
{
    public float amount = 5f;       // rotation amount
    public float smooth = 6f;        // how quickly it follows
    public float maxAmount = 8f;    // clamp

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotX = Mathf.Clamp(-mouseY * amount, -maxAmount, maxAmount);
        float rotY = Mathf.Clamp(mouseX * amount, -maxAmount, maxAmount);

        Quaternion finalRotation = Quaternion.Euler(rotX, rotY, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, initialRotation * finalRotation, Time.deltaTime * smooth);
    }
}
