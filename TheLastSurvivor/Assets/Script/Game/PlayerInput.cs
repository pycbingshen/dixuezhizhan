using UnityEngine;
using System.Collections;
using client;
using proto_islandsurvival;

public class PlayerInput : MonoBehaviour 
{

    [HideInInspector][System.NonSerialized] public bool CanControll;
    private GameObject _thumb;
    private TweenAlpha _tween;

    private bool OnkeyW;
    private bool OnkeyS;
    private bool OnkeyA;
    private bool OnkeyD;
//    private int LastMoveInput;

    public void XStart()
    {
        CanControll = false;
        _thumb = GameObject.Find("UI Root/Joystick/Thumb");
        _tween = GameObject.Find("UI Root/Joystick").GetComponent<TweenAlpha>();
//        LastMoveInput = -1;
    }
	
    public void GetPlayerMoveOrAttack()
    {
        Vector3 cursorScreenPosition = Input.mousePosition;//鼠标在屏幕上的位置
        Ray ray = Camera.main.ScreenPointToRay(cursorScreenPosition);//在鼠标所在的屏幕位置发出一条射线
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            if(hit.collider.tag == "Terrain")
            {
                CMessage mess = new CMessage();
                mess.m_head.m_framenum = Controller.CurrentFrameNum;
                mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSMove));
                CSMove proto = new CSMove();
//                proto.x = hit.point.x;
//                proto.z = hit.point.z;
                mess.m_proto = proto;
                program.SendQueue.push(mess);
//                _con.head[_con.top] = (int)MessageType.e_move2Pos;
//                _con.tt[_con.top].name = GeneralData.myName;
//                _con.tt[_con.top].x = hit.point.x;
//                _con.tt[_con.top].y = hit.point.y;
//                _con.tt[_con.top].z = hit.point.z;
//                _con.tt[_con.top].frameNum = _con.CurrentFrameNum + 5;
//                _con.top ++;
            }
            /*
            if(hit.collider.tag == "Monster" || hit.collider.tag == "Player" && hit.collider.gameObject.name != GeneralData.myID.ToString() )
            {
                if(hit.collider.gameObject.GetComponent<XUnit>().HP.value > 0)
                {
                    CMessage mess = new CMessage();
                    mess.m_head.m_framenum = _con.CurrentFrameNum;
                    mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSAttack));
                    CSAttack proto = new CSAttack();
                    proto.target_id = int.Parse(hit.collider.gameObject.name);
                    mess.m_proto = proto;
                    program.SendQueue.push(mess);
//                    _con.head[_con.top] = (int)MessageType.e_attackGo;
//                    _con.kk[_con.top].name1 = GeneralData.myName;
//                    _con.kk[_con.top].name2 = hit.collider.gameObject.name;
//                    _con.kk[_con.top].frameNum = _con.CurrentFrameNum + 5;
//                    _con.top ++;
                }
            }*/

//            if(_con.top >= 50)
//                _con.top = 0;
        }
    }

    public IEnumerator CanNotControll(int time)
    {
        CMessage mess = new CMessage();
        mess.m_head.m_framenum = Controller.CurrentFrameNum;
        mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSMove));
        CSMove proto = new CSMove();
        proto.dir = -1;
        mess.m_proto = proto;
        program.SendQueue.push(mess);

        CanControll = false;
        GameObject skillList = GameObject.Find("UI Root/Skill_List");
        for (int i = 0; i < skillList.transform.childCount; i ++)
            skillList.transform.GetChild(i).Find("Icon").GetComponent<UIButton>().isEnabled = false;

        while (time-- > 0)
            yield return new WaitForFixedUpdate();

        CanControll = true;
        for (int i = 0; i < skillList.transform.childCount; i ++)
            skillList.transform.GetChild(i).Find("Icon").GetComponent<UIButton>().isEnabled = true;
    }

    public void GetDirectionController()
    {
        if (!CanControll)
            return;
        Hero m_MyHero = GameObject.Find("Player/" + GeneralData.myID.ToString()).GetComponent<Hero>();
        OnkeyW = Input.GetKey(KeyCode.W);
        OnkeyS = Input.GetKey(KeyCode.S);
        OnkeyA = Input.GetKey(KeyCode.A);
        OnkeyD = Input.GetKey(KeyCode.D);
        Vector3 pos = Vector3.zero;
        if (Input.GetMouseButton(0))
        {
            if(!_thumb.GetComponent<Joystick>().IsActive)
                return ;
            pos = _thumb.transform.localPosition;
        }
        else
        {

            if ((OnkeyW ^ OnkeyS) || (OnkeyA ^ OnkeyD))
            {
                if (OnkeyW ^ OnkeyS)
                {
                    if (OnkeyW)
                        pos.y = 35;
                    else
                        pos.y = -35;
                }

                if (OnkeyA ^ OnkeyD)
                {
                    if (OnkeyA)
                        pos.x = -35;
                    else
                        pos.x = 35;
                }

                pos = Vector3.ClampMagnitude(pos, 35);
                _thumb.transform.localPosition = pos;
            }
            else
            {
                _thumb.transform.localPosition = Vector3.zero;
                if(m_MyHero.CurrentMoveDirection != -1)
                {
                    _tween.PlayReverse();
                    //Debug.Log("-1");
//                    LastMoveInput = -1;
                    CMessage mess = new CMessage();
                    mess.m_head.m_framenum = Controller.CurrentFrameNum;
                    mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSMove));
                    CSMove proto = new CSMove();
                    proto.dir = -1;
                    mess.m_proto = proto;
                    program.SendQueue.push(mess);
                }
                return ;
            }
        }

        float angel = CalculateAngle(pos);
        int moveNum = (int)(angel / 22.5f);
        if (Mathf.Abs(angel - 22.5f * moveNum) > Mathf.Abs(angel - 22.5f * (moveNum + 1)))
            moveNum = (moveNum + 1) % 16;
        if (Vector3.SqrMagnitude(pos) < 0.1f)
            moveNum = -1;

        if (moveNum != m_MyHero.CurrentMoveDirection)
        {
            if(m_MyHero.CurrentMoveDirection == -1)
                _tween.PlayForward();
            //Debug.Log(moveNum);
//            LastMoveInput = moveNum;
            CMessage mess = new CMessage();
            mess.m_head.m_framenum = Controller.CurrentFrameNum;
            mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSMove));
            CSMove proto = new CSMove();
            proto.dir = moveNum;
            mess.m_proto = proto;
            program.SendQueue.push(mess);
        }

    }

	public void XUpdate() 
    {
//        if(Input.GetMouseButtonDown(1))
//        {
//            GetPlayerMoveOrAttack();
//        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            CMessage mess = new CMessage();
            mess.m_head.m_framenum = Controller.CurrentFrameNum;
            mess.m_head.m_message_id = MessageRegister.Instance().GetID(typeof(CSGamePause));
            CSGamePause proto = new CSGamePause();
            mess.m_proto = proto;
            program.SendQueue.push(mess);
        }

        GetDirectionController();
	}

    public float CalculateAngle(Vector3 vec)
    {
        float res;
        if (Mathf.Abs(vec.x) > 0.1f)
        {
            //计算角度
            res = Mathf.Atan(vec.y / vec.x);
            if (vec.x < 0f)
                res += Mathf.PI;
            res = res * Mathf.Rad2Deg;
            while (res > 360f)
                res -= 360f;
            while (res < 0)
                res += 360f;
            return res;
        }
        else
        {
            if(vec.y > 0)
                return 90f;
            else
                return 270f;
        }
    }
}
