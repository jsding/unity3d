#region Using
using System;
using UnityEngine;

#endregion

/// <summary>
/// SDK�ӿڻص���
/// </summary>
public class RiseSdkListener : MonoBehaviour {
#if UNITY_ANDROID
    /// <summary>
    /// ��ʾ��Ƶ���Ľ���ص��¼�
    /// </summary>
    //public static event Action<RiseSdk.AdEventType, int, string> OnRewardAdEvent;

    /// <summary>
    /// ֧���Ľ���ص��¼�
    /// </summary>
    public static event Action<RiseSdk.PaymentResult, int> OnPaymentEvent;

    /// <summary>
    /// facebook��ز����Ľ���ص��¼�
    /// </summary>
    public static event Action<RiseSdk.SnsEventType, int> OnSNSEvent;

    /// <summary>
    /// �����ļ��Ľ���ص��¼�
    /// </summary>
    public static event Action<bool, int, string> OnCacheUrlResult;

    /// <submit or load, success, leader board id, extra data>
    public static event Action<bool, bool, string, string> OnLeaderBoardEvent;

    public static event Action<int, bool, string> OnReceiveServerResult;

    public static event Action<string> OnReceivePaymentsPrice;

    /// <summary>
    /// ��ȡ��̨�Զ���json���ݵĽ���ص��¼�
    /// </summary>
    public static event Action<string> OnReceiveServerExtra;

    /// <summary>
    /// ��ȡ��̨֪ͨ����Ϣ�Ľ���ص��¼�
    /// </summary>
    public static event Action<string> OnReceiveNotificationData;

    public static event Action<RiseSdk.AdEventType, int, string, int> OnAdEvent;

    private static RiseSdkListener _instance;

    /// <summary>
    /// ��������
    /// </summary>
    public static RiseSdkListener Instance {
        get {
            if (!_instance) {
                // check if there is a IceTimer instance already available in the scene graph
                _instance = FindObjectOfType (typeof (RiseSdkListener)) as RiseSdkListener;

                // nope, create a new one
                if (!_instance) {
                    var obj = new GameObject ("RiseSdkListener");
                    _instance = obj.AddComponent<RiseSdkListener> ();
                    DontDestroyOnLoad (obj);
                }
            }

            return _instance;
        }
    }

    void OnApplicationPause (bool pauseStatus) {
        if (pauseStatus) {
            RiseSdk.Instance.OnPause ();
        }
    }

    void OnApplicationFocus (bool focusStatus) {
        if (focusStatus) {
            RiseSdk.Instance.OnResume ();
        }
    }

    void OnApplicationQuit () {
        RiseSdk.Instance.OnStop ();
        RiseSdk.Instance.OnDestroy ();
    }

    void Awake () {
        RiseSdk.Instance.OnStart ();
    }

    /// <summary>
    /// ֧���ɹ�����ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="billId">�Ʒѵ�Id</param>
    public void onPaymentSuccess (string billId) {
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            int id = int.Parse (billId);
            OnPaymentEvent (RiseSdk.PaymentResult.Success, id);
        }
    }

    /// <summary>
    /// ֧��ʧ�ܽ���ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="billId">�Ʒѵ�Id</param>
    public void onPaymentFail (string billId) {
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            int id = int.Parse (billId);
            OnPaymentEvent (RiseSdk.PaymentResult.Failed, id);
        }
    }

    /// <summary>
    /// ֧��ȡ������ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="billId">�Ʒѵ�Id</param>
    public void onPaymentCanceled (string billId) {
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            int id = int.Parse (billId);
            OnPaymentEvent (RiseSdk.PaymentResult.Cancel, id);
        }
    }

    public void onPaymentSystemError (string data) {
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PaymentResult.PaymentSystemError, -1);
        }
    }

    public void onReceiveBillPrices (string data) {
        if (OnReceivePaymentsPrice != null && OnReceivePaymentsPrice.GetInvocationList ().Length > 0) {
            OnReceivePaymentsPrice (data);
        }
    }

    /// <summary>
    /// ����֧��ϵͳ״̬��SDK�Զ����á�
    /// </summary>
    /// <param name="dummy"></param>
    public void onPaymentSystemValid (string dummy) {
        RiseSdk.Instance.SetPaymentSystemValid (true);
    }

    /// <summary>
    /// ��½faceboook�˻��Ľ���ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="result">���صĽ������</param>
    public void onReceiveLoginResult (string result) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            int success = int.Parse (result);
            OnSNSEvent (success == 0 ? RiseSdk.SnsEventType.LoginSuccess : RiseSdk.SnsEventType.LoginFailed, 0);
        }
    }

    /// <summary>
    /// ����faceboook���ѵĽ���ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="result">���صĽ������</param>
    public void onReceiveInviteResult (string result) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            int success = int.Parse (result);
            OnSNSEvent (success == 0 ? RiseSdk.SnsEventType.InviteSuccess : RiseSdk.SnsEventType.InviteFailed, 0);
        }
    }

    /// <summary>
    /// faceboook���޵Ľ���ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="result">���صĽ������</param>
    public void onReceiveLikeResult (string result) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            int success = int.Parse (result);
            OnSNSEvent (success == 0 ? RiseSdk.SnsEventType.LikeSuccess : RiseSdk.SnsEventType.LikeFailed, 0);
        }
    }

    /// <summary>
    /// faceboook������ս�Ľ���ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="result">���صĽ������</param>
    public void onReceiveChallengeResult (string result) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            int count = int.Parse (result);
            OnSNSEvent (count > 0 ? RiseSdk.SnsEventType.ChallengeSuccess : RiseSdk.SnsEventType.ChallengeFailed, count);
        }
    }

    public void onSubmitSuccess (string leaderBoardTag) {
        if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList ().Length > 0) {
            OnLeaderBoardEvent (true, true, leaderBoardTag, "");
        }
    }

    public void onSubmitFailure (string leaderBoardTag) {
        if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList ().Length > 0) {
            OnLeaderBoardEvent (true, false, leaderBoardTag, "");
        }
    }

    public void onLoadSuccess (string data) {
        if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList ().Length > 0) {
            string[] results = data.Split ('|');
            OnLeaderBoardEvent (false, true, results[0], results[1]);
        }
    }

    public void onLoadFailure (string leaderBoardTag) {
        if (OnLeaderBoardEvent != null && OnLeaderBoardEvent.GetInvocationList ().Length > 0) {
            OnLeaderBoardEvent (false, false, leaderBoardTag, "");
        }
    }

    public void onServerResult (string data) {
        if (OnReceiveServerResult != null && OnReceiveServerResult.GetInvocationList ().Length > 0) {
            string[] results = data.Split ('|');
            int resultCode = int.Parse (results[0]);
            bool success = int.Parse (results[1]) == 0;
            OnReceiveServerResult (resultCode, success, results[2]);
        }
    }

    /// <summary>
    /// �����ļ�����ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="data">���ص�����</param>
    public void onCacheUrlResult (string data) {
        if (OnCacheUrlResult != null && OnCacheUrlResult.GetInvocationList ().Length > 0) {
            //tag,success,name
            string[] results = data.Split ('|');
            int tag = int.Parse (results[0]);
            bool success = int.Parse (results[1]) == 0;
            if (success) {
                OnCacheUrlResult (true, tag, results[2]);
            } else {
                OnCacheUrlResult (false, tag, "");
            }
        }
    }

    /// <summary>
    /// ��ȡ��̨���õ��Զ���json���ݵĻص�����SDK��ʼ����ɣ���һ�ȡ���?ݺ���Զ?��ø÷����?�?��?�����ǰ��Ӽ��?��    /// </summary>
    /// <param name="data">���غ�̨���õ��Զ���json���ݣ��磺{"x":"x", "x":8, "x":{x}, "x":[x]}</param>
    public void onReceiveServerExtra (string data) {
        if (OnReceiveServerExtra != null && OnReceiveServerExtra.GetInvocationList ().Length > 0) {
            OnReceiveServerExtra (data);
        }
    }

    /// <summary>
    /// ��ȡ��֪ͨ����Ϣ���ݵĻص�����SDK��ʼ����ɣ���һ��ȡ�����ݺ���Զ����ø÷����������Ҫ������ǰ��Ӽ�����
    /// </summary>
    /// <param name="data">��̨���õ�����</param>
    public void onReceiveNotificationData (string data) {
        if (OnReceiveNotificationData != null && OnReceiveNotificationData.GetInvocationList ().Length > 0) {
            OnReceiveNotificationData (data);
        }
    }

    /// <summary>
    /// ��ʾ��Ƶ���Ľ���ص�������SDK�Զ����á�
    /// </summary>
    /// <param name="data">���صĽ������</param>
    public void onReceiveReward (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            bool success = false;
            int id = -1;
            string tag = "Default";
            if (!string.IsNullOrEmpty (data)) {
                string[] results = data.Split ('|');
                if (results != null && results.Length > 1) {
                    success = int.Parse (results[0]) == 0;
                    id = int.Parse (results[1]);
                    if (results.Length > 2) {
                        tag = results[2];
                    }
                }
            }
            if (success) {
                OnAdEvent (RiseSdk.AdEventType.RewardAdShowFinished, id, tag, RiseSdk.ADTYPE_VIDEO);
            } else {
                OnAdEvent (RiseSdk.AdEventType.RewardAdShowFailed, id, tag, RiseSdk.ADTYPE_VIDEO);
            }
        }
    }

    /// <summary>
    /// ������汻�رյĻص�������SDK�Զ����á�
    /// </summary>
    /// <param name="data">���ص�����</param>
    public void onFullAdClosed (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "";
            if (!string.IsNullOrEmpty (data)) {
                string[] msg = data.Split ('|');
                if (msg != null && msg.Length > 0) {
                    tag = msg[0];
                }
            }
            OnAdEvent (RiseSdk.AdEventType.FullAdClosed, -1, tag, RiseSdk.ADTYPE_INTERTITIAL);
        }
    }

    /// <summary>
    /// ������汻����Ļص�������SDK�Զ���á��    /// </summary>
    /// <param name="data">���ص�����</param>
    public void onFullAdClicked (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "";
            if (!string.IsNullOrEmpty (data)) {
                string[] msg = data.Split ('|');
                if (msg != null && msg.Length > 0) {
                    tag = msg[0];
                }
            }
            OnAdEvent (RiseSdk.AdEventType.FullAdClicked, -1, tag, RiseSdk.ADTYPE_INTERTITIAL);
        }
    }

    /// <summary>
    /// ��Ƶ��汻�رյĻص�������SDK�Զ����á�
    /// </summary>
    /// <param name="data">���ص�����</param>
    public void onVideoAdClosed (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "";
            if (!string.IsNullOrEmpty (data)) {
                string[] msg = data.Split ('|');
                if (msg != null && msg.Length > 0) {
                    tag = msg[0];
                }
            }
            OnAdEvent (RiseSdk.AdEventType.RewardAdClosed, -1, tag, RiseSdk.ADTYPE_VIDEO);
        }
    }

    /// <summary>
    /// banner��汻����Ļص�������SDK�Զ����á�
    /// </summary>
    /// <param name="data">���ص�����</param>
    public void onBannerAdClicked (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "";
            if (!string.IsNullOrEmpty (data)) {
                string[] msg = data.Split ('|');
                if (msg != null && msg.Length > 0) {
                    tag = msg[0];
                }
            }
            OnAdEvent (RiseSdk.AdEventType.BannerAdClicked, -1, tag, RiseSdk.ADTYPE_BANNER);
        }
    }

    /// <summary>
    /// �����ƹ��汻����Ļص�������SDK�Զ����á�
    /// </summary>
    /// <param name="data">���ص�����</param>
    public void onCrossAdClicked (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "";
            if (!string.IsNullOrEmpty (data)) {
                string[] msg = data.Split ('|');
                if (msg != null && msg.Length > 0) {
                    tag = msg[0];
                }
            }
            OnAdEvent (RiseSdk.AdEventType.CrossAdClicked, -1, tag, RiseSdk.ADTYPE_OTHER);
        }
    }
#elif UNITY_IOS
    /// <summary>
    /// ��������Ƶ���Ļص��¼�
    /// </summary>
    public static event Action<RiseSdk.AdEventType, int, string, int> OnAdEvent;
    ///// <summary>
    ///// ��Ƶ���Ļص��¼�
    ///// </summary>
    //public static event Action<RiseSdk.AdEventType, int, string> OnRewardAdEvent;
    /// <summary>
    /// ֧���Ļص��¼�
    /// </summary>
    public static event Action<RiseSdk.PaymentResult, int> OnPaymentEvent;
    public static event Action<int, bool> OnCheckSubscriptionResult;
    public static event Action OnRestoreFailureEvent;
    public static event Action<int> OnRestoreSuccessEvent;
    public static event Action<RiseSdk.SnsEventType, int> OnSNSEvent;
    private static RiseSdkListener _instance;


    public static RiseSdkListener Instance {
        get {
            if (!_instance) {
                // check if there is a IceTimer instance already available in the scene graph
                _instance = FindObjectOfType (typeof (RiseSdkListener)) as RiseSdkListener;
                // nope, create a new one
                if (!_instance) {
                    var obj = new GameObject ("RiseSdkListener");
                    _instance = obj.AddComponent<RiseSdkListener> ();
                    DontDestroyOnLoad (obj);
                }
            }
            return _instance;
        }
    }

    public void adReward (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            int placementId = -1;
            if (!string.IsNullOrEmpty (data)) {
                string[] str = data.Split ('|');
                if (str.Length == 1) {
                    tag = str [0];
                } else if (str.Length >= 2) {
                    tag = str[0];
                    int.TryParse (str[1], out placementId);
                }
            }
            OnAdEvent (RiseSdk.AdEventType.RewardAdShowFinished, placementId, tag, RiseSdk.ADTYPE_VIDEO);
        }
    }

    public void adLoaded (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            int adType = -1;
            if (!string.IsNullOrEmpty (data)) {
                string[] str = data.Split ('|');
                if (str.Length == 1) {
                    tag = str [0];
                } else if (str.Length >= 2) {
                    tag = str[0];
                    int.TryParse (str[1], out adType);
                }
            }
            OnAdEvent (RiseSdk.AdEventType.AdLoadCompleted, -1, tag, adType);
        }
    }

    public void adFailed (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            int adType = -1;
            if (!string.IsNullOrEmpty (data)) {
                string[] str = data.Split ('|');
                if (str.Length == 1) {
                    tag = str [0];
                } else if (str.Length >= 2) {
                    tag = str[0];
                    int.TryParse (str[1], out adType);
                }
            }
            OnAdEvent (RiseSdk.AdEventType.AdLoadFailed, -1, tag, adType);
        }
    }

    public void adDidShown (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            int adType = -1;
            if (!string.IsNullOrEmpty (data)) {
                string[] str = data.Split ('|');
                if (str.Length == 1) {
                    tag = str [0];
                } else if (str.Length >= 2) {
                    tag = str[0];
                    int.TryParse (str[1], out adType);
                }
            }
            OnAdEvent (RiseSdk.AdEventType.AdShown, -1, tag, adType);
        }
    }

    public void adDidClose (string data) {
        if (OnAdEvent != null && OnAdEvent.GetInvocationList ().Length > 0) {
            string tag = "Default";
            int adType = -1;
            if (!string.IsNullOrEmpty (data)) {
                string[] str = data.Split ('|');
                if (str.Length == 1) {
                    tag = str [0];
                } else if (str.Length >= 2) {
                    tag = str[0];
                    int.TryParse (str[1], out adType);
                }
            }
            OnAdEvent (RiseSdk.AdEventType.AdClosed, -1, tag, adType);
        }
    }

    public void onPaymentSuccess (string billingId) {
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PaymentResult.Success, int.Parse (billingId));
        }
    }

    public void onPaymentFailure (string billingId) {
        if (OnPaymentEvent != null && OnPaymentEvent.GetInvocationList ().Length > 0) {
            OnPaymentEvent (RiseSdk.PaymentResult.Failed, int.Parse (billingId));
        }
    }

    public void onCheckSubscriptionResult (string data) {
        if (OnCheckSubscriptionResult != null && OnCheckSubscriptionResult.GetInvocationList ().Length > 0) {
            int billingId = -1;
            bool active = false;
            if (!string.IsNullOrEmpty (data)) {
                string[] str = data.Split (',');
                if (str.Length == 2) {
                    billingId = int.Parse (str[0]);
                    active = int.Parse (str[1]) > 0 ? true : false;
                }
            }
            OnCheckSubscriptionResult (billingId, active);
        }
    }

    public void onRestoreFailure (string error) {
        if (OnRestoreFailureEvent != null && OnRestoreFailureEvent.GetInvocationList ().Length > 0) {
            OnRestoreFailureEvent ();
        }
    }

    public void onRestoreSuccess (string billingId) {
        if (OnRestoreSuccessEvent != null && OnRestoreSuccessEvent.GetInvocationList ().Length > 0) {
            OnRestoreSuccessEvent (int.Parse (billingId));
        }
    }

    public void snsShareSuccess (string data) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (RiseSdk.SnsEventType.ShareSuccess, 0);
        }
    }

    public void snsShareFailure (string data) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (RiseSdk.SnsEventType.ShareFailed, 0);
        }
    }

    public void snsShareDidCancel (string data) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (RiseSdk.SnsEventType.ShareCancel, 0);
        }
    }

    public void snsLoginSuccess (string data) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (RiseSdk.SnsEventType.LoginSuccess, 0);
        }
    }

    public void snsLoginFailure (string data) {
        if (OnSNSEvent != null && OnSNSEvent.GetInvocationList ().Length > 0) {
            OnSNSEvent (RiseSdk.SnsEventType.LoginFailed, 0);
        }
    }

#endif
}
