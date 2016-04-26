//#define LOG
using UnityEngine;
using System.Collections;

public class Monster : XUnit 
{
    private float _lastMoveTime;
    public void XStart()
    {
        _moveTargetPos = transform.position;
        _lastMoveTime = Time.time;
    }
    
    public override void XFixedUpdate()
    {
#if LOG
        Debug.Log("target = " + _moveTargetPos);
#endif
        base.XFixedUpdate();
#if LOG
        Debug.Log(transform.position + "   ....    " + _moveTargetPos);
        Debug.Log(GameObject.Find("Controller").GetComponent<Controller>().CurrentFrameNum + " " + gameObject.name + " " + ReDistance());
#endif

        if ( !animator.GetBool("Death") && ReDistance() < 0.1f)
        {
#if LOG
            Debug.Log(gameObject.name + " arrive ") ;
#endif
//            if(GeneralData.XRandom(int.Parse(gameObject.name),0,100) < 2)
//            {
//                Vector3 tmp = Vector3.zero;
//                tmp.x = GeneralData.XRandom(int.Parse(gameObject.name),0,500) / 100f;
//                tmp.z = GeneralData.XRandom(int.Parse(gameObject.name),0,500) / 100f;
//
//                Debug.Log(GameObject.Find("Controller").GetComponent<Controller>().CurrentFrameNum + " " + gameObject.name + " newTarget");
//                _moveTargetPos = transform.position + tmp;
//                Debug.Log("targetPos is " + _moveTargetPos);
//                CalculateAngle(transform.position, _moveTargetPos);
//                _isMovingToPos = true;
//            }
//            else
//                Debug.Log("haven't new target");
        }

        if (Time.time - _lastMoveTime > 5)
        {
            _lastMoveTime = Time.time;
            Vector3 tmp = Vector3.zero;
            tmp.x = GeneralData.XRandom(int.Parse(gameObject.name),0,1000) / 100f - 5f;
            tmp.z = GeneralData.XRandom(int.Parse(gameObject.name),0,1000) / 100f - 5f;

#if LOG
            Debug.Log(GameObject.Find("Controller").GetComponent<Controller>().CurrentFrameNum + " " + gameObject.name + " newTarget");
#endif
            _moveTargetPos = transform.position + tmp;
#if LOG
            Debug.Log("targetPos is " + _moveTargetPos);
#endif
            CalculateAngle(transform.position, _moveTargetPos);
            _isMovingToPos = true;
        }
#if LOG
        else
            Debug.Log("haven't new target");
#endif
    }
}
