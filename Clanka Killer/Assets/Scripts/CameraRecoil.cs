using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    public Vector2 recoilAmount = new Vector2(2f, 1f); // x = vertical, y = horizontal
    public float returnSpeed = 8f;
    public float snappiness = 10f;

    private Vector2 targetRotation;
    private Vector2 currentRotation;

    void Update()
    {
        targetRotation = Vector2.Lerp(targetRotation, Vector2.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector2.Lerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(new Vector3(-currentRotation.x, currentRotation.y, 0f));
    }

    public void ApplyRecoil()
    {
        targetRotation += new Vector2(recoilAmount.x, Random.Range(-recoilAmount.y, recoilAmount.y));
    }
}
