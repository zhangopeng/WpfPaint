using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SaveInfoExtensionAttribute : Attribute
    {
        public string Name { get; }

        public string Description { get; set; }

        public Type TargetType { get; }

        public SaveInfoExtensionAttribute(string name, Type targetType)
        {
            Name = name;
            TargetType = targetType;
        }
    }
}
