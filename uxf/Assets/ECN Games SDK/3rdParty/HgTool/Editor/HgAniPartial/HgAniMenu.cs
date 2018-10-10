using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class HgAnimationKey
{
    private void OnGUI_MenuNew()
    {
        if (m_dicEditClip == null)
        {
            return;
        }

        m_v2MenuScrool = EditorGUILayout.BeginScrollView(m_v2MenuScrool);

        MenuPos();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        MenuFind();
        MenuRevert();
        MenuClearAll();
        EditorGUILayout.Space();
        MenuSearchAnimationClip();
        EditorGUILayout.Space();
        MenuSearchEventFunction();
        EditorGUILayout.Space();
        MenuEventKeyAdd();

        EditorGUILayout.EndScrollView();
    }

    private void MenuPos()
    {
        if (m_eMenuPos == EMenuPos.Left)
        {
            if (GUILayout.Button("Right Pivot") == true)
            {
                m_eMenuPos = EMenuPos.Right;
            }
        }
        else if (m_eMenuPos == EMenuPos.Right)
        {
            if (GUILayout.Button("Left Pivot") == true)
            {
                m_eMenuPos = EMenuPos.Left;
            }
        }
        else { }
    }

    private void MenuFind()
    {
        GUI.color = Color.green;

        if (GUILayout.Button("FIND") == true)
        {
            m_dicFoldout.Clear();
            m_dicEditClip.Clear();
            m_dicBackUp.Clear();
            m_lstKeyName.Clear();

            Dictionary<string, AnimationClip> dicTempClip = new Dictionary<string, AnimationClip>();

            if (FindAnimationClip(ref dicTempClip))
            {
                foreach (KeyValuePair<string, AnimationClip> _keyValue in dicTempClip)
                {
                    AnimationClip clip = _keyValue.Value;

                    UnityEngine.AnimationEvent[] anievents = AnimationUtility.GetAnimationEvents(clip);

                    m_dicFoldout.Add(clip, true);

                    List<AnimationEvent> lstEvent = new List<AnimationEvent>();

                    if (anievents != null)
                    {
                        lstEvent.AddRange(anievents);
                    }

                    if (m_dicEditClip.ContainsKey(clip))
                    {
                        new System.Exception();
                    }
                    else
                    {
                        m_dicEditClip.Add(clip, lstEvent);
                        AddBackUp(clip, ref lstEvent);

                        HgUtil.DisplayProgressBar("AniEvent Find", clip, m_dicEditClip.Count, dicTempClip.Count);
                    }
                }
                EditorUtility.ClearProgressBar();
            }

            SortAniClipList();
        }

        GUI.color = Color.white;
    }

    private void MenuRevert()
    {
        if (GUILayout.Button("REVERT") == false)
        {
            return;
        }

        if (m_dicBackUp != null)
        {
            foreach (KeyValuePair<AnimationClip, List<AnimationEvent>> _keyValue in m_dicBackUp)
            {
                AnimationClip clip = _keyValue.Key;
                List<AnimationEvent> lstItem = _keyValue.Value;

                List<AnimationEvent> lstSource = new List<AnimationEvent>();

                foreach (AnimationEvent item in lstItem)
                {
                    AnimationEvent aniEvent = HgUtil.CopyAnimationEvent(item);
                    if (aniEvent == null)
                    {
                        continue;
                    }
                    lstSource.Add(aniEvent);
                }

                if (clip != null && lstSource != null)
                {
                    if (m_dicEditClip.ContainsKey(clip))
                    {
                        List<AnimationEvent> lstDest = m_dicEditClip[clip];

                        lstDest.Clear();
                        lstDest.AddRange(lstSource.ToArray());

                        AnimationUtility.SetAnimationEvents(clip, lstDest.ToArray());
                    }
                }

                lstSource.Clear();
            }
        }
    }

    private void MenuClearAll()
    {
        if (GUILayout.Button("CLEAR") == false)
        {
            return;
        }

        if (null != m_dicFoldout) m_dicFoldout.Clear();
        if (null != m_dicEditClip) m_dicEditClip.Clear();
        if (null != m_dicBackUp) m_dicBackUp.Clear();
        if (null != m_lstKeyName) m_lstKeyName.Clear();
    }

    private void MenuSearchAnimationClip()
    {
        EditorGUILayout.LabelField("Search AnimationClip", HgUtil.CreateGUIStyle(11, new Color(0.8f, 0.8f, 0.8f, 1.0f), FontStyle.Bold));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Filter", GUILayout.Width(70.0f));
        if (m_lstClipName != null)
        {
            m_lstClipName.Clear();
            m_lstClipName.Add("ALL");

            foreach (KeyValuePair<AnimationClip, List<AnimationEvent>> _keyValue in m_dicEditClip)
            {
                m_lstClipName.Add(_keyValue.Key.name);
            }

            m_nSerachAnimationClipPopup = EditorGUILayout.Popup(m_nSerachAnimationClipPopup, m_lstClipName.ToArray(), EditorStyles.popup);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("String", GUILayout.Width(70.0f));
        m_serachAnimationClipString = EditorGUILayout.TextField(m_serachAnimationClipString, (GUIStyle)"SearchTextField");
        EditorGUILayout.EndHorizontal();
    }

    private void MenuSearchEventFunction()
    {
        EditorGUILayout.LabelField("Search Function", HgUtil.CreateGUIStyle(11, new Color(0.8f, 0.8f, 0.8f, 1.0f), FontStyle.Bold));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Function", GUILayout.Width(70.0f));
        m_strFindKeyEvent = EditorGUILayout.TextField(m_strFindKeyEvent, (GUIStyle)"SearchTextField");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Parmater", GUILayout.Width(70.0f));
        m_strFindKeyParmater = EditorGUILayout.TextField(m_strFindKeyParmater, (GUIStyle)"SearchTextField");
        EditorGUILayout.EndHorizontal();
    }

    private void MenuEventKeyAdd()
    {
        EditorGUILayout.LabelField("New Function", HgUtil.CreateGUIStyle(11, new Color(0.8f, 0.8f, 0.8f, 1.0f), FontStyle.Bold));

        EditorGUILayout.BeginHorizontal();

        m_strEventFuntionAdd = EditorGUILayout.TextField(m_strEventFuntionAdd);

        if (GUILayout.Button("ADD"))
        {
            if (string.IsNullOrEmpty(m_strEventFuntionAdd) == false)
            {
                if (m_lstKeyName != null)
                {
                    if (m_lstKeyName.Contains(m_strEventFuntionAdd) == false)
                    {
                        m_lstKeyName.Add(m_strEventFuntionAdd);
                        m_strEventFuntionAdd = string.Empty;
                    }
                }
            }
        }

        EditorGUILayout.EndHorizontal();
    }
}