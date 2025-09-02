using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    public Vector3 recoilKick = new Vector3(0f, 0f, -0.2f); // how far gun moves back
    public float recoilSpeed = 8f;   // how fast gun kicks back
    public float returnSpeed = 6f;   // how fast it goes back to normal

    private Vector3 initialPosition;
    private Vector3 currentRecoil;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // Smoothly move back towards original position
        currentRecoil = Vector3.Lerp(currentRecoil, Vector3.zero, returnSpeed * Time.deltaTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + currentRecoil, recoilSpeed * Time.deltaTime);
    }

    // Call this when firing
    public void ApplyRecoil()
    {
        currentRecoil += recoilKick;
    }
}
