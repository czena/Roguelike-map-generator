using UnityEngine;

public abstract class BaseObjects : MonoBehaviour
{
    protected int _layer;

    public int Layer
    {
        get { return _layer; }
        set
        {
            _layer = value;
            SetLayers(transform, _layer);
        }
    }

    private string _name;

    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            name = _name;
        }
    }

    protected bool _isVisible;

    public bool IsVisible
    {
        get
        {
            if (!Renderer)
                return false;
            return Renderer.enabled;
        }
        set
        {
            _isVisible = value;
            SetVisibility(transform, _isVisible);
        }
    }

    protected Rigidbody2D _rigidbody2d;

    public Rigidbody2D Rigidbody2d
    {
        get { return _rigidbody2d; }
    }

    public Material Material { get; protected set; }

    public GameObject GameObject { get; protected set; }

    public Renderer Renderer { get; protected set; }

    public SpriteRenderer SpriteRenderer { get; protected set; }

    protected Sprite _sprite;

    public Sprite Sprite
    {
        get { return _sprite; }
        set
        {
            _sprite = value;
            SpriteRenderer.sprite = _sprite;
        }
    }

    protected Color _color;

    public Color Color
    {
        get { return _color; }
        set
        {
            _color = value;
            Material.color = _color;
        }
    }

    protected virtual void Awake()
    {
        GameObject = GetComponent<GameObject>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        Name = name;
        _layer = gameObject.layer;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Renderer = GetComponent<Renderer>();
        if (Renderer)
            Material = Renderer.material;
        if (Material)
            _color = Material.color;
    }

    private void SetLayers(Transform objTransform, int layer)
    {
        objTransform.gameObject.layer = layer;
        foreach (Transform c in objTransform)
        {
            SetLayers(c, layer);
        }
    }

    private void SetVisibility(Transform objTransform, bool visible)
    {
        var rend = objTransform.GetComponent<Renderer>();
        if (rend)
            rend.enabled = visible;
        foreach (var r in GetComponentsInChildren<Renderer>(true))
            r.enabled = visible;
    }
}