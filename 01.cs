using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // 移动参数
    [Header("Movement Settings")]
    public float moveSpeed = 5f;      // 移动速度
    public float gravity = -9.81f;    // 重力加速度
    public float jumpHeight = 2f;     // 跳跃高度

    [Header("Ground Check")]
    public Transform groundCheck;     // 地面检测点
    public float groundDistance = 0.4f; // 检测距离
    public LayerMask groundMask;      // 地面层级

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Transform cameraTransform; // 主相机参考

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform; // 获取主相机
    }

    void Update()
    {
        // 地面检测
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // 重置垂直速度当接触地面时
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 获取输入
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 根据相机方向计算移动方向
        Vector3 move = cameraTransform.right * x + cameraTransform.forward * z;
        move.y = 0; // 确保水平移动
        
        // 应用移动速度
        controller.Move(move.normalized * moveSpeed * Time.deltaTime);

        // 跳跃处理
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 应用重力
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 角色面朝移动方向（可选）
        if (move != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(move),
                Time.deltaTime * 10f
            );
        }
    }
}