using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievemntNotificationEvent : MonoBehaviour
{

    public void Close()
    {
        DataAchievementNotifications.CloseAnimation?.Invoke();

    }

}
