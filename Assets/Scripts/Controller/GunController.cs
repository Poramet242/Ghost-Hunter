using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public static GunController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] public Animator _animtor;
    [SerializeField] public GameObject _effectShotting;
    public bool isShotting;

    private void Start()
    {
        _animtor.Play("GunStart");
        _animtor.SetBool("Shotting", false);
        _animtor.SetBool("GunStart", true);
        _animtor.SetBool("Idle", false);
    }
    private void Update()
    {
        if (isShotting)
        {
            _animtor.SetBool("Shotting", true);
            _animtor.SetBool("GunStart", false);
            _animtor.SetBool("Idle", false);
        }
        else
        {
            if (ARGameManager.instance.isCatch)
            {
                _animtor.SetBool("Shotting", false);
                _animtor.SetBool("GunStart", false);
                _animtor.SetBool("Idle", false);
                _animtor.SetBool("Down", true);
            }
            else
            {
                _animtor.SetBool("Shotting", false);
                _animtor.SetBool("GunStart", false);
                _animtor.SetBool("Idle", true);
            }
        }
    }
}
