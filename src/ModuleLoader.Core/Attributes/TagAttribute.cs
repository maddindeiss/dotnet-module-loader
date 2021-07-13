using System;
using System.Linq;

namespace ModuleLoader.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TagAttribute: Attribute
    {
        public string[] Tags { get; }

        public TagAttribute(params string[] tags)
        {
            Tags = tags.Select(s => s.ToLowerInvariant()).ToArray();
        }
    }
}
