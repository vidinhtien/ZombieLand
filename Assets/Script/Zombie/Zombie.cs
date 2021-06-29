using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Zombie : Character, IZombie
{
    Transform thisTransform;
    Rigidbody rgbd;
    ZombieLevelMap levelMap;
    AreaZombie areaZombie;
    [SerializeField]
    Animator animator;
    [SerializeField]
    Transform playerTrans;
    [SerializeField]
    Transform RaycastPoint;
    CapsuleCollider thisCollider;
    [SerializeField]
    ZombieNormal_Attack attackObject;
    Vector3 aimPos;
    [SerializeField]
    private Vector2 distanceToSeePlayer;
    [SerializeField]
    Pathfinding.RichAI aiPath;
    [SerializeField]
    float[] recalRateRand = { 0.15f, 0.3f, .45f, .6f };
    [SerializeField]
    HealthComponent healthComponent;
    [SerializeField]
    GameObject Shadow;
    [SerializeField]
    Transform[] listPositionBleeding;
    public Vector3 GetPosition()
    {
        return thisTransform.position;
    }
    public Vector3 GetRaycastPoint()
    {
        return RaycastPoint.position;
    }
    public Vector2 GetDistanceToSeePlayer()
    {
        return distanceToSeePlayer;
    }
    [SerializeField]
    float _distanceSeePlayer;
    public void OnCreate(AreaZombie area)
    {
        _isAlive = true;
        thisTransform = transform;
        areaZombie = area;
        gameObject.layer = 10;
        aiPath = GetComponent<Pathfinding.RichAI>();
        aiPath.enabled = false;
        thisCollider = GetComponent<CapsuleCollider>();
        //Shadow.SetActive(true);
        rgbd = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        _distanceSeePlayer = Random.Range(distanceToSeePlayer.x, distanceToSeePlayer.y);
    }
    public void Setup()
    {
        gameObject.layer = 10;
        canDidFall = false;
        if (animator != null)
        {
            animator.SetBool("isDeath", false);
            animator.SetBool("seePlayer", false);
            animator.SetBool("catchPlayer", false);
            animator.SetBool("isDeath2", false);
        }
        if (rgbd != null)
        {
            rgbd.mass = massDefault;
            rgbd.velocity = Vector3.zero;
        }
        if (thisCollider != null)
            thisCollider.enabled = true;
    }
    [SerializeField]
    private float distanceCloset = 1f;
    [SerializeField]
    bool isAttacking = false;
    [SerializeField]
    float pauseTime = .3f;
    [SerializeField]
    float pauseCount = 0f;
    Vector3 _playerPos;
    [SerializeField]
    float timeAttack = 3.5f;
    [SerializeField]
    float attacktTimeCount;
    [SerializeField]
    float showHealtTime = 1.5f;
    float _showHealtTime;
    [SerializeField]
    float distanceCanSearch = 16f;
    [SerializeField]
    bool isSeeingPlayer;
    bool isGoingToPos;
    [SerializeField]
    float _timeChangeIdle = 0;
    [SerializeField]
    Vector2 timeChangeIdle;
    [SerializeField]
    float _timePlaySound = 0;
    [SerializeField]
    Vector2 timeSound;
    private void Update()
    {
        isSeeingPlayer = aiPath.canMove && (aiPath.velocity.x != 0f || aiPath.velocity.z != 0f);
        animator?.SetBool("seePlayer", isSeeingPlayer);
        if (_showHealtTime > 0)
        {
            _showHealtTime -= Time.deltaTime;
        }
        else
        {
        }
        healthComponent.HideHealth();
        if (attacktTimeCount > 0)
        {
            attacktTimeCount -= Time.deltaTime;
        }
        else
        {
            if (isAttacking)
            {
                isAttacking = false;
                animator.SetBool("catchPlayer", false);
            }
        }
        if (pauseCount > 0)
        {
            pauseCount -= Time.deltaTime;
            //rgbd.velocity = 0.5f * rgbd.velocity ;
        }
        if (_timeGoToPos > 0)
        {
            _timeGoToPos -= Time.deltaTime;
        }
        else
        {
            if (isGoingToPos)
            {
                isGoingToPos = false;
                aiPath.canSearch = true;
                aiPath.enabled = false;
            }
        }
        if (!isSeeingPlayer)
        {
            if (_timeChangeIdle > 0)
            {
                _timeChangeIdle -= Time.deltaTime;
            }
            else
            {
                ChangeIdle();
                _timeChangeIdle = Random.Range(timeChangeIdle.x, timeChangeIdle.y);
            }
            if (_timePlaySound > 0)
            {
                _timePlaySound -= Time.deltaTime;
            }
            else
            {
                PlaySound();
                _timePlaySound = Random.Range(timeSound.x, timeSound.y);
            }
        }
    }
    //Vector3 vel;
    float d;
    int recalIndex;
    [SerializeField]
    float timeGotoPos = 4f;
    float _timeGoToPos;
    public void GotoPos(Vector3 pos)
    {
        aiPath.canSearch = true;
        aiPath.destination = pos;
        isGoingToPos = true;
        _timeGoToPos = timeGotoPos;
        aiPath.canMove = true;
    }
    void ChangeIdle()
    {
        int rand = Random.Range(1, 4);
        if (rand < 1 || rand > 3) return;
        animator.SetTrigger("idle" + rand);
    }
    void PlaySound()
    {
        SoundManage.Instance.Play_ZombieScream();
    }
    public void OnUpdate(Vector3 playerPos)
    {
        if (!_isAlive)
        {
            areaZombie.OnZombieKilled(this);
            return;
        }
        _playerPos = PlayerController.Instance.GetPlayerPosition();
        _playerPos.y = thisTransform.position.y;
        d = Vector3.SqrMagnitude(thisTransform.position - playerPos);
        if (d > _distanceSeePlayer)
        {
            return;
        }
        if (!aiPath.enabled)
        {
            aiPath.enabled = true;
        }
        if (d <= distanceCloset)
        {
            if (!isAttacking)
            {
                animator.SetBool("catchPlayer", true);
                thisTransform.LookAt(_playerPos);
                attacktTimeCount = timeAttack;
                isAttacking = true;
            }
        }
        else
        {
            if (isAttacking)
            {
                animator.SetBool("catchPlayer", false);
                isAttacking = false;
            }
        }

        if (d >= distanceCanSearch)
        {
            if (!isGoingToPos && (aiPath.canSearch || aiPath.reachedDestination || !aiPath.hasPath))
            {
                GotoPos(_playerPos);
                recalIndex = GetIndexRecal(d);
                aiPath.repathRate = recalRateRand[recalIndex];
            }
        }
        else
        {
            isGoingToPos = false;
            aiPath.canSearch = true;
            recalIndex = GetIndexRecal(d);
            aiPath.repathRate = recalRateRand[recalIndex];
            aiPath.destination = _playerPos;
        }
        if (!isAttacking)
        {
            aiPath.destination = playerPos;
        }
        else
        {
            if (aiPath.canMove)
            {
                aiPath.canMove = false;
            }
        }
    }
    private int GetIndexRecal(float d)
    {
        if (d > 49)
        {
            return 3;
        }
        else if (d > 36)
        {
            return 2;
        }
        else if (d > 16)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    public void OutOfSight()
    {
        if (aiPath.enabled && !aiPath.hasPath)
        {
            aiPath.enabled = false;
            aiPath.canSearch = false;
        }
        else
        {

        }
    }
    private void OnPathDone(Path path)
    {
        //if (aiPath.enabled)
        //{
        //    aiPath.enabled = false;
        //}

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("catchPlayer", true);
            thisTransform.LookAt(_playerPos);

            isAttacking = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("catchPlayer", false);
            isAttacking = false;
        }
    }
    public override void TakeDamage(int damage)
    {
        if (!gameObject.activeSelf) return;
        healthComponent.Damage(damage);
        pauseCount = pauseTime;
        _showHealtTime = showHealtTime;
        if (healthComponent.IsDead())
        {
            Death();
        }
    }
    public virtual void DoAttack()
    {

    }
    [SerializeField]
    float timeToBleeding = 2.5f;
    [SerializeField]
    float timeDeativeAfterDead1 = 4f;
    bool canDidFall = false;
    public override void Death()
    {
        if (!_isAlive) return;
        aiPath.enabled = false;
        _isAlive = false;
        Shadow.SetActive(false);
        animator.SetBool("isDeath", true);
        areaZombie.OnZombieKilled(this);
        //rgbd.velocity = new Vector3(0, 0, 0);
        aiPath.canMove = false;
        gameObject.layer = 14;
        //animator.SetBool("isDeath", false);
        SoundManage.Instance.Play_ZombieDead();
        LevelManage.Instance.GetCurrentLevelMap().GetComponent<LevelMap>().OnKillZombie(thisTransform.position);
        Player.Instance.RemoveEnenmy(this);
        canDidFall = true;
        Invoke("Bleeding", timeToBleeding);
        Invoke("Falling", timeDeativeAfterDead1);
        //StartCoroutine(DeactiveIn(Random.Range(timeDeativeAfterDead, timeDeativeAfterDead1)));
        //gameObject.SetActive(false);
    }
    [SerializeField]
    float massDead = 0.001f;
    [SerializeField]
    float massDefault = 1;
    private void Bleeding()
    {
        if (!canDidFall) return;
        if (listPositionBleeding.Length == 0)
        {
            EffectManage.Instance.TurnOnBlood(thisTransform.position);
        }
        else
        {
            int index = Random.Range(0, listPositionBleeding.Length);
            EffectManage.Instance.TurnOnBlood(listPositionBleeding[index].position);
        }
    }
    private void Falling()
    {
        if (!canDidFall) return;
        rgbd.mass = massDead;
        thisCollider.enabled = false;
        Invoke("Deactive", .3f);
    }
    private void Deactive()
    {
        gameObject.SetActive(false);
    }
    public override void OnSpawn()
    {

    }

    public void SetupInformation(ZombieInformation infor)
    {
        gameObject.layer = 10;
        _isAlive = true;
        if (aiPath != null)
        {
            aiPath.canSearch = true;
            aiPath.canMove = true;

        }
        if (rgbd != null)
        {
            rgbd.velocity = Vector3.zero;
            rgbd.angularVelocity = Vector3.zero;
            rgbd.mass = massDefault;
        }
        if (thisCollider != null)
            thisCollider.enabled = true;
        if (animator != null)
        {
            animator.SetBool("isDeath", false);
        }
        healthComponent.MaxHealthPoint = infor.HP;
        healthComponent.Setup();
        attackObject.damage = infor.Damage;
        aiPath.maxSpeed = infor.Speed;
    }
}
