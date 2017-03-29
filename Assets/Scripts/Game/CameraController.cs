using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float scrollSpeed;
    [SerializeField]
    private float maxCameraHeight;
    [SerializeField]
    private float minCameraHeight;


    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float rotationLimit;

    private float mouseScroll;

    private float rotationX;
    private float rotationY;
    private float rotationZ;


    void Start()
    {
        rotationX = transform.localEulerAngles.x;
    }

	void Update () {
        mouseScroll = Input.GetAxisRaw("Mouse ScrollWheel");

        if (mouseScroll != 0)
        {
            Vector3 _pos = transform.position + (transform.forward * mouseScroll * scrollSpeed);

            if (_pos.y > minCameraHeight && _pos.y < maxCameraHeight)
            {
                transform.position = _pos;
            }
                
        }

        if (Input.GetMouseButton(2))
        {
            float _xMov = Input.GetAxisRaw("Mouse X");
            float _zMov = Input.GetAxisRaw("Mouse Y");

            Vector3 _pos = transform.forward;
            _pos.y = 0;

            _pos = (_pos * moveSpeed * -_zMov) + transform.position + (transform.right * moveSpeed * -_xMov);

            if (_pos.y > minCameraHeight && _pos.y < maxCameraHeight)
            {
                transform.position = _pos;
            }
           
        } 
        else if (Input.GetMouseButton(1))
        {
            float _yRot= Input.GetAxisRaw("Mouse X");
            float _xRot = Input.GetAxisRaw("Mouse Y");


            rotationX -= (_xRot*rotationSpeed);
            rotationX = Mathf.Clamp(rotationX, -rotationLimit, rotationLimit);

            rotationY = transform.localEulerAngles.y;
            rotationY -= (_yRot * -rotationSpeed);

            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0f);
        }
	}
}
