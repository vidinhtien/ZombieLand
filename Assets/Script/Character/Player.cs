using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public static Player Instance;
    Transform thisTransform;
    Transform dadTransform;
    [SerializeField]
    private bool isShooting = false;
    [SerializeField]
    private bool isSeeingEnemy = false;
    public bool IsSeeingEnemy { get { return isSeeingEnemy; } }
    [SerializeField]
    private bool isClickingMouse = false;
    public bool IsMoving { get { return isClickingMouse; } }
    [SerializeField]
    private int _distanceShotZombie = 70;
    public int DistanceShotZombie { get { return _distanceShotZombie; } }
    public Vector3 Velocity { get { return currentVel;/*rgbd.velocity;*/ } }
    [SerializeField]
    float maxAngleGun = 60f;
    [SerializeField]
    BaseCharacter baseChar;
    [SerializeField]
    Transform RayCastPoint;
    CharacterController characterController;
    Rigidbody rgbd;
    Camera mainCam;
    Vector3 aimPos;
    public List<Vector3> enemyPosDic = new List<Vector3>();
    public List<Zombie> enemySaw = new List<Zombie>();
    float zCamPos;
    public LayerMask ZombieLayer;


    Vector3 moveDir, v3tmp1, v3tmp2, v3tmp3;
    [SerializeField]
    private Transform bodyBone;
    [SerializeField]
    Transform gunBarrel2, gunBarrel1;
    [SerializeField]
    int indexZ1 = -1;
    [SerializeField]
    int indexZ2 = -1;
    float disZ1 = -1f;
    float disZ2 = -1f;
    [SerializeField]
    Joystick joystick;
    Transform rightShoulderTransform;
    float angleGunBarrelWithFoward;
    [SerializeField]
    float maxLeftBodyAngle = -45f;
    [SerializeField]
    float maxRightBodyAngle = 45f;
    [SerializeField]
    float angleBodyAndForward;

    internal void Hide()
    {
        healthComponent.HideHealth();
    }

    [SerializeField]
    Transform tpLookAt;
    [SerializeField]
    Transform raycastStartPoint;
    public Vector3 RaycastPosition { get { return raycastStartPoint.position; } }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    Vector3 v3Up;
    Vector3 v3Zero;
    [SerializeField]
    float minMoveJoystick = 0.04f;
    private void Start()
    {
        thisTransform = transform;
        dadTransform = transform;
        mainCam = Camera.main;
        characterController = dadTransform.GetComponent<CharacterController>();
        moveDir = dadTransform.forward;
        v3Down = Vector3.down;
        v3Up = Vector3.up;
        v3Zero = Vector3.zero;
        angleBodyAndForward = Vector3.SignedAngle(thisTransform.forward, bodyBone.forward, v3Up);
        rgbd = dadTransform.GetComponent<Rigidbody>();
        if (baseChar == null)
        {
            baseChar = GetComponentInChildren<BaseCharacter>();
        }
        _isAlive = true;
        //rightShoulderTransform = skinMeshRender.rootBone.
    }
    internal void Setup()
    {
        _isAlive = true;
        isLockTarget = false;
        if (BuffManager.Instance != null)
        {
            healthComponent.MaxHealthPoint = BuffManager.Instance.GetBuffValue(1);
        }
        healthComponent.Setup();
        enemySaw.Clear();
        angleBodyAndForward = Vector3.SignedAngle(thisTransform.forward, bodyBone.forward, v3Up);
    }
    Quaternion a;
    [SerializeField]
    private float timeDelayCalculateDistance = .7f;
    [SerializeField]
    float timeDelayCalculateDistanceCount = 0f;
    bool canCalculate = true;
    [SerializeField]
    int numEnemyRestricted = 100;
    public void RemoveEnenmy(Zombie enemyTrans)
    {
        if (enemySaw.Contains(enemyTrans))
            enemySaw.Remove(enemyTrans);
    }
    [SerializeField]
    float offsetY_CheckRaycastZombie = 0.25f;
    [SerializeField]
    LayerMask RayCastLayer;
    [SerializeField]
    HealthComponent healthComponent;

    Vector3 v3Tmp1;
    Vector3 tmpDir;
    Zombie currentTarget;
    public void SeeEnemy(Zombie zombie)
    {
        //Debug.Log("Check see zombie " + zombie.transform.parent.name);
        if (enemySaw.Contains(zombie) || enemySaw.Count > numEnemyRestricted)
        {
            //Debug.Log("exit 1 ");
            return;
        }
        v3tmp1 = UIManager.Instance.GetGameCam().WorldToViewportPoint(zombie.GetPosition());
        if (v3tmp1.x < 0 || v3tmp1.x > 1 || v3tmp1.y < 0 || v3tmp1.y > 1) return;
        v3tmp1 = zombie.GetPosition();
        v3tmp1.y = RayCastPoint.position.y;
        tmpDir = v3tmp1 - RayCastPoint.position;
        float dis = Vector3.Distance(v3tmp1, RayCastPoint.position);
        if (tmpDir.sqrMagnitude > _distanceShotZombie)
        {
            return;
        }
        //Debug.DrawLine(v3tmp1, RayCastPoint.position, Color.black, 3f);
        Ray ray = new Ray(RayCastPoint.position, tmpDir.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, RayCastLayer))
        {
            // Debug.Log(hit.collider.tag + " ---- " + hit.collider.gameObject.name);
            if (hit.transform.CompareTag("zombie"))
            {
                enemySaw.Add(zombie);
                //Debug.Log("OK " + zombie.transform.parent.name);
            }
            else
            {
                //Debug.Log("exit 4");
            }
        }
        else
        {
            // Debug.Log("exit 5");

        }
    }
    Vector3 rcV3;
    Vector3 rcTmpDir;
    private bool CheckRayCastZombie(Zombie z)
    {
        rcV3 = z.GetPosition();
        rcV3.y = RayCastPoint.position.y;
        rcTmpDir = rcV3 - RayCastPoint.position;
        //Debug.DrawLine(rcV3, RayCastPoint.position, Color.black, 3f);
        Ray ray = new Ray(RayCastPoint.position, rcTmpDir.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, RayCastLayer))
        {
            if (hit.transform.CompareTag("zombie"))
            {
                return true;
            }
        }
        return false;
    }
    Quaternion quaternionTmp;
    [SerializeField]
    float rotateSpeed;
    [SerializeField]
    float lerpRotateSpeed = 0.02f;
    float angleForwardToGun;
    float angleEnemyToForward;
    Vector3 gunBarrelDir;
    [SerializeField]
    bool isLockTarget;
    int numberNeed;
    bool c1, c2;
    [SerializeField]
    float _timeThrowNadeRun;
    [SerializeField]
    float _timeThrowNadeIdle;
    float timeThrowNade;
    public bool IsThrowingNade { get; private set; }
    Vector3 currentVel;
    private void Update()
    {
        if (_isAlive)
        {
            aimPos = Input.mousePosition;
            aimPos.z = mainCam.WorldToScreenPoint(thisTransform.position).z;
            aimPos = mainCam.ScreenToWorldPoint(aimPos);
            aimPos.y = thisTransform.position.y;
            if (joystick.DirectionVector3.sqrMagnitude > minMoveJoystick)
            {
                moveDir = joystick.DirectionVector3.normalized;
                isClickingMouse = true;
            }
            else
            {
                moveDir = v3Zero;
                isClickingMouse = false;
            }
            if (!GameController.Instance.IsPlaying)
            {
                moveDir = v3Zero;
                isClickingMouse = false;
            }
            if (timeThrowNade > 0)
            {
                timeThrowNade -= Time.deltaTime;
            }
            else
            {
                IsThrowingNade = false;
            }
            gunBarrelDir = gunBarrel2.position - gunBarrel1.position;
            angleGunBarrelWithFoward = Vector3.SignedAngle(gunBarrelDir, thisTransform.forward, Vector3.up);
            //Debug.DrawRay(gunBarrel1.position, gunBarrel1.forward * 10, Color.red);
            //Debug.DrawRay(thisTransform.position, thisTransform.forward * Mathf.Sqrt(_distanceShotZombie), Color.blue);
            if (timeDelayCalculateDistanceCount > 0)
            {
                timeDelayCalculateDistanceCount -= Time.deltaTime;
                canCalculate = true;
            }
            numberNeed = GunManage.Instance.currentGun.NumberOfTargetAtSameTime;

            // Tính toán lấy quái gần nhất
            if (canCalculate && enemySaw.Count > 0)
            {
                canCalculate = false;
                enemyPosDic.Clear();
                timeDelayCalculateDistanceCount = timeDelayCalculateDistance;
                indexZ1 = -1;
                indexZ2 = -1;
                numberNeed = numberNeed > 2 ? 2 : numberNeed;
                // không khóa target
                if (isLockTarget && currentTarget != null)
                {
                    if (!currentTarget.gameObject.activeSelf || !currentTarget.isAlive)
                    {
                        isLockTarget = false;
                        canCalculate = true;
                    }
                    tmpF = Vector3.SqrMagnitude(thisTransform.position - currentTarget.GetPosition());
                    c1 = tmpF > _distanceShotZombie;
                    c2 = CheckRayCastZombie(currentTarget);
                    if (c1 || !c2)
                    {
                        isLockTarget = false;
                        canCalculate = true;
                    }
                    else
                    {
                        enemyPosDic.Clear();
                        v3tmp1 = currentTarget.GetPosition() - thisTransform.position;
                        tpLookAt.position = currentTarget.GetPosition();
                        enemyPosDic.Add(currentTarget.GetPosition());

                    }
                }
                else
                {
                    if (isLockTarget)
                    {
                        isLockTarget = false;
                    }
                }
                if (!isLockTarget) // Khóa target
                {
                    // tính
                    for (int i = enemySaw.Count - 1; i >= 0; i--)
                    {
                        if (!enemySaw[i].gameObject.activeSelf)
                        {
                            enemySaw.RemoveAt(i);
                        }
                        else
                        {
                            if (!IsHitTransform(enemySaw[i].transform, _distanceShotZombie))
                            {
                                enemySaw.RemoveAt(i);
                            }
                            else
                            {
                                tmpF = Vector3.SqrMagnitude(thisTransform.position - enemySaw[i].GetPosition());
                                if (tmpF > _distanceShotZombie)
                                {
                                    enemySaw.RemoveAt(i);
                                }
                                else
                                {
                                    if (indexZ1 == -1)
                                    {
                                        indexZ1 = i;
                                        disZ1 = tmpF;
                                    }
                                    else
                                    {
                                        if (indexZ2 == -1)
                                        {
                                            indexZ2 = i;
                                            disZ2 = tmpF;
                                        }
                                        else
                                        {
                                            if (tmpF < disZ1)
                                            {
                                                indexZ1 = i;
                                                disZ1 = tmpF;
                                            }
                                            else
                                            {
                                                if (tmpF < disZ2)
                                                {
                                                    disZ2 = tmpF;
                                                    indexZ2 = i;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (indexZ1 != -1 && indexZ1 < enemySaw.Count)
                    {
                        enemyPosDic.Clear();
                        isSeeingEnemy = true;
                        currentTarget = enemySaw[indexZ1];
                        isLockTarget = true;
                        v3tmp1 = enemySaw[indexZ1].GetPosition() - thisTransform.position;
                        tpLookAt.position = enemySaw[indexZ1].GetPosition();
                        Debug.DrawRay(thisTransform.position, v3tmp1 * 10, Color.cyan);
                        enemyPosDic.Add(enemySaw[indexZ1].GetPosition());
                        if (indexZ2 != -1 && numberNeed > 1)
                        {
                            v3tmp2 = enemySaw[indexZ2].GetPosition() - thisTransform.position;
                            enemyPosDic.Add(enemySaw[indexZ2].GetPosition());
                            v3tmp1 += v3tmp2;
                            a = Quaternion.LookRotation(v3tmp1);
                            v3tmp1 = a.eulerAngles;
                            v3tmp1.x = 0;
                            v3tmp1.z = 0;
                            a.eulerAngles = v3tmp1;
                            //thisTransform.rotation = Quaternion.RotateTowards(thisTransform.rotation, a, rotateSpeed);
                        }
                    }
                    else
                    {
                        isSeeingEnemy = false;
                    }
                }
            }
            else
            {
                if (enemySaw.Count == 0)
                {
                    isSeeingEnemy = false;
                    timeDelayCalculateDistanceCount = 0;
                    canCalculate = true;
                }
                else
                {
                    isSeeingEnemy = true;
                }
            }
            if (isSeeingEnemy)
            {
                GunManage.Instance.Shoot(enemyPosDic);
                v3tmp3 = tpLookAt.position - thisTransform.position;
                //angleEnemyToForward = Vector3.SignedAngle(v3tmp3, thisTransform.forward, v3Up);
                a = thisTransform.rotation;
                a = Quaternion.LookRotation(v3tmp3);
                a = Quaternion.Euler(0, a.eulerAngles.y + angleGunBarrelWithFoward, 0);
                quaternionTmp = Quaternion.Lerp(thisTransform.rotation, a, lerpRotateSpeed);
                thisTransform.rotation = quaternionTmp;
            }
            if (isClickingMouse)
            {
                if (!isShooting && !isSeeingEnemy)
                {
                    quaternionTmp = Quaternion.LookRotation(moveDir.normalized);
                    quaternionTmp = Quaternion.Euler(0, quaternionTmp.eulerAngles.y, 0);
                    thisTransform.rotation = Quaternion.Lerp(thisTransform.rotation, quaternionTmp, lerpRotateSpeed);
                }
                Move();
            }
            else
            {
                SlowToStop();
            }
        }
        if (!GameController.Instance.IsPlaying)
        {
            SlowToStop();
        }
    }
    //private void FixedUpdate()
    //{
    //    if (joystick.DirectionVector3.sqrMagnitude > minMoveJoystick)
    //    {
    //        moveDir = joystick.DirectionVector3.normalized;
    //        moveDir.y = 0;
    //        isClickingMouse = true;
    //    }
    //    else
    //    {
    //        moveDir = v3Zero;
    //        isClickingMouse = false;
    //    }
    //    if (isClickingMouse && _isAlive && GameController.Instance.IsPlaying)
    //    {
    //        //if (!isShooting && !isSeeingEnemy)
    //        //{
    //        //    quaternionTmp = Quaternion.LookRotation(moveDir.normalized);
    //        //    thisTransform.rotation = Quaternion.RotateTowards(thisTransform.rotation, quaternionTmp, rotateSpeed);
    //        //}
    //        Move();
    //    }
    //    else
    //    {
    //        SlowToStop();
    //    }
    //}
    float tmpF;
    float minDis;
    Vector3 v3Down;
    [SerializeField]
    float gravity = 9.81f;
    public void SetSpeed(float speedd)
    {
        if (speedd < 0) return;
        moveSpeed = speedd;
    }
    Vector3 refV3 = Vector3.zero;
    [SerializeField]
    float smoothTime = 0.2f;
    private void Move()
    {
        moveDir.y -= gravity * Time.deltaTime;
        characterController.Move(Time.deltaTime * moveSpeed * moveDir);
        //characterController.SimpleMove(Time.deltaTime * moveSpeed * moveDir);
        //rgbd.velocity = (Time.fixedDeltaTime * moveSpeed * moveDir);
        //rgbd.MovePosition(thisTransform.position + moveSpeed * moveDir * Time.fixedDeltaTime);
        //rgbd.MovePosition(thisTransform.position + moveSpeed * moveDir * Time.fixedDeltaTime);
        //moveDir.y = 0;
        //currentVel = Time.deltaTime * moveSpeed * moveDir;
        //thisTransform.position = Vector3.Lerp(thisTransform.position, thisTransform.position + Time.deltaTime * moveSpeed * moveDir, smoothTime);
        //Vector3.SmoothDamp(thisTransform.position, (thisTransform.position + Time.deltaTime * moveSpeed * moveDir), ref refV3, smoothTime);
    }
    [SerializeField]
    float timeRunThrowNade = 0.43f;
    [SerializeField]
    float timeIdleThrowNade = 0.39f;
    public void ThrowNade()
    {
        if (IsThrowingNade) return;
        IsThrowingNade = true;
        if (isClickingMouse)
        {
            Invoke("Throw", timeRunThrowNade);
            timeThrowNade = _timeThrowNadeRun;
        }
        else
        {
            Invoke("Throw", timeIdleThrowNade);
            timeThrowNade = _timeThrowNadeIdle;
        }
    }
    void Throw()
    {
        if (IsSeeingEnemy)
        {
            Vector3 dir = enemyPosDic[0] - baseChar._GrenadePos.position;
            Debug.DrawRay(baseChar._GrenadePos.position, dir * 100f, Color.gray, 5f);
            dir = Quaternion.Euler(70, 0, 0) * dir;
            Debug.DrawRay(baseChar._GrenadePos.position, dir * 100f, Color.red, 5f);
            SpawnerManage.Instance.SpawnGrenade(baseChar._GrenadePos.position, dir.normalized);
        }
        else
        {
            Vector3 dir = thisTransform.forward;
            Debug.DrawRay(baseChar._GrenadePos.position, dir * 100f, Color.yellow, 5f);
            dir = Quaternion.Euler(60, 0, 0) * dir;
            Debug.DrawRay(baseChar._GrenadePos.position, dir * 100f, Color.white, 5f);
            SpawnerManage.Instance.SpawnGrenade(
                baseChar._GrenadePos.position,
                baseChar._GrenadePos.forward);
        }
    }
    private void SlowToStop()
    {
        //rgbd.velocity = v3Zero;
        //currentVel = v3Zero;
        moveDir = v3Zero;
    }
    Vector3 tmpDir1, v3tmpRayCast;
    public bool IsHitTransform(Transform hitCheck, float distance)
    {
        v3tmpRayCast = hitCheck.position;
        v3tmpRayCast.y += RayCastPoint.position.y;
        tmpDir1 = v3tmpRayCast - RayCastPoint.position;
        Ray ray = new Ray(RayCastPoint.position, tmpDir1.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance + 1, RayCastLayer))
        {
            if (hit.transform.GetHashCode().Equals(hitCheck.GetHashCode()))
            {
                return true;
            }
        }
        return false;
    }
    public override void TakeDamage(int damage)
    {
        if (EffectOnPlayerManage.Instance.IsProtecting) return;
        healthComponent.Damage(damage);
        if (healthComponent.IsDead())
        {
            Death();
        }
    }

    public override void Death()
    {
        _isAlive = false;
        LevelManage.Instance.FinishLevel();
    }

    public override void OnSpawn()
    {

    }
}
