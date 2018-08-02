using System;
using System.Collections.Generic;

namespace MHLab.Plugins
{
    public partial class HooksManager
    {
        protected static Dictionary<String, SortedList<Int32, Func<Object, Object>>> m_filters = new Dictionary<String, SortedList<Int32, Func<Object, Object>>>();

        /// <summary>
        /// Returns the amount of registered filters.
        /// </summary>
        public static Int32 FiltersCount
        {
            get
            {
                return m_filters.Count;
            }
        }
        
        /// <summary>
        /// Provides for a simple way to register a filter in PluginManager.
        /// </summary>
        /// <param name="filterName">An unique string that represents the filter.</param>
        /// <param name="callback">The associated function to this filter.</param>
        /// <param name="priority">The associated function priority.</param>
        public static void RegisterFilter(String filterName, Func<Object, Object> callback, Int32 priority)
        {
            // If filterName or callback aren't valid, throw an exception.
            if (string.IsNullOrEmpty(filterName) || callback == null) throw new ArgumentException("Arguments cannot be null or empty");

            // If this filter doesn't exist yet, create its own entry
            if (!m_filters.ContainsKey(filterName))
            {
                m_filters.Add(filterName, new SortedList<Int32, Func<Object, Object>>());
            }

            if (m_filters[filterName].ContainsKey(priority))
            {
                // If it already exists, overwrite it
                m_filters[filterName][priority] = callback;
            }
            else
            {
                // If not, finally, add that entry
                m_filters[filterName].Add(priority, callback);
            }
        }

        /// <summary>
        /// Executes all functions bound to a filter on input object and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of input object.</typeparam>
        /// <param name="filterName">An unique string that represents the filter.</param>
        /// <param name="input">The input object.</param>
        /// <returns></returns>
        public static T ExecuteFilter<T>(String filterName, Object input)
        {
            // If this filter doesn't exist, return the input object with no filtering
            if (!m_filters.ContainsKey(filterName)) return (T)input;

            // Execute each filter on input object.
            for(int i = 0; i < m_filters[filterName].Count; i++)
            {
                input = m_filters[filterName][i].Invoke(input);
            }

            return (T)input;
        }

        /// <summary>
        /// Returns the amount of registered filters associated to a filter name.
        /// </summary>
        /// <param name="filterName">An unique string that represents the filter.</param>
        /// <returns></returns>
        public static Int32 RegisteredFiltersAmount(String filterName)
        {
            // If this filter doesn't exist, return 0
            if (!m_filters.ContainsKey(filterName)) return 0;

            return m_filters[filterName].Count;
        }

        /// <summary>
        /// Remove and unbound all registered filters.
        /// </summary>
        public static void ClearFilters()
        {
            m_filters.Clear();
        }
    }
}
