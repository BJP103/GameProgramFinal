using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    [Header("Bobbing Settings")]
    public float bobSpeed = 5f;       // how fast it bobs (linked to movement)
    public float bobAmount = 0.04f;     // how much it moves up/down
    public float swayAmount = 0.04f;    // optional left/right sway

    private float timer = 0f;
    private Vector3 initialPos;

    void Start()
    {
        initialPos = transform.localPosition;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Are we moving?
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            timer += Time.deltaTime * bobSpeed;
            float bobX = Mathf.Cos(timer) * swayAmount;
            float bobY = Mathf.Sin(timer * 2) * bobAmount; // double speed for up/down

            Vector3 targetPos = initialPos + new Vector3(bobX, bobY, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 8f);
        }
        else
        {
            // Return smoothly to rest position when not moving
            timer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPos, Time.deltaTime * 8f);
        }
    }
}
