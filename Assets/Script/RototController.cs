using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class RototController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Camera _camera;

    [Header("CannonParameters")] [SerializeField] private Transform _cannon;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField, ColorUsage(true, true)] private Color _defaultLazerColor;
    [SerializeField, ColorUsage(true, true)] private Color _fireLazerColor;
    [SerializeField] private GameObject _prfFireImpact;

    private bool _isFire;
    RaycastHit hit;
    void Start() {
        _lineRenderer.startColor =_defaultLazerColor;
        _lineRenderer.endColor =_defaultLazerColor;
    }

    void Update() {
        //ManageMovement();
        ManageAim();
        ManageFire();
    }

    private void ManageAim() {
        
        if (Physics.Raycast(_camera.ScreenPointToRay(Mouse.current.position.value),  out hit)) {
            _cannon.forward = hit.point - _cannon.position;
            _lineRenderer.SetPosition(0, _cannon.position);
            _lineRenderer.SetPosition(1, hit.point);
        }
        
    }

    private void ManageFire() {
        if (Mouse.current.leftButton.isPressed) {
            if (!_isFire) {
                _isFire = true;
                OnStartFire();
            }
        }
        else if (_isFire && !Mouse.current.rightButton.isPressed) {
            _isFire = false;
            OnEndFire();
        }
    }

    private void OnStartFire() {
        if( _prfFireImpact){ 
            GameObject go = Instantiate(_prfFireImpact, hit.point, Quaternion.identity);
            go.transform.up = hit.normal;
        }

        if (hit.collider.GetComponent<IDamagable>() != null) {
            IDamagable target = hit.collider.GetComponent<IDamagable>();
            target.TakeDamage(1, hit.point, hit.normal);
        }
        _lineRenderer.startColor =_fireLazerColor;
        _lineRenderer.endColor =_fireLazerColor;
    }

    private void OnEndFire()
    {
        _lineRenderer.startColor =_defaultLazerColor;
        _lineRenderer.endColor =_defaultLazerColor;
    }
    

    //private void ManageMovement()
    //{
    //    Vector2 moveInputVec =InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
    //    Vector3 moveVec = new Vector3(0,0,moveInputVec.y);
    //    moveVec *= _moveSpeed;
    //    moveVec*=Time.deltaTime;
    //    transform.Translate(moveVec);
//
    //    float rotation = moveInputVec.x;
    //    rotation *= _rotateSpeed;
    //    rotation *= Time.deltaTime;
    //    transform.Rotate(transform.up, rotation);
    //}
}