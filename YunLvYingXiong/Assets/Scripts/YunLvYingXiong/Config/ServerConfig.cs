//===================================================
//作    者：周连康 
//创建时间：2018-12-05 20:05:34
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServerConfig {

    public static string SERVER_IP = "127.0.0.1";
    public static int SERVER_PORT = 80;

    public static string SongListUrl = "http://" + SERVER_IP + ":" + SERVER_PORT + "/res/song/songList.json";
}
