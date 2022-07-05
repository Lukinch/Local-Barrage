//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(UnityEnemyDetector))]
//public class UnityEnemyDetectorEditor : Editor
//{
//    private void OnSceneGUI()
//    {
//        UnityEnemyDetector detector = (UnityEnemyDetector)target;
//        Handles.color = Color.white;
//        Handles.DrawWireArc(detector.rotatorObject.position, Vector3.up, Vector3.forward, 360, detector.detectionRadius);
//        Vector3 detectionAngleA = DirFromAngle(detector.rotatorObject, -detector.detectionAngle / 2, false);
//        Vector3 detectionAngleB = DirFromAngle(detector.rotatorObject, detector.detectionAngle / 2, false);

//        Handles.DrawLine(detector.rotatorObject.position, detector.rotatorObject.position + detectionAngleA * detector.detectionRadius);
//        Handles.DrawLine(detector.rotatorObject.position, detector.rotatorObject.position + detectionAngleB * detector.detectionRadius);

//        Handles.color = Color.red;
//        foreach (Transform visiblePlayer in detector.visiblePlayers)
//        {
//            Handles.DrawLine(detector.rotatorObject.transform.position, visiblePlayer.position);
//        }
//    }

//    public Vector3 DirFromAngle(Transform rotatorObject,float angleInDegrees, bool angleIsGlobal)
//    {
//        if (!angleIsGlobal) angleInDegrees += rotatorObject.eulerAngles.y;
//        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
//    }
//}
