using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class HgAnimationKey
{
    private void OnGUI_EventKeyEditor()
    {
        m_v2EditorScrool = EditorGUILayout.BeginScrollView(m_v2EditorScrool);

        AniClipCopyView();
        EditorAniClipList();

        EditorGUILayout.EndScrollView();
    }

    private void AniClipCopyView()
    {
        EditorGUILayout.BeginHorizontal();

        if (m_lstAniEventKeyCopy != null && m_lstAniEventKeyCopy.Count > 0)
        {
            System.IO.FileInfo fileInfo;
            HgUtil.GetFileInfo(out fileInfo, m_curCopyAniClip);

            string strMsg = string.Format("[EVENT_KEY_COPY] AniName({0}) EventKey Count({1})", fileInfo.Name, m_lstAniEventKeyCopy.Count);
            EditorGUILayout.LabelField(strMsg, HgUtil.CreateGUIStyle(12, Color.green, FontStyle.Bold));

            if (GUILayout.Button("CLEAR", GUILayout.Width(50)))
            {
                m_lstAniEventKeyCopy.Clear();
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void EditorAniClipList()
    {
        if (m_dicEditClip == null)
        {
            return;
        }

        foreach (KeyValuePair<AnimationClip, List<AnimationEvent>> _keyValue in m_dicEditClip)
        {
            AnimationClip clip = _keyValue.Key;
            List<AnimationEvent> lstEvent = _keyValue.Value;

            // serach string animation clip
            if (m_lstClipName != null &&
                m_lstClipName.Count > 0 &&
                m_nSerachAnimationClipPopup > -1)
            {
                string strPopUpName = m_lstClipName[Mathf.Clamp(m_nSerachAnimationClipPopup, 0, m_lstClipName.Count - 1)];
                if (HgUtil.IsCompareStr("ALL", strPopUpName) == false)
                {
                    if (HgUtil.IsCompareStr(clip.name, strPopUpName, false) == false)
                    {
                        continue;
                    }
                }
            }

            // serach string animation clip
            if (string.IsNullOrEmpty(m_serachAnimationClipString) == false)
            {
                if (HgUtil.IsCompareIncludeStr(clip.name, m_serachAnimationClipString, true) == false)
                {
                    continue;
                }
            }

            System.IO.FileInfo fileInfo;
            HgUtil.GetFileInfo(out fileInfo, clip);

            EditorGUILayout.Space();

            m_dicFoldout[clip] = EditorGUILayout.Foldout(m_dicFoldout[clip], fileInfo.Name);
            if (m_dicFoldout[clip] == false)
            {
                continue;
            }

            int nChangeCount = 0;

            EditorGUILayout.BeginHorizontal();

            float fMaxFrame = clip.length * clip.frameRate;
            string strInfo = string.Format("Frame({0}) Time({1}) Sample({2}) Path({3})", fMaxFrame, clip.length, clip.frameRate, fileInfo.Directory);
            EditorGUILayout.HelpBox(strInfo, MessageType.None);

            float fBtnWidth = 80.0f;

            if (GUILayout.Button("Trace", GUILayout.Width(fBtnWidth)))
            {
                //Selection.activeObject = clip;
                ProjectWindowUtil.ShowCreatedAsset(clip);
            }

            if (GUILayout.Button("Revert", GUILayout.Width(fBtnWidth)))
            {
                if (m_dicBackUp.ContainsKey(clip))
                {
                    List<AnimationEvent> lstItem = m_dicBackUp[clip];
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

                    lstEvent.Clear();
                    lstEvent.AddRange(lstSource.ToArray());

                    lstSource.Clear();

                    nChangeCount++;
                }
            }

            if (lstEvent != null && lstEvent.Count > 0)
            {
                if (GUILayout.Button("Replace", GUILayout.Width(fBtnWidth)))
                {
                    SortTime(ref lstEvent);
                    nChangeCount++;
                }

                if (GUILayout.Button("Copy", GUILayout.Width(fBtnWidth)))
                {
                    m_curCopyAniClip = clip;

                    m_lstAniEventKeyCopy.Clear();

                    foreach (AnimationEvent item in lstEvent)
                    {
                        AnimationEvent aniEvent = HgUtil.CopyAnimationEvent(item);
                        if (aniEvent == null)
                        {
                            continue;
                        }

                        m_lstAniEventKeyCopy.Add(aniEvent);
                    }
                }
            }

            if (m_lstAniEventKeyCopy != null && m_lstAniEventKeyCopy.Count > 0)
            {
                if (GUILayout.Button("Paste", GUILayout.Width(fBtnWidth)))
                {
                    lstEvent.AddRange(m_lstAniEventKeyCopy.ToArray());
                    SortTime(ref lstEvent);
                    nChangeCount++;
                }
            }

            if (GUILayout.Button("＋", GUILayout.Width(20)))
            {
                AnimationEvent aniEvent = new AnimationEvent();
                aniEvent.time = clip.length;
                aniEvent.functionName = "NEW_EVENT";
                lstEvent.Add(aniEvent);
                nChangeCount++;
            }

            EditorAniClipEventList(lstEvent, clip, ref nChangeCount, fMaxFrame);

            if (nChangeCount > 0)
            {
                AnimationUtility.SetAnimationEvents(clip, lstEvent.ToArray());
            }
        }
    }

    private void EditorAniClipEventList(List<AnimationEvent> lstEvent, AnimationClip clip, ref int nChangeCount, float fMaxFrame)
    {
        EditorGUILayout.EndHorizontal();

        if (lstEvent != null && lstEvent.Count > 0)
        {
            GUIStyle titleStyle = HgUtil.CreateGUIStyle(10, new Color(0.8f, 0.8f, 0.8f), FontStyle.Bold);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Frame", titleStyle, GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("KeyEvent", titleStyle, GUILayout.Width(200));
            EditorGUILayout.LabelField("Float", titleStyle, GUILayout.Width(100));
            EditorGUILayout.LabelField("Int", titleStyle, GUILayout.Width(100));
            EditorGUILayout.LabelField("String", titleStyle, GUILayout.Width(250));
            EditorGUILayout.LabelField("", titleStyle, GUILayout.Width(20));
            EditorGUILayout.EndHorizontal();

            AnimationEvent delEvent = null;

            for (int index = 0; index < lstEvent.Count; index++)
            {
                AnimationEvent aniEvent = lstEvent[index];
                if (aniEvent == null)
                {
                    continue;
                }

                string strFunName = aniEvent.functionName;
                float paramFloat = aniEvent.floatParameter;
                int paramInt = aniEvent.intParameter;
                string paramString = aniEvent.stringParameter;

                if (m_lstKeyName.Contains(aniEvent.functionName) == false)
                {
                    m_lstKeyName.Add(aniEvent.functionName);
                }

                // serach event function
                if (string.IsNullOrEmpty(m_strFindKeyEvent) == false)
                {
                    if (HgUtil.IsCompareIncludeStr(aniEvent.functionName, m_strFindKeyEvent, true) == false)
                    {
                        continue;
                    }
                }

                // serach event function parmater
                if (string.IsNullOrEmpty(m_strFindKeyParmater) == false)
                {
                    int nSearch = 0;

                    string floatParameter = aniEvent.floatParameter.ToString();
                    string intParameter = aniEvent.intParameter.ToString();
                    string stringParameter = aniEvent.stringParameter;

                    if (string.IsNullOrEmpty(floatParameter) == false)
                    {
                        if (HgUtil.IsCompareIncludeStr(floatParameter, m_strFindKeyParmater, true) == true)
                        {
                            nSearch++;
                        }
                    }

                    if (string.IsNullOrEmpty(intParameter) == false)
                    {
                        if (HgUtil.IsCompareIncludeStr(intParameter, m_strFindKeyParmater, true) == true)
                        {
                            nSearch++;
                        }
                    }

                    if (string.IsNullOrEmpty(stringParameter) == false)
                    {
                        if (HgUtil.IsCompareIncludeStr(stringParameter, m_strFindKeyParmater, true) == true)
                        {
                            nSearch++;
                        }
                    }

                    if (nSearch <= 0)
                    {
                        continue;
                    }
                }

                EditorGUILayout.BeginHorizontal();

                //int nOldFrame = (int)(aniEvent.time * clip.frameRate);
                //int nNewFrame = EditorGUILayout.IntSlider(nOldFrame, 0, (int)fMaxFrame, GUILayout.ExpandWidth(true));
                float fOldFrame = (aniEvent.time * clip.frameRate);
                float fNewFrame = EditorGUILayout.Slider(fOldFrame, 0.0f, fMaxFrame, GUILayout.ExpandWidth(true));

                fNewFrame = Mathf.Clamp(fNewFrame, 0.0f, fMaxFrame);
                if (float.Equals(fOldFrame, fNewFrame) == false)
                {
                    aniEvent.time = fNewFrame / clip.frameRate;
                    nChangeCount++;
                }

                int nSelectName = GetKeyNameIndex(aniEvent.functionName);
                int nNewSelectName = EditorGUILayout.Popup(nSelectName, m_lstKeyName.ToArray(), GUILayout.Width(200));

                aniEvent.functionName = m_lstKeyName[nNewSelectName];
                nChangeCount = aniEvent.functionName != strFunName ? nChangeCount + 1 : nChangeCount;

                aniEvent.floatParameter = EditorGUILayout.FloatField(aniEvent.floatParameter, GUILayout.Width(100));
                nChangeCount = aniEvent.floatParameter != paramFloat ? nChangeCount + 1 : nChangeCount;

                aniEvent.intParameter = EditorGUILayout.IntField(aniEvent.intParameter, GUILayout.Width(100));
                nChangeCount = aniEvent.intParameter != paramInt ? nChangeCount + 1 : nChangeCount;

                aniEvent.stringParameter = EditorGUILayout.TextField(aniEvent.stringParameter, GUILayout.Width(250));
                nChangeCount = aniEvent.stringParameter != paramString ? nChangeCount + 1 : nChangeCount;

                if (GUILayout.Button("－", GUILayout.Width(20)))
                {
                    delEvent = aniEvent;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (delEvent != null)
            {
                lstEvent.Remove(delEvent);
                nChangeCount++;
            }
        }
    }

    private bool IsSearchAniEvent(AnimationEvent aniEvent)
    {
        int nSearch = 0;

        // serach event function
        if (string.IsNullOrEmpty(m_strFindKeyEvent) == true)
        {
            nSearch++;
        }
        else
        {
            if (HgUtil.IsCompareIncludeStr(aniEvent.functionName, m_strFindKeyEvent, true) == true)
            {
                nSearch++;
            }
        }

        // serach event function parmater
        if (string.IsNullOrEmpty(m_strFindKeyParmater) == true)
        {
            nSearch++;
        }
        else
        {
            float fFloat = 0.0f;
            float.TryParse(m_strFindKeyParmater, out fFloat);
            if (true == float.Equals(fFloat, aniEvent.floatParameter))
            {
                nSearch++;
            }

            int nInt = 0;
            int.TryParse(m_strFindKeyParmater, out nInt);
            if (true == int.Equals(nInt, aniEvent.intParameter))
            {
                nSearch++;
            }

            if (HgUtil.IsCompareIncludeStr(m_strFindKeyParmater, aniEvent.stringParameter, true) == true)
            {
                nSearch++;
            }
        }

        return (nSearch > 0);
    }
}