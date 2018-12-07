//===================================================
//作    者：周连康 
//创建时间：2018-12-05 20:05:34
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServerConfig {

    public static string SERVER_IP = "172.16.4.112";
    public static int SERVER_PORT = 80;

    public static string SongBaseUrl = "http://" + SERVER_IP + ":" + SERVER_PORT + "/res/song";
    public static string SongListUrl = SongBaseUrl + "/songList.json";
}
