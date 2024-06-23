using Services;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private PlayerController Player;
    
    private const string PROP_NAME_FILL = "_Fill";
    private int PROP_ID_FILL;
    
    private MaterialPropertyBlock PropertyBlock;
    private Material Renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        PropertyBlock = new MaterialPropertyBlock();
        ServiceLocator.TryGetService(out Player);
        PROP_ID_FILL = Shader.PropertyToID(PROP_NAME_FILL);
        Renderer = GetComponent<Image>().material;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"{Player.Health} / {Player.MaxHealth}");
        Renderer.SetFloat(PROP_ID_FILL, Mathf.Clamp01((float)Player.Health/Player.MaxHealth));
    }
}
