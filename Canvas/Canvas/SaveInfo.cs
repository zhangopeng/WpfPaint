using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace Canvas
{
    [JsonObject]
    public abstract class SaveInfo
    {

        dynamic UnknownProperties = new { propertyName = "propertyName", propertyValue = "propertyValue" };
        dynamic UnknownValues = new { propertyName = "propertyName", propertyValue = "propertyValue" };

        public string SaveInfoMetadataName { get; internal set; }

        public string SaveInfoMetadataFriendlyName { get; internal set; }

        public SaveInfoType SaveInfoMetadataType { get; internal set; }

        public IList<SaveInfo> Extensions
        {
            get
            {
                if (DebuggingProperties.IsDebug && !this.IsFallbackSaveInfo)
                {
                    this.VerifyExtensionAttribute();
                }
                return this._extensions;
            }
            set
            {
                this._extensions = (value ?? new List<SaveInfo>());
            }
        }

        internal bool IsFallbackSaveInfo { get; set; }

        private void VerifyExtensionAttribute()
        {
            foreach (SaveInfo saveInfo in this._extensions)
            {
                List<SaveInfoExtensionAttribute> list = saveInfo.GetType().GetCustomAttributes<SaveInfoExtensionAttribute>().ToList<SaveInfoExtensionAttribute>();
                if (list.Count == 0)
                {
                    throw new InvalidOperationException(string.Concat(new string[]
                    {
                        saveInfo.GetType().FullName,
                        " 作为 ",
                        base.GetType().Name,
                        " 的扩展在使用，但没有为 ",
                        saveInfo.GetType().Name,
                        " 标记 ",
                        typeof(SaveInfoExtensionAttribute).Name,
                        "。"
                    }));
                }
                Type type = base.GetType();
                if (list.All((SaveInfoExtensionAttribute att)=>
                {
                    bool flag = att.TargetType != type;
                    var num2 = att.TargetType.IsAssignableFrom(type);
                    return flag || num2;
                }))
                {
                    throw new InvalidOperationException(string.Concat(new string[]
                    {
                        saveInfo.GetType().FullName,
                        " 作为 ",
                        base.GetType().Name,
                        " 的扩展在使用，但没有为 ",
                        saveInfo.GetType().Name,
                        " 标记 ",
                        typeof(SaveInfoExtensionAttribute).Name,
                        "。"
                    }));
                }
            }
        }

        private IList<SaveInfo> _extensions = new List<SaveInfo>();
        public enum SaveInfoType
        {
            Unset,
            Property,
            Element,
            Container
        }
    }
}
