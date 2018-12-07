//===================================================
//作    者：周连康 
//创建时间：2018-12-07 16:42:47
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按钮导航管理类
/// </summary>


class UINaviNode
{
    public GameObject leftObj;
    public GameObject rightObj;
    public GameObject upObj;
    public GameObject downObj;

    public void Clear()
    {
        leftObj = null;
        rightObj = null;
        upObj = null;
        downObj = null;
    }
}

public enum UINaviNodeRelation
{
    Left,
    Right,
    Up,
    Down
}

public class UINaviManager : Singleton<UINaviManager> {

    Dictionary<GameObject, UINaviNode> m_NaviMap = new Dictionary<GameObject, UINaviNode>();

    GameObject m_defaultObject;
    public GameObject DefaultObject
    {
        get
        {
            return m_defaultObject;
        }
        set
        {
            m_defaultObject = value;
        }
    }

    /// <summary>
    /// 添加一个导航信息
    /// </summary>
	public void AddNavi(GameObject fromObj, GameObject toObj, UINaviNodeRelation relation)
    {
        if (fromObj == null || toObj == null)
        {
            return;
        }

        UINaviNode naviNode = GetActiveNaviNode(fromObj);
        switch (relation)
        {
            case UINaviNodeRelation.Left:
                naviNode.leftObj = toObj;
                break;
            case UINaviNodeRelation.Right:
                naviNode.rightObj = toObj;
                break;
            case UINaviNodeRelation.Up:
                naviNode.upObj = toObj;
                break;
            case UINaviNodeRelation.Down:
                naviNode.downObj = toObj;
                break;
        }
    }

    public GameObject GetNavi(GameObject fromObj, UINaviNodeRelation relation)
    {
        GameObject toObj = null;
        UINaviNode naviNode = GetActiveNaviNode(fromObj);
        switch (relation)
        {
            case UINaviNodeRelation.Left:
                 toObj = naviNode.leftObj;
                break;
            case UINaviNodeRelation.Right:
                toObj = naviNode.rightObj;
                break;
            case UINaviNodeRelation.Up:
                toObj = naviNode.upObj;
                break;
            case UINaviNodeRelation.Down:
                toObj = naviNode.downObj;
                break;
        }
        return toObj;
    }

    /// <summary>
    /// 移除一个导航
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveNavi(GameObject obj)
    {
        UINaviNode naviNode = GetActiveNaviNode(obj);
        naviNode.Clear();
    }

    /// <summary>
    /// 获取一个有效的节点
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="fromObj"></param>
    /// <returns></returns>
    UINaviNode GetActiveNaviNode(GameObject fromObj)
    {
        UINaviNode node;
        if (!m_NaviMap.TryGetValue(fromObj, out node))
        {
            node = new UINaviNode();
            m_NaviMap.Add(fromObj, node);
        }
        return node;
    }
}
