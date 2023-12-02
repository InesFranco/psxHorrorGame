using UnityEngine;

public class TvController : MonoBehaviour
{
    [SerializeField] private GameObject tvScreen;

    private MeshRenderer _screenMeshRenderer;
    private Light _screenLight;
    private AudioSource _audioSource;
    public bool isOn {get; private set;}
    
    // Start is called before the first frame update
    void Start()
    {
        isOn = true;
        _screenMeshRenderer = tvScreen.GetComponent<MeshRenderer>();
        _screenLight = tvScreen.GetComponent<Light>();
        _audioSource = tvScreen.GetComponent<AudioSource>();
        InvokeRepeating("TurnOff", 10f, 30f);
    }

    public void TurnOnOff()
    {
        if (isOn) TurnOff();
        else TurnOn();
    }

    private void TurnOff()
    {
        isOn = false;
        _screenMeshRenderer.enabled = false;
        _screenLight.enabled = false;
        _audioSource.Stop();
    }

    private void TurnOn()
    {
        isOn = true;
        _screenMeshRenderer.enabled = true;
        _screenLight.enabled = true;
        _audioSource.Play();
    }
}