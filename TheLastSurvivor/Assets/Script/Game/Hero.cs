using UnityEngine;
using System.Collections;

public class Hero : XUnit 
{
    
    private float _currExp;
    private float _needExp;
//    private UISlider _expGo;
//    private UILabel _levelLabel;
//    private UILabel _attackLabel;
    [HideInInspector][System.NonSerialized] public bool _isMyHero = false;
    [HideInInspector][System.NonSerialized] public int CurrentMoveDirection;
    [HideInInspector][System.NonSerialized] public bool Visible;
    [HideInInspector][System.NonSerialized] public Renderer[] m_Renderers;
    [HideInInspector][System.NonSerialized] public bool isRangeHero = false;
    [HideInInspector][System.NonSerialized] public int FeibiaoNumber = 2;
    private CharacterController _cc;
    private Vector3 _moveDelta;

	public void XStart () 
    {
        m_Renderers = GetComponentsInChildren<Renderer>();
        Visible = true;
        _expWhenIsDie = 90f;
        CurrentMoveDirection = -1;
        if (transform.name == GeneralData.myID.ToString())
            _isMyHero = true;
        HP = GameObject.Find("UI Root/HP/PlayerHP/" + gameObject.name).GetComponent<UISlider>();
        _cc = GetComponent<CharacterController>();
        _currExp = 0;
        _needExp = 100;
        if (_isMyHero)
        {
//            _expGo = GameObject.Find("UI Root/boardDlg/Property/EXP").GetComponent<UISlider>();
//            _levelLabel = GameObject.Find("UI Root/boardDlg/Property/EXP/foreground/Num").GetComponent<UILabel>();
//            _attackLabel = GameObject.Find("UI Root/boardDlg/Property/Attack/Num").GetComponent<UILabel>();
//            _expGo.value = _currExp;
//            _levelLabel.text = _level.ToString();
//            _attackLabel.text = _attackValue.ToString();
        }

        transform.FindChild("SmallMapDraw").GetComponent<SmallMapDraw>().XStart();
	}

    public void ResumeFeibiaoNumber(int add)
    {
        if (add == 2)
            FeibiaoNumber = 2;
        else
            FeibiaoNumber += add;
        if (_isMyHero)
            GameObject.Find("UI Root/Skill_List/feibiao/Number/Label").GetComponent<UILabel>().text = FeibiaoNumber.ToString();
    }

    public void StartHidding(int disHideTime)
    {
        StartCoroutine(Hidding(disHideTime));
    }

    public IEnumerator Hidding(int disHideTime)
    {
        Color color = new Color(1, 1, 1, 1);
        _moveSpeed = 4f;

        for(int i = 0 ; i < 4 ; i ++)
        {
            color.a -= 0.15f; 
            for(int j = 0; j < m_Renderers.Length; j++)
                m_Renderers[j].material.SetColor("_Color", color);
            for(int j = 0 ; j < 5 ; j++)
                yield return new WaitForFixedUpdate();
        }

        Visible = false;
        if (TeamID != GeneralData.TeamId[GeneralData.myID])
        {
            color.a = 0;
            for (int j = 0; j < m_Renderers.Length; j++)
                m_Renderers[j].material.SetColor("_Color", color);
            HP.gameObject.SetActive(false);
            GameObject.Find("UI Root/Name/" + gameObject.name).SetActive(false);
        }

        disHideTime -= 20;
        while (disHideTime-- > 0)
            yield return new WaitForFixedUpdate();

        Visible = true;
        _moveSpeed = 8f;
        color.a = 1;
        for (int j = 0; j < m_Renderers.Length; j++)
            m_Renderers[j].material.SetColor("_Color", color);
        if (HP != null)
            HP.gameObject.SetActive(true);
        Transform ts = GameObject.Find("UI Root/Name").transform.Find(gameObject.name);
        if(ts != null)
            ts.gameObject.SetActive(true);

    }

    public void SetVisible()
    {
        if (Visible)
            return;
        Visible = true;
        _moveSpeed = 8f;
        Color color = new Color(1, 1, 1, 1);
        m_Renderers [0].material.color = color;
        if(HP != null)
            HP.gameObject.SetActive(true);
        Transform ts = GameObject.Find("UI Root/Name").transform.Find(gameObject.name);
        if(ts != null)
            ts.gameObject.SetActive(true);
    }

//    public void ResumeLevel(int level)
//    {
//        _level = level;
//        _needExp = 10 + level * 10;
//        _attackValue = 15 + level * 10;
//        _expWhenIsDie = 10 + level * 10;
//        NameLabel.GetComponent<NameFollow>().ResumeLevel(_level);
//    }

    public override void XFixedUpdate()
    {
        if (_isDead)
            return;
        _moveDelta = Vector3.zero;
        MoveForwardDirection();

        _moveDelta.y -= 0.2f;
        _cc.Move(_moveDelta);

//        if (Mathf.Abs(nav.remainingDistance) < 0.01f)
//            animator.SetBool("Run", false);
//        if (_isMovingToPos)
//            MovingToPos(_moveTargetPos);
//        if (_isAttackingGo)
//            AttackingGo();

//        transform.FindChild("SmallMapDraw").GetComponent<SmallMapDraw>().XFixedUpdate();
        
    }

    private void MoveForwardDirection()
    {
        if (CurrentMoveDirection == -1)
        {
            animator.SetBool("Run", false);
            return;
        }
        else
            animator.SetBool("Run", true);
        Vector3 ttt = Vector3.zero;
        ttt.x = Mathf.Cos(CurrentMoveDirection * Mathf.PI / 8) * 0.02f * _moveSpeed;
        ttt.z = Mathf.Sin(CurrentMoveDirection * Mathf.PI / 8) * 0.02f * _moveSpeed;
        _moveDelta += ttt;
        Quaternion mmm = Quaternion.Euler(0, -CurrentMoveDirection * 22.5f + 90 , 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, mmm, 15);
    }

    private void LevelUp()
    {
        _currExp -= _needExp;
        _needExp += 100;
        _level ++;
//        _attackValue += 10f;
        _expWhenIsDie = 80 * _level + Mathf.Sqrt(100 * _level);
        NameLabel.GetComponent<NameFollow>().ResumeLevel(_level);
        if (_isMyHero)
        {
            for(int i = 3; i < GeneralData.SkillNum ; i++)
                GeneralData.skillCD[i] -= 0.2f;
        }
//        if (_isMyHero)
//        {
//            _levelLabel.text = _level.ToString();
//            _attackLabel.text = _attackValue.ToString();
//        }

        //GameObject.Find("UI Root/RankList").GetComponent<RankList>().Refresh("Player" + gameObject.name, _level);
        transform.FindChild("level_up").GetComponent<LevelUpAnima>().Play();
    }

    public override void GetExp(float exp)
    {
        _currExp += exp;
        while (_currExp >= _needExp)
        {
            LevelUp();
        }

//        if(_isMyHero)
//            _expGo.value = _currExp / _needExp;
    }

//    void OnTriggerEnter(Collider collider)
//    {
//        
//        Debug.Log("aaaaaaaaaaaaaaaaaattack!!!");
//    }
}
