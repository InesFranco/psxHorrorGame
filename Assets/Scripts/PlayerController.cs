using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float sanity = 100;
    private const float Speed = 5;
    public const float VisionMax = 5;
    public float currentVision;
    private const float RotationSpeed = 80;
    private Camera _mainCamera;
    private bool _losingVision = true;
    private bool carryingMushroom = false;

    private float _yaw;
    private float _pitch;
    
    public UnityEvent<float, float> adjustVision;
    public UnityEvent playerBlind;
    public UnityEvent pLayerHasVision;
    public UnityEvent interactedWithTv;
    private int layerMask ;
    private GameObject mushroom;
    public GameObject mushroomsParent;


    // Start is called before the first frame update
    void Start()
    {
        layerMask = ~(1 << LayerMask.NameToLayer("Orbs"));
        currentVision = VisionMax;
        _mainCamera = Camera.main;
        if (_mainCamera != null) _pitch = _mainCamera.transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(carryingMushroom && Input.GetKeyDown(KeyCode.E)){
            mushroom.transform.SetParent(mushroomsParent.transform);
            mushroom.transform.SetPositionAndRotation(new Vector3(_mainCamera.transform.position.x + _mainCamera.transform.forward.x * 5, 0,_mainCamera.transform.position.z + _mainCamera.transform.forward.z * 5) , mushroomsParent.transform.rotation);
            carryingMushroom = false;
        }
        else if(!carryingMushroom)CheckInteract();
        if (_losingVision)
        {
            if (currentVision > 0)
            {
                currentVision-=Time.deltaTime/10;
            }
            else playerBlind.Invoke();
                
        }
        
        if (!_losingVision)
        {
            pLayerHasVision.Invoke();
            if (currentVision <= VisionMax)
            {
                currentVision+=Time.deltaTime/10;

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
            Ray ray = _mainCamera.ScreenPointToRay(new Vector3(_mainCamera.pixelWidth / 2, _mainCamera.pixelHeight / 2, 3));
            if (Physics.Raycast(ray, out hit,Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 3f);
                if (hit.collider.gameObject.CompareTag("TV")) // Check if the hit object is the one you're interested in
                {
                    // Run your code here
                    interactedWithTv.Invoke();
                    Debug.Log("its a tv");
                }
                else if (hit.collider.gameObject.CompareTag("Mushroom")) // Check if the hit object is the one you're interested in
                {
                    mushroom = hit.collider.gameObject;
                    mushroom.transform.SetParent(transform);
                    Vector3 newPosition = _mainCamera.transform.position + _mainCamera.transform.forward * 5;
                    mushroom.transform.SetPositionAndRotation(newPosition, Quaternion.identity);
                    carryingMushroom = true;
                    
                    Debug.Log("its a mushroom");
                }
            }
        }
    }

    public void EnteredLightEmitter()
    {
        _losingVision = false;
        currentVision = VisionMax;
        adjustVision.Invoke(currentVision, VisionMax);
    }
    
    public void LeftLightEmitter()
    {
        _losingVision = true;
    }
}
