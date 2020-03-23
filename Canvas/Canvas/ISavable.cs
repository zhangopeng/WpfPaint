using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas
{
    public interface ISavable<T> : ISavable where T : SaveInfo
    {
        // Token: 0x06000192 RID: 402
        T GetSaveInfo();

        // Token: 0x06000193 RID: 403
        void SetSaveInfo(T saveInfo);
    }
    public interface ISavable
    {
    }
}
