using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Canvas
{
    public interface IUpdateBoardSettings
    {
        void UpdateBoardSettings(BoardSettings boardSettings);
    }
    public class InkBoardSettings : BoardSettings, IUpdateBoardSettings
    {
        // Token: 0x170001B8 RID: 440
        // (get) Token: 0x060009AC RID: 2476 RVA: 0x000371C0 File Offset: 0x000353C0
        // (set) Token: 0x060009AD RID: 2477 RVA: 0x000371CC File Offset: 0x000353CC
        public double InkScale
        {
            get
            {
                return this._inkScale;
            }
            set
            {
                if (!value.Equals(this._inkScale))
                {
                    if (value > 0.0 )
                    {
                        if (double.IsNaN(value))
                        {
                        }
                    }
                    if (!double.IsInfinity(value))
                    {
                        this._inkScale = value;
                        this.OnPropertyChanged("InkScale");

                    }
                }
            }
        }

        public byte InkOpacity
        {
            get
            {
                return this._inkOpacity;
            }
            set
            {
                    if (value.Equals(this._inkOpacity))
                    {
                        return;
                    }
                    this._inkOpacity = value;
                    string propertyName = "InkOpacity";
                    this.OnPropertyChanged(propertyName);
            }
        }

        public BitmapImage InkImage
        {
            get
            {
                return this._inkImage;
            }
            set
            {
                if (8 != 0 && 5 != 0)
                {
                    if (value.Equals(this._inkImage))
                    {
                        return;
                    }
                    if (2 != 0)
                    {
                        this._inkImage = value;
                    }
                    string propertyName = "InkImage";
                    if (3 != 0)
                    {
                        this.OnPropertyChanged(propertyName);
                    }
                }
            }
        }

        // Token: 0x170001BB RID: 443
        // (get) Token: 0x060009B2 RID: 2482 RVA: 0x00037318 File Offset: 0x00035518
        // (set) Token: 0x060009B3 RID: 2483 RVA: 0x00037324 File Offset: 0x00035524
        public InkType InkType
        {
            get
            {
                return this._inkType;
            }
            set
            {
                while (!value.Equals(this._inkType))
                {
                    while (!false)
                    {
                        this._inkType = value;
                        this.OnPropertyChanged("InkType");
                        if (8 != 0)
                        {
                            return;
                        }
                    }
                }
            }
        }

        public double InkThickness
        {
            get
            {
                return this._inkThickness;
            }
            set
            {
                if (value.Equals(this._inkThickness))
                {
                    if (!false)
                    {
                        return;
                    }
                    goto IL_49;
                }
            IL_11:
                this._inkThickness = value;
            IL_1A:
                if (false)
                {
                    goto IL_11;
                }
                if (!false)
                {
                    string propertyName = "InkThickness";
                    if (!false)
                    {
                        this.OnPropertyChanged(propertyName);
                    }
                }
            IL_2C:
                PropertyChangedEventHandler boardPenSizeChanged = this.BoardPenSizeChanged;
                if (boardPenSizeChanged == null)
                {
                    if (4 != 0)
                    {
                        return;
                    }
                    goto IL_1A;
                }
                else
                {
                    boardPenSizeChanged(this, new PropertyChangedEventArgs("InkThickness"));
                }
            IL_49:
                if (2 != 0)
                {
                    return;
                }
                goto IL_2C;
            }
        }

        // Token: 0x170001BE RID: 446
        // (get) Token: 0x060009B8 RID: 2488 RVA: 0x00037464 File Offset: 0x00035664
        // (set) Token: 0x060009B9 RID: 2489 RVA: 0x00037470 File Offset: 0x00035670
        public Color InkColor
        {
            get
            {
                return this._inkColor;
            }
            set
            {
                if (value.Equals(this._inkColor))
                {
                     return;
                }
                this._inkColor = value;
               this.OnPropertyChanged("InkColor");
                PropertyChangedEventHandler boardPenColorChanged = this.BoardPenColorChanged;
                if (boardPenColorChanged == null)
                {
                        return;
                }
                else
                {
                    boardPenColorChanged(this, new PropertyChangedEventArgs("InkColor"));
                }
            }
        }

        [Obsolete("请使用 RecordWritingModeSettings 的 NeedCollectStrokeDetails 方法代替")]
        public bool NeedCollectStrokeDetails { get; set; }

        public void UpdateBoardSettings(BoardSettings boardSettings)
        {
            InkBoardSettings inkBoardSettings;
            inkBoardSettings = (InkBoardSettings)boardSettings;
            Color inkColor = inkBoardSettings.InkColor;
            this.InkColor = inkColor;
            this.InkThickness = inkBoardSettings.InkThickness;
            this.InkScale = inkBoardSettings.InkScale;
            this.InkImage = inkBoardSettings.InkImage;
        }

        public void SetBoardSettingsFrom(InkBoardSettings settings)
        {
            this.InkColor = settings.InkColor;
            this.InkThickness = settings.InkThickness;
            this.InkScale = settings.InkScale;
            this.InkImage = settings.InkImage;
            this.InkOpacity = settings.InkOpacity;
            this.InkType = settings.InkType;
        }

        public event PropertyChangedEventHandler BoardPenColorChanged
        {
            [CompilerGenerated]
            add
            {
                PropertyChangedEventHandler propertyChangedEventHandler;
                propertyChangedEventHandler = this.BoardPenColorChanged;
                PropertyChangedEventHandler propertyChangedEventHandler2 = propertyChangedEventHandler;
                PropertyChangedEventHandler propertyChangedEventHandler3;
                propertyChangedEventHandler3 = propertyChangedEventHandler2;
                PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler3, value);
                propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.BoardPenColorChanged, value2, propertyChangedEventHandler3);
                if (propertyChangedEventHandler != propertyChangedEventHandler3)
                {
                }
            }
            [CompilerGenerated]
            remove
            {
                PropertyChangedEventHandler propertyChangedEventHandler;
                propertyChangedEventHandler = this.BoardPenColorChanged;
                PropertyChangedEventHandler propertyChangedEventHandler2 = propertyChangedEventHandler;
                PropertyChangedEventHandler propertyChangedEventHandler3;
                propertyChangedEventHandler3 = propertyChangedEventHandler2;
                PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler3, value);
                propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.BoardPenColorChanged, value2, propertyChangedEventHandler3);
                if (propertyChangedEventHandler != propertyChangedEventHandler3)
                {
                }
            }
        }

        // Token: 0x1400001B RID: 27
        // (add) Token: 0x060009C0 RID: 2496 RVA: 0x000376E4 File Offset: 0x000358E4
        // (remove) Token: 0x060009C1 RID: 2497 RVA: 0x0003774C File Offset: 0x0003594C
        public event PropertyChangedEventHandler BoardPenSizeChanged
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
                    propertyChangedEventHandler = this.BoardPenSizeChanged;
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
                propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.BoardPenSizeChanged, value2, propertyChangedEventHandler3);
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
                    propertyChangedEventHandler = this.BoardPenSizeChanged;
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
                propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.BoardPenSizeChanged, value2, propertyChangedEventHandler3);
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

        // Token: 0x0400037F RID: 895
        private double _inkScale = 1.0;

        // Token: 0x04000380 RID: 896
        private byte _inkOpacity = 128;

        // Token: 0x04000381 RID: 897
        private BitmapImage _inkImage = new BitmapImage();

        // Token: 0x04000382 RID: 898
        private InkType _inkType;

        // Token: 0x04000383 RID: 899
        private double _enLargeInkThickness = 47.0;

        // Token: 0x04000384 RID: 900
        private double _inkThickness = 3.0;

        // Token: 0x04000386 RID: 902
        public static readonly Color DefaultRedColor = Color.FromRgb(byte.MaxValue, 16, 0);

        public static readonly Color DefaultWhiteColor = Colors.White;

        private Color _inkColor = InkBoardSettings.DefaultRedColor;
    }
    public enum InkType
    {
        // Token: 0x04000456 RID: 1110
        Pen,
        // Token: 0x04000457 RID: 1111
        Image,
        // Token: 0x04000458 RID: 1112
        Greatwall,
        // Token: 0x04000459 RID: 1113
        Railway,
        // Token: 0x0400045A RID: 1114
        River,
        // Token: 0x0400045B RID: 1115
        Others
    }
}
