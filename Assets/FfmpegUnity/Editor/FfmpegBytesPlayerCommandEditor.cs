using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FfmpegUnity
{
    [CustomEditor(typeof(FfmpegBytesPlayerCommand))]
    public class FfmpegBytesPlayerCommandEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Undo.RecordObject(target, "Parameter Change");

            EditorGUI.BeginChangeCheck();

            FfmpegBytesPlayerCommand ffmpegPlayerCommand = (FfmpegBytesPlayerCommand)target;

            SerializedObject so = serializedObject;

            ffmpegPlayerCommand.ExecuteOnStart = EditorGUILayout.Toggle("Execute On Start", ffmpegPlayerCommand.ExecuteOnStart);

            SerializedProperty inputsProperty = so.FindProperty("InputByteOptions");
            EditorGUILayout.PropertyField(inputsProperty, new GUIContent("Input Options"), true);

            ffmpegPlayerCommand.AutoStreamSettings = EditorGUILayout.Toggle("Auto Settings", ffmpegPlayerCommand.AutoStreamSettings);
            if (!ffmpegPlayerCommand.AutoStreamSettings && !EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
            {
                if (ffmpegPlayerCommand.Streams == null || ffmpegPlayerCommand.Streams.Length < 2)
                {
                    ffmpegPlayerCommand.Streams = new FfmpegPlayerCommand.FfmpegStream[] {
                        new FfmpegPlayerCommand.FfmpegStream()
                        {
                            CodecType = FfmpegPlayerCommand.FfmpegStream.Type.VIDEO,
                            Width = 640,
                            Height = 480,
                        },
                        new FfmpegPlayerCommand.FfmpegStream()
                        {
                            CodecType = FfmpegPlayerCommand.FfmpegStream.Type.AUDIO,
                            Channels = 2,
                            SampleRate = 48000,
                        },
                    };
                }

                Vector2Int videoSize = EditorGUILayout.Vector2IntField("Video Size",
                    new Vector2Int(ffmpegPlayerCommand.Streams[0].Width, ffmpegPlayerCommand.Streams[0].Height));
                ffmpegPlayerCommand.Streams[0].Width = videoSize.x;
                ffmpegPlayerCommand.Streams[0].Height = videoSize.y;

                ffmpegPlayerCommand.FrameRate = EditorGUILayout.FloatField("Video Frame Rate", ffmpegPlayerCommand.FrameRate);

                ffmpegPlayerCommand.Streams[1].Channels = EditorGUILayout.IntField("Audio Channels", ffmpegPlayerCommand.Streams[1].Channels);
                ffmpegPlayerCommand.Streams[1].SampleRate = EditorGUILayout.IntField("Audio Sample Rate", ffmpegPlayerCommand.Streams[1].SampleRate);
            }

            EditorGUILayout.LabelField("Options");
            ffmpegPlayerCommand.PlayerOptions = EditorGUILayout.TextArea(ffmpegPlayerCommand.PlayerOptions);

            SerializedProperty videoProperty = so.FindProperty("VideoTextures");
            EditorGUILayout.PropertyField(videoProperty, true);

            SerializedProperty audioProperty = so.FindProperty("AudioSources");
            EditorGUILayout.PropertyField(audioProperty, true);

            ffmpegPlayerCommand.SyncFrameRate = EditorGUILayout.Toggle("Sync Frame Rate", ffmpegPlayerCommand.SyncFrameRate);

            ffmpegPlayerCommand.PrintStdErr = EditorGUILayout.Toggle("Print StdErr", ffmpegPlayerCommand.PrintStdErr);

            ffmpegPlayerCommand.BufferTime = EditorGUILayout.FloatField("Buffer Time", ffmpegPlayerCommand.BufferTime);

            so.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(ffmpegPlayerCommand);
            }
        }
    }
}
