using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PointClickMovement : MonoBehaviour {
    [SerializeField] private Transform target;

    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;

    private CharacterController _charController;

    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    public float deceleration = 25.0f;
    public float targetBuffer = 1.75f;
    private float _curSpeed = 0f;
    private Vector3 _targetPos = Vector3.one;

    private float _vertSpeed;

    private ControllerColliderHit _contact;//存储碰撞数据

    private Animator _animator;

    public float pushForce = 3.0f;

    // Use this for initialization
    void Start () {
        //Debug.Log(Time.deltaTime);
        _charController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _vertSpeed = minFall;
    }
    
    // Update is called once per frame
    void Update () {
        Vector3 movement = Vector3.zero;
        /*float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        if (horInput != 0 || vertInput != 0) {//做转向
            movement.x = horInput * moveSpeed;
            movement.z = vertInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);
            Quaternion tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmp;
            //transform.rotation = Quaternion.LookRotation(movement);
            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }*/
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) {//鼠标单击且鼠标不在UI上
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//鼠标位置发射射线
            RaycastHit mouseHit;
            if (Physics.Raycast(ray, out mouseHit)) {
                GameObject hitObject = mouseHit.transform.gameObject;
                if (hitObject.layer == LayerMask.NameToLayer("Ground")) {//点击到地面才移动
                    _targetPos = mouseHit.point;
                    _curSpeed = moveSpeed;
                    //Debug.Log("单机碰撞点： " + mouseHit.point);
                }
            }
        }
        if (_targetPos != Vector3.one) {//如果设置了目标移动位置,则移动
            Vector3 adjustedPos = new Vector3(_targetPos.x, transform.position.y, _targetPos.z);
            Quaternion targetRot = Quaternion.LookRotation(adjustedPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);//转向目标
            movement = _curSpeed * Vector3.forward;
            movement = transform.TransformDirection(movement);//转换成世界坐标系
            //Debug.Log(movement);
            if (Vector3.Distance(_targetPos, transform.position) < targetBuffer) {
                _curSpeed -= deceleration * Time.deltaTime;//接近目标时减速到0
                if (_curSpeed <= 0) {
                    _targetPos = Vector3.one;
                }
            }
        }
        _animator.SetFloat("Speed", movement.sqrMagnitude);
        bool hitGround = false;
        RaycastHit hit;
        if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit)) {
            float check = (_charController.height + _charController.radius)/1.9f;
            hitGround = (hit.distance <= check);
        }
        if (hitGround) {//在地面上。用射线投射结果代替_charController.isGrounded检查
            if (Input.GetButtonDown("Jump")) {
                _vertSpeed = jumpSpeed;
            } else {
                _vertSpeed = minFall;
                _animator.SetBool("Jumping", false);
            }
        } else {
            _vertSpeed += gravity * 5 * Time.deltaTime;
            if (_vertSpeed < terminalVelocity) {
                _vertSpeed = terminalVelocity;
            }
            if (_contact != null) {
                _animator.SetBool("Jumping", true);
            }
            if (_charController.isGrounded) {//射线没有检测刀地面，但胶囊体接触到了地面
                if (Vector3.Dot(movement, _contact.normal) < 0) {
                    movement = _contact.normal * 0;
                } else {
                    movement += (_contact.normal * moveSpeed);
                }
            }
        }


        movement.y = _vertSpeed;
        movement *= Time.deltaTime;
        _charController.Move(movement);
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        _contact = hit;
        //Debug.Log(_contact.normal);
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic) {
            body.velocity = hit.moveDirection * pushForce;
        }
    }
}
