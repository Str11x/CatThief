//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerMovementAgent : MonoBehaviour
//{
//    [SerializeField] private float _speed;
//    [SerializeField] private PathHandler pathHandler;

//    private WaitForFixedUpdate _updateTime = new WaitForFixedUpdate();
//    private Coroutine _movement;
//    private Node _targetNode;
//    private const float _tolerance = 0.4f;

//    //public void MoveToTarget(Node firstNode)
//    //{
//    //    _targetNode = firstNode;

//    //    if (_targetNode == null)
//    //        return;

//    //    if (_movement != null)
//    //    {
//    //        StopCoroutine(_movement);
//    //        _movement = StartCoroutine(Move());
//    //    }
//    //    else
//    //    {
//    //        _movement = StartCoroutine(Move());
//    //    }
//    //}

//    //private IEnumerator Move()
//    //{
//    //    //while (_targetNode != null)
//    //    //{
//        //    transform.Translate(nextNode);

//        //    if (_gridHolder.GetTargetNode() == _targetNode)
//        //        _targetNode.NextNode = null;

//        //    yield return _updateTime;
//        //}
//    }
//}