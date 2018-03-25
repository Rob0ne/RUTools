using UnityEngine;
using RUT.Tools.Collision;

/// <summary>
/// TargetExample1 class.
/// </summary>
public class TargetExample1 : MonoBehaviour, ITargetable
{
    #region Public properties
    public ParticleSystem[] hitFX;

    public ITargetable Target
    {
        get
        {
            return this;
        }
    }
    #endregion

    #region Private properties
    private float[] _hitTime;
    #endregion

    #region API
    public void GetHit(int code)
    {
        if (hitFX.Length <= code || _hitTime.Length <= code)
            return;

        hitFX[code].Play();

        switch (code)
        {
            case 0:
                _hitTime[code] = 2; //looped fx
                break;
            case 1:
                _hitTime[code] = -1; //Oneshot fx
                break;
        }
    }
    #endregion

    #region Unity
    private void Awake()
    {
        _hitTime = new float[hitFX.Length];
    }

    private void Update()
    {
        for(int i = 0; i < hitFX.Length; ++i)
        {
            if(_hitTime.Length > i && _hitTime[i] > 0)
            {
                _hitTime[i] -= Time.deltaTime;

                if (_hitTime[i] <= 0)
                {
                    _hitTime[i] = 0;
                    hitFX[i].Stop();
                }
            }
        }
    }

    #endregion
}
