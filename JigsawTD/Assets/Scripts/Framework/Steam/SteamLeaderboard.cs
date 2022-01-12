using UnityEngine;
using Steamworks;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System;

public class SteamLeaderboard : MonoBehaviour
{
    private const string s_leaderboardName = "EndlessMode";
    private const ELeaderboardUploadScoreMethod s_leaderboardMethod = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest;

    private static SteamLeaderboard_t s_currentLeaderboard;
    private static bool s_initialized = false;
    private static CallResult<LeaderboardFindResult_t> m_findResult = new CallResult<LeaderboardFindResult_t>();
    private static CallResult<LeaderboardScoreUploaded_t> m_uploadResult = new CallResult<LeaderboardScoreUploaded_t>();

    private static CallResult<LeaderboardScoresDownloaded_t> m_downloadResult = new CallResult<LeaderboardScoresDownloaded_t>();

    public static List<LeaderboardEntry_t> EndlessLeaderBoard = new List<LeaderboardEntry_t>();

    private void Start()
    {
        Init();
    }

    public static void UpdateScore(int score)
    {
        if (!s_initialized)
        {
            Debug.Log("Can't upload to the leaderboard because isn't loadded yet");
        }
        else
        {
            Debug.Log("uploading score(" + score + ") to steam leaderboard(" + s_leaderboardName + ")");
            SteamAPICall_t hSteamAPICall = SteamUserStats.UploadLeaderboardScore(s_currentLeaderboard, s_leaderboardMethod, score, null, 0);
            m_uploadResult.Set(hSteamAPICall, OnLeaderboardUploadResult);

        }
    }

    public static void DownloadScore()
    {
        if (!s_initialized)
        {
            Debug.Log("Can't download the leaderboard because isn't loadded yet");
        }
        else
        {
            Debug.Log("download(" + 50 + ") to steam leaderboard(" + s_leaderboardName + ")");
            EndlessLeaderBoard.Clear();
            SteamAPICall_t hSteamAPICall = SteamUserStats.DownloadLeaderboardEntries(s_currentLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 50);
            m_downloadResult.Set(hSteamAPICall, OnLeaderboardDownloadResult);

        }
    }

    public static void Init()
    {
        EndlessLeaderBoard.Clear();
        SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(s_leaderboardName);
        m_findResult.Set(hSteamAPICall, OnLeaderboardFindResult);
    }

    static private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool failure)
    {
        UnityEngine.Debug.Log("STEAM LEADERBOARDS: Found - " + pCallback.m_bLeaderboardFound + " leaderboardID - " + pCallback.m_hSteamLeaderboard.m_SteamLeaderboard);
        s_currentLeaderboard = pCallback.m_hSteamLeaderboard;
        s_initialized = true;
        DownloadScore();
    }

    static private void OnLeaderboardUploadResult(LeaderboardScoreUploaded_t pCallback, bool failure)
    {
        UnityEngine.Debug.Log("STEAM LEADERBOARDS: failure - " + failure + " Completed - " + pCallback.m_bSuccess + " NewScore: " + pCallback.m_nGlobalRankNew + " Score " + pCallback.m_nScore + " HasChanged - " + pCallback.m_bScoreChanged);
    }

    static private void OnLeaderboardDownloadResult(LeaderboardScoresDownloaded_t pCallback, bool failure)
    {
        UnityEngine.Debug.Log("STEAM LEADERBOARDS: failure - " + failure + " Completed - " + pCallback.m_cEntryCount);
        if (!failure)
        {
            int m_nLeaderboardEntries = Mathf.Min(pCallback.m_cEntryCount, 50);
            for (int i = 0; i < m_nLeaderboardEntries; i++)
            {
                LeaderboardEntry_t leaderBoardEntry;
                SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, i, out leaderBoardEntry, null, 0);
                
                EndlessLeaderBoard.Add(leaderBoardEntry);
            }
        }
    }




    private static Timer timer1;
    public static void InitTimer()
    {
        timer1 = new Timer(timer1_Tick, null, 0, 1000);
    }

    private static void timer1_Tick(object state)
    {
        SteamAPI.RunCallbacks();
        Debug.Log("Yes");
    }
}

