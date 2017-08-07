using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CreateAR.Commons.Unity.Editor
{
    /// <summary>
    /// Set of utilities for working in the editor.
    /// </summary>
    public static class EditorUtils
    {
        /// <summary>
        /// Stack for enabled booleans.
        /// </summary>
        private static readonly Stack<bool> _enabledStack = new Stack<bool>();

        /// <summary>
        /// Parallel lists of coroutines + associated ids.
        /// </summary>
        private static readonly List<IEnumerator> _coroutines = new List<IEnumerator>();
        private static readonly List<uint> _coroutineIds = new List<uint>();

        /// <summary>
        /// Keys ids.
        /// </summary>
        private static uint IDS = 0;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static EditorUtils()
        {
            EditorApplication.update -= EditorApplication_OnUpdate;
            EditorApplication.update += EditorApplication_OnUpdate;
        }

        /// <summary>
        /// Starts a coroutine. Returns a unique id for the coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine to begin.</param>
        /// <returns></returns>
        public static uint StartCoroutine(IEnumerator coroutine)
        {
            var id = ++IDS;
            _coroutines.Add(coroutine);
            _coroutineIds.Add(id);

            return id;
        }

        /// <summary>
        /// Stops a coroutine by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool StopCoroutine(uint id)
        {
            var index = _coroutineIds.IndexOf(id);
            if (-1 == index)
            {
                return false;
            }

            _coroutineIds.RemoveAt(index);
            _coroutines.RemoveAt(index);

            return true;
        }
        
        /// <summary>
        /// Pushes current GUI.enabled flag onto stack and replaces value.
        /// </summary>
        /// <param name="enabled">Value to set for GUI.enabled.</param>
        public static void PushEnabled(bool enabled)
        {
            _enabledStack.Push(GUI.enabled);
            GUI.enabled = enabled;
        }

        /// <summary>
        /// Pops enabled flag from stack back into GUI.enabled.
        /// </summary>
        public static void PopEnabled()
        {
            GUI.enabled = _enabledStack.Pop();
        }

        /// <summary>
        /// Returns true iff the mouse event clicked the last control drawn.
        /// </summary>
        public static bool WasLastRectClicked()
        {
            var rect = GUILayoutUtility.GetLastRect();
            var @event = Event.current;

            return @event.type == EventType.mouseUp
                && rect.Contains(@event.mousePosition);
        }

        /// <summary>
        /// Called every EditorApplication update.
        /// </summary>
        private static void EditorApplication_OnUpdate()
        {
            var coroutineCopy = _coroutines.ToArray();
            var coroutineIdsCopy = _coroutineIds.ToArray();

            _coroutines.Clear();
            _coroutineIds.Clear();

            for (int i = 0, len = coroutineCopy.Length; i < len; i++)
            {
                if (coroutineCopy[i].MoveNext())
                {
                    _coroutines.Add(coroutineCopy[i]);
                    _coroutineIds.Add(coroutineIdsCopy[i]);
                }
            }
        }
    }
}
