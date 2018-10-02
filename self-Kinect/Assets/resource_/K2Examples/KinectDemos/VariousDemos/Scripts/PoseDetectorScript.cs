using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class PoseDetectorScript : MonoBehaviour 
{
	[Tooltip("User avatar model, who needs to reach the target pose.")]
	public PoseModelHelper avatarModel;

	//[Tooltip("Model in pose that need to be reached by the user.")]
	//public PoseModelHelper poseModel;

    [Tooltip("Model in pose that need to be reached by the user.")]
    public List<PoseModelHelper> poseModel = new List<PoseModelHelper>();

    [Tooltip("List of joints to compare.")]
	public List<KinectInterop.JointType> poseJoints = new List<KinectInterop.JointType>();

	[Tooltip("Threshold, above which we consider the pose is matched.")]
	public float matchThreshold = 0.7f;

	[Tooltip("GUI-Text to display information messages.")]
	public UnityEngine.UI.Text infoText;

	// match percent (between 0 and 1)
	private float fMatchPercent = 0f;
	// whether the pose is matched or not
	private bool bPoseMatched = false;
    
    public static int avatar=0;
    
    
    /*
    class Pose
    {
        public static int pose_gameStart = 0;
        public static int pose_gmaeEnd = 1;
        public static int pose_rock = 2;
        public static int pose_paper = 3;
        public static int pose_scissors = 4;
        
    }
    */
    
    /// <summary>
    /// Gets the pose match percent.
    /// </summary>
    /// <returns>The match percent (value between 0 and 1).</returns>
    public float GetMatchPercent()
	{
		return fMatchPercent;
	}


	/// <summary>
	/// Determines whether the target pose is matched or not.
	/// </summary>
	/// <returns><c>true</c> if the target pose is matched; otherwise, <c>false</c>.</returns>
	public bool IsPoseMatched()
	{
		return bPoseMatched;
	}


	void Update () 
	{
		KinectManager kinectManager = KinectManager.Instance;
		AvatarController avatarCtrl = avatarModel ? avatarModel.gameObject.GetComponent<AvatarController>() : null;

		if(kinectManager != null && kinectManager.IsInitialized() && 
		   avatarModel != null && avatarCtrl && kinectManager.IsUserTracked(avatarCtrl.playerId))
		{
			// get mirrored state
			bool isMirrored = avatarCtrl.mirroredMovement; // 거울상태인지? 일단은 1임
			
			// get the match pose
			string sDiffDetails = string.Empty;
            avatar = GetMatchpose(isMirrored, true, ref sDiffDetails);
            
            string sPoseMessage = string.Format("Pose match: {0:F0}% {1}", fMatchPercent * 100f, 
			                                    (bPoseMatched ? "- Matched" : ""));
			if(infoText != null)
			{
                infoText.text = "";
            }
		}
		else
		{
			// no user found
			if(infoText != null)
			{
				infoText.text = "Stand in front of the monitor.";
			}
		}

	}


    // gets angle or percent difference in pose
    public int GetMatchpose(bool isMirrored, bool bPercentDiff, ref string sDiffDetails)
    {
        float fAngleDiff = 0f;
        float fMaxDiff = 0f;
        sDiffDetails = string.Empty;
        KinectManager kinectManager = KinectManager.Instance;

        int AvatarNumb=-1;

        
        float fPercentDiff = 0f;
        float pre_fPercentDiff = 1f;

        List<float> fPercentDiff_L = new List<float>();


        for (int Pi = 0; Pi < poseModel.Count; Pi++)
        {
            fAngleDiff = 0f;
            fMaxDiff = 0f;

            if (!kinectManager || !avatarModel || !poseModel[Pi] || poseJoints.Count == 0)
            {
                return -1;
            }

            // copy model rotation , 모델의 회전
            Quaternion poseSavedRotation = poseModel[Pi].GetBoneTransform(0).rotation;
            poseModel[Pi].GetBoneTransform(0).rotation = avatarModel.GetBoneTransform(0).rotation;

            StringBuilder sbDetails = new StringBuilder();
            sbDetails.Append("Joint differences:").AppendLine();

            for (int i = 0; i < poseJoints.Count; i++)
            {
                KinectInterop.JointType joint = poseJoints[i]; // 0번 척추부분, 4번부터 왼쪽 어깨, 8번부터 오른쪽 어깨
                KinectInterop.JointType nextJoint = kinectManager.GetNextJoint(joint);

                if (nextJoint != joint && (int)nextJoint >= 0 && (int)nextJoint < KinectInterop.Constants.MaxJointCount) // 다음조인트와 같지않고, 척추가 아니며, 마지막 조인트가 아닐때
                {
                    Transform avatarTransform1 = avatarModel.GetBoneTransform(avatarModel.GetBoneIndexByJoint(joint, isMirrored)); // 조인트
                    Transform avatarTransform2 = avatarModel.GetBoneTransform(avatarModel.GetBoneIndexByJoint(nextJoint, isMirrored)); // 다음조인트

                    Transform poseTransform1 = poseModel[Pi].GetBoneTransform(poseModel[Pi].GetBoneIndexByJoint(joint, isMirrored));
                    Transform poseTransform2 = poseModel[Pi].GetBoneTransform(poseModel[Pi].GetBoneIndexByJoint(nextJoint, isMirrored));

                    if (avatarTransform1 != null && avatarTransform2 != null && poseTransform1 != null && poseTransform2 != null)
                    {
                        // 이게 두 포즈의 본의 각도를 비교해서 차이를 검사하는듯 위치가 아니라, 그렇네 위치가 더 별로겠네
                        Vector3 vAvatarBone = (avatarTransform2.position - avatarTransform1.position).normalized;
                        Vector3 vPoseBone = (poseTransform2.position - poseTransform1.position).normalized;

                        float fDiff = Vector3.Angle(vPoseBone, vAvatarBone);
                        if (fDiff > 90f) fDiff = 90f;

                        fAngleDiff += fDiff; // 각도 차이의 총합을 구하는거같음.
                        fMaxDiff += 90f;  // we assume the max diff could be 90 degrees

                        sbDetails.AppendFormat("{0} - {1:F0} deg.", joint, fDiff).AppendLine();
                    }
                    else
                    {
                        sbDetails.AppendFormat("{0} - n/a", joint).AppendLine();
                    }
                }
            }

            poseModel[Pi].GetBoneTransform(0).rotation = poseSavedRotation;
            
            // calculate percent diff
            if (bPercentDiff && fMaxDiff > 0f)
            {
                fPercentDiff_L.Add(fPercentDiff);               
                fPercentDiff = fAngleDiff / fMaxDiff;
                if(pre_fPercentDiff > fPercentDiff)
                {
                    pre_fPercentDiff = fPercentDiff;
                    if ((1f-pre_fPercentDiff)> matchThreshold)
                    {
                        AvatarNumb = Pi+1; // 0번부터 검사하기 때문에 +1을 해야 맞음
                    }
                    else
                    {
                        AvatarNumb = 0; // 0번은 아무것도 기본 포즈
                    }
                }
            }

            
            // details info
            sbDetails.AppendLine();
            sbDetails.AppendFormat("Sum-Diff: - {0:F0} deg out of {1:F0} deg", fAngleDiff, fMaxDiff).AppendLine();
            sbDetails.AppendFormat("Percent-Diff: {0:F0}%", fPercentDiff * 100).AppendLine();
            sDiffDetails = sbDetails.ToString();
            
        }
        fPercentDiff_L.Clear();
        
        return AvatarNumb;


    }
}
