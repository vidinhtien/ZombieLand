using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    Player player;
    Transform playerTransform;
    [SerializeField]
    Animator animator;
    CharacterController characterController;
    [SerializeField]
    GameObject child;
    [SerializeField]
    Vector3 maxSpeed;
    Transform thisTrans;
    [SerializeField]
    DirectionEffect arrowDirection;
    [SerializeField]
    LayerMask RaycastWallLayer;
    [SerializeField]
    Transform RaycastWallPosition;
    Camera gameCam;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        characterController = GetComponent<CharacterController>();
        v3up = Vector3.up;
        thisTrans = transform;
        playerTransform = transform;
        _timeNotSeeEnemy = 0f;
        child = transform.GetChild(0).gameObject;
    }
    public void Setup()
    {
        playerTransform.gameObject.SetActive(true);
        player = playerTransform.GetComponentInChildren<Player>();
        animator = player.GetComponentInChildren<Animator>();
        player.Setup();
        animator.SetBool("isDeath", false);
        animator.SetBool("isMoving", false);
        _raycastTimeCount = 0;
        isHideByWall = false;
        if (gameCam == null)
        {
            gameCam = UIManager.Instance.GetGameCam();
        }
    }
    public Vector3 GetPlayerPosition()
    {
        return playerTransform.position;
    }
    public void TurnOffPlayer()
    {
        //player.gameObject.SetActive(false);
        child.SetActive(false);
        player.Hide();
    }
    public void TurnOnPlayer()
    {
        child.SetActive(true);
        Setup();
        player.gameObject.SetActive(true);
        player.Setup();
    }
    public void TurnOnPlayer(Vector3 position)
    {
        child.SetActive(true);
        transform.position = position;
        CameraMove.Instance.SetPositionNow(CamPosition);
        if (player == null)
        {
            Setup();
        }
        Setup();
        player.gameObject.SetActive(true);
    }
    Vector3 vectorValue;
    Vector3 velocity;
    float anglefw, anglebw;
    Vector3 v3up;
    [SerializeField]
    float timeDelayDust = 0.5f;
    float delayDust = 0f;
    [SerializeField]
    float timeDelayChangeStateAttack = 0.5f;
    float _timeDelayChangeStateAttack = 0;
    bool lastState = false;
    [SerializeField]
    float _timeNotSeeEnemy = 0f;
    [SerializeField]
    float timeTurnDirectionOn = 5f;
    [SerializeField]
    private Vector3 _camPosOffset;
    public Vector3 CamOffset { get { return _camPosOffset; } }
    public Vector3 CamPosition { get { return transform.position + _camPosOffset; } }
    [SerializeField]
    float raycastDelayTime = 0.5f;
    float _raycastTimeCount;
    public bool isHideByWall { get; private set; }
    private void Update()
    {
        if (player != null && animator != null)
        {
            if (player.isAlive)
            {
                if (player.IsSeeingEnemy)
                {
                    if (player.IsThrowingNade)
                    {
                        animator.SetBool("grenade", true);
                        EffectManage.Instance.TurnOffMuzzle();
                    }
                    else
                    {
                        animator.SetBool("grenade", false);
                        _timeNotSeeEnemy = 0;
                        if (!lastState)
                        {
                            lastState = true;
                            animator.SetBool("isAttack", true);
                            EffectManage.Instance.TurnOnMuzzle();
                            _timeDelayChangeStateAttack = timeDelayChangeStateAttack;
                        }
                    }
                }
                else
                {
                    if (player.IsThrowingNade)
                    {
                        animator.SetBool("grenade", true);
                        EffectManage.Instance.TurnOffMuzzle();
                    }
                    else
                    {
                        animator.SetBool("grenade", false);
                        if (lastState)
                        {
                            lastState = false;
                            animator.SetBool("isAttack", false);
                            EffectManage.Instance.TurnOffMuzzle();
                            _timeDelayChangeStateAttack = timeDelayChangeStateAttack;
                        }
                    }
                    _timeNotSeeEnemy += Time.deltaTime;

                }
                velocity = characterController.velocity;
                vectorValue.x = velocity.x / maxSpeed.x;
                vectorValue.z = velocity.z / maxSpeed.z;
                anglefw = Vector3.SignedAngle(vectorValue.normalized, playerTransform.forward, v3up);
                anglebw = Vector3.SignedAngle(vectorValue.normalized, playerTransform.forward * -1, v3up);
                vectorValue.z = vectorValue.x > vectorValue.z ? vectorValue.x : vectorValue.z;
                if (Mathf.Abs(anglefw) <= 90)
                {
                    vectorValue.z = Mathf.Abs(vectorValue.z);
                    vectorValue.x = anglefw;
                }
                else
                {
                    vectorValue.x = anglebw;
                    vectorValue.z = -Mathf.Abs(vectorValue.z);
                }
                animator.SetFloat("A", vectorValue.x);
                animator.SetFloat("S", vectorValue.z);
                if (player.IsMoving)
                {
                    animator.SetBool("isMoving", true);
                }
                else
                {
                    animator.SetBool("isMoving", false);
                }

            }
            else
            {
                animator.SetBool("isDeath", true);
            }
        }
        if (GameController.Instance.IsPlaying)
        {
            if (_raycastTimeCount > 0)
            {
                _raycastTimeCount -= Time.deltaTime;
            }
            else
            {
                _raycastTimeCount = raycastDelayTime;
                CheckWall();
            }
        }
    }
    private void CheckWall()
    {
        Vector3 dir = RaycastWallPosition.position - gameCam.transform.position;
        Ray ray = new Ray(gameCam.transform.position, dir.normalized);
        //Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow, 5f); ;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, RaycastWallLayer))
        {
            if (hit.transform.CompareTag("wall"))
            {
                MaterialController.Instance.AddWall(hit.transform.GetComponent<MeshRenderer>());
            }
            else
            {
                MaterialController.Instance.RemoveWalls();
            }
        }
        else
        {
            MaterialController.Instance.RemoveWalls();
        }
    }

}
