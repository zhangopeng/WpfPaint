using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Canvas
{
    public abstract class BoardSettings : INotifyPropertyChanged
    {
        // Token: 0x1700026E RID: 622
        // (get) Token: 0x06000DB2 RID: 3506 RVA: 0x0003733C File Offset: 0x0003553C
        // (set) Token: 0x06000DB3 RID: 3507 RVA: 0x00037348 File Offset: 0x00035548
        public Board Board { get; set; }

        // Token: 0x14000087 RID: 135
        // (add) Token: 0x06000DB4 RID: 3508 RVA: 0x0003735C File Offset: 0x0003555C
        // (remove) Token: 0x06000DB5 RID: 3509 RVA: 0x000373C4 File Offset: 0x000355C4
        public event PropertyChangedEventHandler PropertyChanged
        {
            [CompilerGenerated]
            add
            {
                if (false)
                {
                    goto IL_38;
                }
                PropertyChangedEventHandler propertyChangedEventHandler;
                if (!false)
                {
                    propertyChangedEventHandler = this.PropertyChanged;
                }
            IL_0F:
                if (7 == 0)
                {
                    goto IL_34;
                }
                PropertyChangedEventHandler propertyChangedEventHandler2 = propertyChangedEventHandler;
                PropertyChangedEventHandler propertyChangedEventHandler3;
                if (6 != 0)
                {
                    propertyChangedEventHandler3 = propertyChangedEventHandler2;
                }
            IL_18:
                PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler3, value);
                propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, value2, propertyChangedEventHandler3);
            IL_34:
                if (propertyChangedEventHandler != propertyChangedEventHandler3)
                {
                    goto IL_0F;
                }
            }
            [CompilerGenerated]
            remove
            {
                if (false)
                {
                    goto IL_38;
                }
                PropertyChangedEventHandler propertyChangedEventHandler;
                if (!false)
                {
                    propertyChangedEventHandler = this.PropertyChanged;
                }
            IL_0F:
                if (7 == 0)
                {
                    goto IL_34;
                }
                PropertyChangedEventHandler propertyChangedEventHandler2 = propertyChangedEventHandler;
                PropertyChangedEventHandler propertyChangedEventHandler3;
                if (6 != 0)
                {
                    propertyChangedEventHandler3 = propertyChangedEventHandler2;
                }
            IL_18:
                PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler3, value);
                propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, value2, propertyChangedEventHandler3);
            IL_34:
                if (propertyChangedEventHandler != propertyChangedEventHandler3)
                {
                    goto IL_0F;
                }
            IL_38:
                if (!false)
                {
                    return;
                }
                goto IL_18;
            }
        }

        // Token: 0x06000DB6 RID: 3510 RVA: 0x0003742C File Offset: 0x0003562C
        public new BoardSettings MemberwiseClone()
        {
            return (BoardSettings)base.MemberwiseClone();
        }

        // Token: 0x06000DB7 RID: 3511 RVA: 0x00037444 File Offset: 0x00035644
        [NotifyPropertyChangedInvocator]
        protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(field, value))
            {
                return false;
            }
            field = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        // Token: 0x06000DB8 RID: 3512 RVA: 0x0003749C File Offset: 0x0003569C
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
              
        }
    }
}
