using UnityEngine;

public class DataCollectible : MonoBehaviour
{
    [SerializeField] private ItemData data;
    [SerializeField] private Renderer visual;

    public ItemData Data => data;

    private void Awake()
    {
        var properties = new MaterialPropertyBlock();
        properties.SetColor("_BaseColor", data.Color);
        visual.SetPropertyBlock(properties);
    }

    private void Update()
    {
        visual.transform.Rotate(0f, 60f * Time.deltaTime, 0f);
    }
}
