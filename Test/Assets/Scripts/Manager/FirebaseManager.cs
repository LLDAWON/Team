using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;
    private DatabaseReference _databaseReference;

    private void Awake()
    {
        instance = this;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

            // Firebase 초기화가 완료된 후에 추가적인 로직을 넣을 수 있습니다.
        });
    }

    public void SaveClearTime(string playerName, double clearTime)
    {
        string key = _databaseReference.Child("cleartimes").Push().Key;
        Dictionary<string, object> clearTimeData = new Dictionary<string, object>();
        clearTimeData["playerName"] = playerName;
        clearTimeData["clearTime"] = clearTime;

        _databaseReference.Child("cleartimes").Child(key).SetValueAsync(clearTimeData);
    }

    public void LoadTopClearTimes(int topCount, Action<List<ClearTimeEntry>> onComplete)
    {
        _databaseReference.Child("cleartimes")
                         .OrderByChild("clearTime")  // 클리어 타임 기준으로 정렬
                         .LimitToFirst(topCount)    // 상위 몇 개까지 가져올지 설정
                         .GetValueAsync().ContinueWithOnMainThread(task => {
                             if (task.IsFaulted)
                             {
                                 Debug.LogError("Failed to load clear times.");
                             }
                             else if (task.IsCompleted)
                             {
                                 DataSnapshot snapshot = task.Result;
                                 List<ClearTimeEntry> clearTimes = new List<ClearTimeEntry>();

                                 foreach (DataSnapshot childSnapshot in snapshot.Children)
                                 {
                                     string playerName = childSnapshot.Child("playerName").Value.ToString();
                                     double clearTime = Convert.ToDouble(childSnapshot.Child("clearTime").Value);

                                     clearTimes.Add(new ClearTimeEntry(playerName, clearTime));
                                 }

                                 // 클리어 타임 기준으로 오름차순 정렬
                                 clearTimes.Sort((a, b) => a.ClearTime.CompareTo(b.ClearTime));

                                 onComplete?.Invoke(clearTimes);
                             }
                         });
    }
}

public class ClearTimeEntry
{
    public string PlayerName { get; private set; }
    public double ClearTime { get; private set; }
    public string FormattedClearTime { get; private set; }

    public ClearTimeEntry(string playerName, double clearTime)
    {
        PlayerName = playerName;
        ClearTime = clearTime;

        // 클리어 타임을 시:분:초 형식으로 포맷팅
        TimeSpan timeSpan = TimeSpan.FromSeconds(clearTime);

        FormattedClearTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeSpan.Hours,
            timeSpan.Minutes,
            timeSpan.Seconds);
    }
}
