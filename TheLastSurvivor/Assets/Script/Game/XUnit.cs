using UnityEngine;
using System.Collections;
using client;
using proto_islandsurvival;
public class XUnit : MonoBehaviour 
{
    
    [HideInInspector][System.NonSerialized] public GameObject _attackTargetGo;
    [HideInInspector][System.NonSerialized] public GameObject m_Wudi;
    [HideInInspector][System.NonSerialized] public UISlider HP;
    [HideInInspector][System.NonSerialized] public GameObject NameLabel;

    public Animator animator;
    protected float _turnAngle;
    [HideInInspector][System.NonSerialized] public bool _isMovingToPos = false;
    [HideInInspector][System.NonSerialized] public Vector3 _moveTargetPos;
    [HideInInspector][System.NonSerialized] public bool _isAttackingGo = false;
    [HideInInspector][System.NonSerialized] public GameObject _attacker;

    //public float _lastAttackTime;
    [HideInInspector][System.NonSerialized] public float _expWhenIsDie = 10f;
    [HideInInspector][System.NonSerialized] public int _attackValue = 1;
    [HideInInspector][System.NonSerialized] public float _attackSpeed = 2f;
    [HideInInspector][System.NonSerialized] public float _moveSpeed = 8f;
    [HideInInspector][System.NonSerialized] public float _attackRange = 5f;
    [HideInInspector][System.NonSerialized] public int _level = 1;
    [HideInInspector][System.NonSerialized] public bool _isDead = false;
    [HideInInspector][System.NonSerialized] public int _currHP = 1;
    [HideInInspector][System.NonSerialized] public int _maxHP = 1;
    [HideInInspector][System.NonSerialized] public bool CanRevive = true;
    [HideInInspector][System.NonSerialized] public int TeamID;
    
    public virtual void XFixedUpdate()
    {
        if (_isDead)
            return;
        if (ReDistance() < 0.01f)
            animator.SetBool("Run", false);
        if (_isMovingToPos)
            MovingToPos(_moveTargetPos);
        if (_isAttackingGo)
            AttackingGo();

    }

    public void BeAttacked(GameObject attacker, int valueOfHurt)
    {
        if (attacker.GetComponent<Hero>().TeamID == TeamID)
            return;
        if (m_Wudi != null)
            return ;
        UISlider hp = attacker.GetComponent<XUnit>().HP;
        if (hp == null || _currHP <= 0)
            return;
        if (attacker.name != GeneralData.myID.ToString())
            return;

        CMessage mess = new CMessage();
        mess.m_head.m_framenum = Controller.CurrentFrameNum;
        mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSKill));
        CSKill proto = new CSKill();
        proto.killer = int.Parse(attacker.name);
        proto.bekilled = int.Parse(gameObject.name);
        mess.m_proto = proto;
        program.SendQueue.push(mess);
    }

    public void BeKillFor(GameObject attacker, int valueOfHurt)
    {
        _currHP -= valueOfHurt;
        HP.value = _currHP * 1f / _maxHP;
        //HP.gameObject.GetComponent<HPFollow>().AppearHurtNum(valueOfHurt);
        if (_currHP <= 0)
        {
            Debug.Log("error ? : " + GeneralData.PlayerName[int.Parse(attacker.name)] + " kill " + GeneralData.PlayerName[int.Parse(gameObject.name)]);
            Debug.Log(attacker.transform.position.ToString("f8") + "  =weizhi=  " + gameObject.transform.position.ToString("f8"));
            GameObject.Find("UI Root/RankList").GetComponent<RankList>().Kill(int.Parse(attacker.name), int.Parse(gameObject.name));
            
            GameJudgement.DealWith();
            attacker.GetComponent<XUnit>().GetExp(_expWhenIsDie);
            Dying();
        }
    }

    private IEnumerator Disappear(int spawnIndex)
    {
        if (gameObject.tag == "Player")
        {
            if(gameObject.GetComponent<Hero>()._isMyHero)
                StartCoroutine(GameObject.Find("Controller").GetComponent<PlayerInput>().CanNotControll(200));
            for(int i = 0 ; i < 200 ; i++)
                yield return new WaitForFixedUpdate();
            if(!CanRevive)
            {
                if(GetComponent<Hero>()._isMyHero)
                {
                    GeneralData.choosed = false;
                    GameObject.Find("Controller").GetComponent<PlayerInput>().CanControll = false;
                }
                Destroy(HP.gameObject);
                Destroy(NameLabel);
                Destroy(gameObject);
            }
            else
                GameObject.Find("Player").GetComponent<PlayerSpawn>().Resume(int.Parse(gameObject.name), spawnIndex);
        }
    }

    public void Dying()
    {
        _isDead = true;
        gameObject.GetComponent<Hero>().SetVisible();
        HP.gameObject.SetActive(false);
        animator.SetBool("Death", true);
        StartCoroutine(Disappear(GeneralData.XRandom(0,0,9)));
        _isDead = true;
    }

    public virtual void GetExp(float exp)
    {

    }

    public void AttackingGo()
    {
        if (_attackTargetGo.GetComponent<XUnit>().HP == null)
        {
//            GexExp(_attackTargetGo.GetComponent<XUnit>()._expWhenIsDie);
            _isAttackingGo = false;
            return ;
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (Vector3.Distance(transform.position, _attackTargetGo.transform.position) > _attackRange)
            {
                MovingToPos(_attackTargetGo.transform.position);
            }
            else
            {
                animator.SetBool("Run", false);
                CalculateAngle(transform.position, _attackTargetGo.transform.position);
                Vector3 tt = new Vector3(0f, _turnAngle, 0f);
                if (Vector3.Distance(transform.eulerAngles, tt) > 1)
                    transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(transform.eulerAngles), Quaternion.Euler(tt), 5);
                else
                    animator.SetTrigger("Attack");
            }
        }
    }

    public void MovingToPos(Vector3 targetPos)
    {
        animator.SetBool("Run", true);
        Debug.Log(transform.position);
        Debug.Log(targetPos);
        Debug.Log(ReDistance());
        _isMovingToPos = false;
//        Debug.Log(nav.remainingDistance);
//        if (nav.remainingDistance == 0 || nav.remainingDistance > 1000f)
//        {
//            nav.SetDestination(transform.position);
//            _isMovingToPos = false;
//        }

    }

    public float ReDistance()
    {
        return Vector3.Distance(transform.position, _moveTargetPos);
    }

    public void CalculateAngle(Vector3 startPos, Vector3 endPos)
    {
        float detaX = endPos.x - startPos.x;
        float detaY = endPos.z - startPos.z;
        if (Mathf.Abs(detaY) > 0.1f)
        {
            //计算箭头角度
            _turnAngle = Mathf.Atan(detaY / detaX);
            if (detaX < 0f)
            {
                _turnAngle += Mathf.PI;
            }
            _turnAngle = -_turnAngle * Mathf.Rad2Deg + 90f;
            while(_turnAngle > 360f)
                _turnAngle -= 360f;
            while(_turnAngle < 0)
                _turnAngle += 360f;
        }
    }

}
