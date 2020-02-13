using System;
using System.Collections.Generic;
using System.Configuration;

namespace VirtualNodes
{
    /// <summary>
    ///     Loads rules for VirtualNodesUrlProvider
    /// </summary>
    public sealed class VirtualNodesRuleManager
    {
        #region Private Members

        /// <summary>
        ///     Lazy singleton instance member
        /// </summary>
        private static readonly Lazy<VirtualNodesRuleManager> _instance =
            new Lazy<VirtualNodesRuleManager>(() => new VirtualNodesRuleManager());

        #endregion

        #region Public Members

        /// <summary>
        ///     Gets the list of rules
        /// </summary>
        public List<string> Rules { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Returns a (singleton) VirtualNodesRuleManager instance
        /// </summary>
        public static VirtualNodesRuleManager Instance => _instance.Value;

        /// <summary>
        ///     Private constructor for Singleton
        /// </summary>
        private VirtualNodesRuleManager()
        {
            Rules = new List<string>();

            //Get all entries with keys starting with specified prefix
            var rules = ConfigurationManager.AppSettings.Get("VirtualNodes");

            if (string.IsNullOrEmpty(rules)) return;

            //Register a rule for each item
            foreach (var rule in rules.Split(',')) Rules.Add(rule.Trim());
        }

        #endregion
    }
}