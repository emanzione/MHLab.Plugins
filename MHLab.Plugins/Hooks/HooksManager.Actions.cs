using System;
using System.Collections.Generic;

namespace MHLab.Plugins
{
    public partial class HooksManager
    {
        protected static Dictionary<String, SortedList<Int32, Action<Object>>> m_actions = new Dictionary<String, SortedList<Int32, Action<Object>>>();

        /// <summary>
        /// Returns the amount of registered actions.
        /// </summary>
        public static Int32 ActionsCount
        {
            get
            {
                return m_actions.Count;
            }
        }

        /// <summary>
        /// Provides for a simple way to register an action in PluginManager.
        /// </summary>
        /// <param name="actionName">An unique string that represents the action.</param>
        /// <param name="callback">The associated function to this action.</param>
        /// <param name="priority">The associated function priority.</param>
        public static void RegisterAction(String actionName, Action<Object> callback, Int32 priority)
        {
            // If actionName or callback aren't valid, throw an exception.
            if (actionName == null || actionName == String.Empty || callback == null) throw new ArgumentException("Arguments cannot be null or empty");

            // If this action doesn't exist yet, create its own entry
            if (!m_actions.ContainsKey(actionName))
            {
                m_actions.Add(actionName, new SortedList<Int32, Action<Object>>());
            }

            if (m_actions[actionName].ContainsKey(priority))
            {
                // If it already exists, overwrite it
                m_actions[actionName][priority] = callback;
            }
            else
            {
                // If not, finally, add that entry
                m_actions[actionName].Add(priority, callback);
            }
        }

        /// <summary>
        /// Executes all functions bound to a action on input object.
        /// </summary>
        /// <typeparam name="T">The type of input object.</typeparam>
        /// <param name="actionName">An unique string that represents the action.</param>
        /// <param name="input">The input object.</param>
        /// <returns></returns>
        public static void ExecuteAction(String actionName, Object input)
        {
            // If this action doesn't exist, return the input object with no actioning
            if (!m_actions.ContainsKey(actionName)) return;

            // Execute each action on input object.
            for (int i = 0; i < m_actions[actionName].Count; i++)
            {
                m_actions[actionName][i].Invoke(input);
            }
        }

        /// <summary>
        /// Returns the amount of registered actions associated to a action name.
        /// </summary>
        /// <param name="actionName">An unique string that represents the action.</param>
        /// <returns></returns>
        public static Int32 RegisteredActionsAmount(String actionName)
        {
            // If this action doesn't exist, return 0
            if (!m_actions.ContainsKey(actionName)) return 0;

            return m_actions[actionName].Count;
        }

        /// <summary>
        /// Remove and unbound all registered actions.
        /// </summary>
        public static void ClearActions()
        {
            m_actions.Clear();
        }
    }
}
