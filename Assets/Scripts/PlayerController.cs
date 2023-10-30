using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private const float Speed = 5;
    public  float visionMax = 100;
    public float currentVision = 0;
    private const float RotationSpeed = 80;
    private Camera _mainCamera;
    private bool _losingVision = true;

    private float _yaw;
    private float _pitch;
    
    public UnityEvent<float, float> adjustVision;
    public UnityEvent playerBlind;
    public UnityEvent pLayerHasVision;
    public UnityEvent interactedWithTv;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        if (_mainCamera != null) _pitch = _mainCamera.transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteract();
        if (_losingVision)
        {
            if (currentVision > 0)
            {
                currentVision-=Time.deltaTime;
            }
            else playerBlind.Invoke();
                
        }
        
        if (!_losingVision)
        {
            pLayerHasVision.Invoke();
            if (currentVision < visionMax)
            {
                currentVision+=Time.deltaTime/10f;

            }
                
        }
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * RotationSpeed * Time.deltaTime);
        
        
        if (Input.GetAxis("Vertical") > 0)
        {
            transform.Translate(Vector3.forward * (Speed * Time.deltaTime));
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            transform.Translate(Vector3.back * (Speed * Time.deltaTime));
        }

        
        _pitch -= Speed * Input.GetAxis("Mouse Y");


        var transform1 = _mainCamera.transform;
        var previousAngle = transform1.eulerAngles;
        transform1.eulerAngles = new Vector3(_pitch, previousAngle.y, previousAngle.z);
    }
    
    void CheckInteract()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Check if Enter key is pressed
        {
            RaycastHit hit;
            Ray ray = _mainCamera.ScreenPointToRay(new Vector3(_mainCamera.pixelWidth / 2, _mainCamera.pixelHeight / 2, 0));
            Debug.Log(Screen.width/2 + "," + Screen.height/2);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Object detected and Enter pressed!");
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 1f);
                if (hit.collider.gameObject.CompareTag("TV")) // Check if the hit object is the one you're interested in
                {
                    // Run your code here
                    interactedWithTv.Invoke();
                    Debug.Log("its a tv");
                }
            }
        }
    }

    public void EnteredLightEmitter()
    {
        _losingVision = false;
        currentVision = visionMax;
        adjustVision.Invoke(currentVision, visionMax);
    }
    
    public void LeftLightEmitter()
    {
        _losingVision = true;
    }
}
