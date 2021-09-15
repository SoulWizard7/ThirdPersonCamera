using System.Collections;
using UnityEngine;

public class ObjectCanFade : MonoBehaviour
{
    [Header("OBJECT MUST BE DISABLED AS STANDARD")]
    
    // This script puts the update responsibility on this object instead of other objects.
    // If an object needs to fade, it gets hit by players raycast and enables, starts its fade function,
    // then fades back when no longer hit by raycast and disables itself until it gets hit again.
    
    private CameraIsObscured _cam;
    private float _fadePercent = 0.1f;
    private float _fadeTime = 0.25f;
    private Color _orginalColor;
    private Color _currentColor;
    private Material _material;

    private void Awake()
    {
        _cam = Camera.main.GetComponent<CameraIsObscured>();
        _material = GetComponent<MeshRenderer>().materials[0];
        _orginalColor = _material.color;
        enabled = false;
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(Fade());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        StartCoroutine(FadeBack());
    }

    private void Update()
    {
        foreach (RaycastHit hit in _cam.hits)
        {
            if (hit.transform == gameObject.transform)
            {
                return;
            }
        }
        this.enabled = false;
    }

    IEnumerator Fade()
    {
        Color fadeColor = new Color(_material.color.r, _material.color.g, _material.color.b, _fadePercent);

        float time = 0;
        float lerpTime = _fadeTime;

        while (time < lerpTime)
        {
            time += Time.deltaTime;
            float t = time / lerpTime;
            _currentColor = Color.Lerp(_orginalColor, fadeColor, t);
            _material.color = _currentColor;
            yield return null;
        }
    }
    
    IEnumerator FadeBack()
    {
        Color tempColor = _material.color;
        
        float time = 0;
        float lerpTime = _fadeTime;

        while (time < lerpTime)
        {
            time += Time.deltaTime;
            float t = time / lerpTime;
            _currentColor = Color.Lerp(tempColor, _orginalColor, t);
            _material.color = _currentColor;
            yield return null;
        }
    }
}
