using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public Rigidbody rb;
    public Transform cameraTransform;
    private Animator anim;
    private Renderer rend;

    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private bool isGrounded;

    private float defaultFOV;
    public float runFOV = 80f;
    private Camera cam;

    private float animationSpeed = 0f; //Sincronizada para animaciones

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<Renderer>();

        rb.freezeRotation = true;

        if (!photonView.IsMine)
        {
            cameraTransform.gameObject.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            cam = cameraTransform.GetComponent<Camera>();
            defaultFOV = cam.fieldOfView;
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        MovePlayer();
        RotateCamera();
        HandleRunningFOV();

        if (Input.GetMouseButtonDown(0))
        {
            TryKillTarget();
        }
    }

    void FixedUpdate()
    {
        AnimatePlayer(); //Se ejecuta en todos (local o remoto)
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.linearVelocity = new Vector3(move.x * currentSpeed, rb.linearVelocity.y, move.z * currentSpeed);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }

        animationSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleRunningFOV()
    {
        if (cam == null) return;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && (Input.GetAxis("Vertical") > 0);
        float targetFOV = isRunning ? runFOV : defaultFOV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * 8f);
    }

    public void ChangePlayerColor(Color color)
    {
        if (rend != null)
        {
            rend.material.color = color;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    void TryKillTarget()
    {
        PlayerRole role = GetComponent<PlayerRole>();
        if (role == null || role.role != 2) return; //Solo el asesino puede matar

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, 3f))
        {
            if (hit.collider.CompareTag("NPC"))
            {
                Debug.Log("Asesinaste un NPC");
                Destroy(hit.collider.gameObject);
            }

            PhotonView targetView = hit.collider.GetComponent<PhotonView>();
            if (targetView != null && !targetView.IsMine)
            {
                Debug.Log("Asesinaste un jugador");
                targetView.RPC("Die", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void Die()
    {
        Debug.Log("Moriste");
        gameObject.SetActive(false);
    }

    void AnimatePlayer()
    {
        if (anim != null)
        {
            anim.SetFloat("speed", animationSpeed);
        }
    }

    //Sincroniza la velocidad para animaci�n
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(animationSpeed);
        }
        else
        {
            animationSpeed = (float)stream.ReceiveNext();
        }
    }
}
