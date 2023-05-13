using System.Collections;
using UnityEngine;
using System;

public class ProjectileFadeAnimator : MonoBehaviour
{
    private MeshRenderer _renderer;
    private Material[] _renderMats;

    private Coroutine _fadeCoroutine;
    public Action fadeComplete;

    private void OnEnable()
    {
        if (!_renderer)
        {
            gameObject.TryGetComponent(out _renderer);
        }

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }
    }

    #region "Public Functions"
    /// <summary>
    /// Fades the projectiles model out over the given time. publishes fadecomplete when its done
    /// </summary>
    /// <param name="duration"> time it takes for the mesh to fade </param>
    public void FadeProjectile(float duration)
    {
        if (_fadeCoroutine == null)
        {
            StartCoroutine(ProjectileFadeout(duration));
        }
    }
    /// <summary>
    /// Resets the disolve, makes model visible again
    /// </summary>
    public void ResetFade()
    {
        if (_renderer)
        {
            _renderMats = _renderer.materials;

            if (_renderMats != null)
            {
                for (int i = 0; i < _renderMats.Length; i++)
                {
                    if (_renderMats[i].HasProperty("_Dissolve"))
                    {
                        _renderMats[i].SetFloat("_Dissolve", 0);
                    }
                }
            }

        }
    }
    #endregion

    #region "Local Functions"
    // Coroutine for slowly fading out meshes using the shader
    private IEnumerator ProjectileFadeout(float duration)
    {
        if(_renderer)
        {
            _renderMats = _renderer.materials;

            if(_renderMats != null)
            {
                for (float t = 0; t < duration; t += Time.deltaTime)
                {
                    for (int i = 0; i < _renderMats.Length; i++)
                    {
                        if (_renderMats[i].HasProperty("_Dissolve"))
                        {
                            _renderMats[i].SetFloat("_Dissolve", t / duration);
                        }
                    }

                    yield return null;
                }
            }
        }

        PublishComplete();
    }
    //publishes event for fade complete when ProjectileFadeout finishes
    private void PublishComplete()
    {
        fadeComplete?.Invoke();
    }
    #endregion
}