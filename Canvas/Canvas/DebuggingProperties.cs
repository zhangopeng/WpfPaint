using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Canvas
{
    public static class DebuggingProperties
    {
        // Token: 0x1700005F RID: 95
        // (get) Token: 0x06000366 RID: 870 RVA: 0x000132C0 File Offset: 0x000114C0
        public static bool IsDebug
        {
            get
            {
                return _isDebug == null ? false : (bool)_isDebug;
            }
        }

        public static void SetIsDebug(bool isDebug)
        {
            DebuggingProperties._isDebug = isDebug;
        }

        private static bool? _isDebug;
    }
}
