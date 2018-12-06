//===================================================
//作    者：周连康 
//创建时间：2018-12-06 10:09:48
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuanKaLogic : Singleton<GuanKaLogic> {

    public List<GuanKa> GetGuanKaList()
    {
        List<GuanKa> guanKaList = new List<GuanKa>();

        List<Song> songList = SongManager.Instance.GetLocalSongList();

        if (songList != null)
        {
            foreach (var song in songList)
            {
                GuanKa guanka = new GuanKa();
                guanka.id = song.id;
                guanka.name = song.songTitle;
                guanka.song = song;
                guanKaList.Add(guanka);
            }
        }
        return guanKaList;
    }
}
