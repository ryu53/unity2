using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class IineMarker : MonoBehaviour, ITrackableEventHandler
{
    // いいね数の表示
    CanvasGroup iineCounter;
    Text iineCounterText;
    // ユーザーデータ
    UserData userData = null;

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    protected virtual void Start()
    {
        // ユーザーデータを取得
        userData = UserData.GetData();
        // いいねカウンターを取得
        GameObject iine = GameObject.Find("IineCounter");
        iineCounter = iine.GetComponent<CanvasGroup>();
        iineCounterText = iineCounter.GetComponentInChildren<Text>();
        iineCounter.alpha = 0;

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound(mTrackableBehaviour.TrackableName);
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            OnTrackingLost();
        }
        else
        {
            OnTrackingLost();
        }
    }

    protected virtual void OnTrackingFound(string markerName)
    {
        if (mTrackableBehaviour)
        {
            // いいねマーカーを記録
            userData.AddIineMarker(markerName);
            // いいねカウンターを表示
            iineCounter.alpha = 1;
            iineCounterText.text = userData.iineMarker.Count.ToString();
        }
    }

    protected virtual void OnTrackingLost()
    {
        iineCounter.alpha = 0;
    }
}