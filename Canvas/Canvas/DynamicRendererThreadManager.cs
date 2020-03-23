using System;
using System.ComponentModel;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Windows;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Threading;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;

namespace Canvas
{
    public class DynamicRendererThreadManager : IDisposable
    {
        public void StartUp()
        {
            this._startupCompleted = new AutoResetEvent(false);
            Thread thread = new Thread(new ThreadStart(this.InkingThreadProc));
            thread.Priority = ThreadPriority.Highest;
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
            this.WaitForStartupCompleted();
            this._mainDispatcher = Dispatcher.CurrentDispatcher;
            this.MainMediaContext = DynamicRendererThreadManager.MediaContextReflection.From(this._mainDispatcher);
            this._startupCompleted.Close();
            this._startupCompleted = null;
        }

        public void Close()
        {
            this.InkingDispatcher.InvokeShutdown();
        }

        public Dispatcher InkingDispatcher { get; private set; }

        public event EventHandler InkingMediaContextRenderComplete
        {
            add
            {
                this.InkingDispatcher.VerifyAccess();
                DynamicRendererThreadManager.MediaContextReflection.AddRenderComplete(this.InkingMediaContext, value);
            }
            remove
            {
                this.InkingDispatcher.VerifyAccess();
                DynamicRendererThreadManager.MediaContextReflection.RemoveRenderComplete(this.InkingMediaContext, value);
            }
        }

        public event EventHandler MainMediaContextRenderComplete
        {
            add
            {
                this._mainDispatcher.VerifyAccess();
                DynamicRendererThreadManager.MediaContextReflection.AddRenderComplete(this.MainMediaContext, value);
            }
            remove
            {
                this._mainDispatcher.VerifyAccess();
                DynamicRendererThreadManager.MediaContextReflection.RemoveRenderComplete(this.MainMediaContext, value);
            }
        }

        private object InkingMediaContext { get; set; }

        private object MainMediaContext { get; set; }

        private void WaitForStartupCompleted()
        {
            this._startupCompleted.WaitOne();
        }

        [SecurityCritical]
        private void InkingThreadProc()
        {
            Thread.CurrentThread.Name = "DynamicRenderer";
            this.InkingDispatcher = Dispatcher.CurrentDispatcher;
            this.InkingMediaContext = DynamicRendererThreadManager.MediaContextReflection.From(this.InkingDispatcher);
            this.InkingDispatcher.UnhandledException += this.InkingDispatcher_UnhandledException;
            this._startupCompleted.Set();
            Dispatcher.Run();
        }

        private void InkingDispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }

        public void Dispose()
        {
            this.Close();
        }

        private AutoResetEvent _startupCompleted;

        private Dispatcher _mainDispatcher;

        private class MediaContextReflection
        {
            static MediaContextReflection()
            {
                DynamicRendererThreadManager.MediaContextReflection._removeRenderComplete = DynamicRendererThreadManager.MediaContextReflection.RenderComplete.GetRemoveMethod(true);
            }

            private static Type MediaContext { get; } = Assembly.GetAssembly(typeof(StylusPlugIn)).GetType("System.Windows.Media.MediaContext");

            private static EventInfo RenderComplete { get; } = MediaContext.GetEvent("RenderComplete", BindingFlags.Instance | BindingFlags.NonPublic);

            public static void AddRenderComplete(object mediaContext, EventHandler onRenderComplete)
            {
                DynamicRendererThreadManager.MediaContextReflection._addRenderComplete.Invoke(mediaContext, new object[]
                {
                   onRenderComplete
                });
            }

            public static void RemoveRenderComplete(object mediaContext, EventHandler onRenderComplete)
            {
                do
                {
                    if (true && !false)
                    {
                        DynamicRendererThreadManager.MediaContextReflection._removeRenderComplete.Invoke(mediaContext, new object[]
                        {
                            onRenderComplete
                        });
                    }
                }
                while (false || false);
            }

            public static object From(Dispatcher dispatcher)
            {
                return DynamicRendererThreadManager.MediaContextReflection._from.Invoke(null, new object[]
                {
                    dispatcher
                });
            }

            private static readonly MethodInfo _from = DynamicRendererThreadManager.MediaContextReflection.Reflector.GetMethod(MediaContext, "From",new Type[] {typeof(Dispatcher)}, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            private static readonly MethodInfo _removeRenderComplete;

            private static readonly MethodInfo _addRenderComplete = DynamicRendererThreadManager.MediaContextReflection.RenderComplete.GetAddMethod(true);
            private class Reflector : Binder
            {
                public override FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, CultureInfo culture)
                {
                    if (match == null)
                    {
                        throw new ArgumentNullException("match");
                    }
                    foreach (var mat in match)
                    {
                        if (this.ChangeType(value, mat.FieldType, culture) != null)
                        {
                            return mat;
                        }
                    }
                    return null;
                }

                public override MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] match, ref object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] names, out object state)
                {
                    BinderState binderState = new BinderState();
                    object[] array = new object[args.Length];
                    args.CopyTo(array, 0);
                    binderState.args = array;
                    state = binderState;
                    if (match == null)
                    {
                        throw new ArgumentNullException();
                    }
                    int length = 0;
                    while (length < match.Length)
                    {
                        int plength = 0;
                        ParameterInfo[] parameters = match[length].GetParameters();
                        if (args.Length == parameters.Length)
                        {
                            int alength = 0;
                            while (plength < args.Length)
                            {
                                if (names != null)
                                {
                                    if (names.Length != args.Length)
                                    {
                                        throw new ArgumentException("names and args must have the same number of elements.");//4 p,8 a,12 n
                                    }
                                    var nlength = 0;
                                    while (nlength < names.Length)
                                    {
                                        if (string.Compare(parameters[alength].Name, names[nlength].ToString()) == 0)
                                        {
                                            args[alength] = binderState.args[nlength];
                                        }
                                        nlength++;
                                    }
                                }
                                if (this.ChangeType(args[alength], parameters[alength].ParameterType, culture) == null)
                                {
                                    break;
                                }
                                plength++;
                                alength++;
                            }
                            if (plength == args.Length)
                            {
                                return match[length];
                            }
                        }
                    }
                    return null;
                }

                private bool CanChangeType(Type inputType, Type outputType)
                {
                    Contract.Requires(outputType != null);
                    if (inputType == null)
                    {
                        return !outputType.IsValueType;
                    }
                    return CanConvertFrom(inputType, outputType);
                }

                public override object ChangeType(object value, Type myChangeType, CultureInfo culture)
                {
                    if (this.CanConvertFrom(value.GetType(), myChangeType))
                    {
                        return Convert.ChangeType(value, myChangeType);
                    }
                    return null;
                }

                public override void ReorderArgumentArray(ref object[] args, object state)
                {
                    ((DynamicRendererThreadManager.MediaContextReflection.Reflector.BinderState)state).args.CopyTo(args, 0);
                }

                public override MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers)
                {
                    if (match == null)
                    {
                        throw new ArgumentNullException("match");
                    }
                    int length = 0;
                    while (length < match.Length)
                    {
                        int plength = 0;
                        ParameterInfo[] parameters = match[length].GetParameters();
                        int num = types.Length;
                        if (num == parameters.Length)//0 l,4 p,8 t
                        {
                            int tlength = 0;
                            while (num < types.Length)
                            {
                                if (this.CanConvertFrom(types[tlength], parameters[tlength].ParameterType))
                                {
                                    plength++;
                                }
                                tlength++;
                            }
                            if (plength == types.Length)
                            {
                                return match[length];
                            }
                        }
                    }
                    return null;
                }

                public override PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers)
                {
                    if (match == null)
                    {
                        throw new ArgumentNullException("match");
                    }
                    int length = 0;
                    while (length < match.Length)
                    {
                        int plength = 0;
                        ParameterInfo[] indexParameters = match[length].GetIndexParameters();//0 l,4 p,8 t
                        if (indexes.Length == indexParameters.Length)
                        {
                            var tlength = 0;
                            while (tlength < indexes.Length && this.CanConvertFrom(indexes[tlength], indexParameters[tlength].ParameterType))
                            {
                                plength++;
                                tlength++;
                            }
                            if (plength == indexes.Length && this.CanConvertFrom(returnType, match[length].PropertyType))
                            {
                                return match[length];
                            }
                        }
                        length++;
                    }
                    return null;
                }

                private bool CanConvertFrom(Type type1, Type type2)
                {
                    if (type1.IsPrimitive && type2.IsPrimitive)
                    {
                        TypeCode typeCode = Type.GetTypeCode(type1);
                        TypeCode typeCode2 = Type.GetTypeCode(type2);
                        if (typeCode == typeCode2)
                        {
                            return true;
                        }
                        if (typeCode != TypeCode.Char)
                        {
                            if (typeCode == TypeCode.Byte)
                            {
                                switch (typeCode2)
                                {
                                    case TypeCode.Char:
                                        return true;
                                    case TypeCode.Int16:
                                        return true;
                                    case TypeCode.UInt16:
                                        return true;
                                    case TypeCode.Int32:
                                        return true;
                                    case TypeCode.UInt32:
                                        return true;
                                    case TypeCode.Int64:
                                        return true;
                                    case TypeCode.UInt64:
                                        return true;
                                    case TypeCode.Single:
                                        return true;
                                    case TypeCode.Double:
                                        return true;
                                }
                                return false;
                            }
                            if (typeCode == TypeCode.SByte)
                            {
                                switch (typeCode2)
                                {
                                    case TypeCode.Int16:
                                        return true;
                                    case TypeCode.Int32:
                                        return true;
                                    case TypeCode.Int64:
                                        return true;
                                    case TypeCode.Single:
                                        return true;
                                    case TypeCode.Double:
                                        return true;
                                }
                                return false;
                            }
                            if (typeCode != TypeCode.UInt16)
                            {
                                while (typeCode != TypeCode.Int16)
                                {
                                    if (typeCode == TypeCode.UInt32)
                                    {
                                        switch (typeCode2)
                                        {
                                            case TypeCode.Int64:
                                                return true;
                                            case TypeCode.UInt64:
                                                return true;
                                            case TypeCode.Single:
                                                return true;
                                            case TypeCode.Double:
                                                return true;
                                            default:
                                                return false;
                                        }
                                    }
                                    else
                                    {
                                        if (typeCode == TypeCode.Int32)
                                        {
                                            switch (typeCode2)
                                            {
                                                case TypeCode.Int64:
                                                    return true;
                                                case TypeCode.Single:
                                                    return true;
                                                case TypeCode.Double:
                                                    return true;
                                            }
                                            return false;
                                        }
                                        if (typeCode == TypeCode.UInt64)
                                        {
                                            return typeCode2 == TypeCode.Single || typeCode2 == TypeCode.Double;
                                        }
                                        if (!false)
                                        {
                                            if (typeCode == TypeCode.Int64)
                                            {
                                                if (typeCode2 == TypeCode.Single)
                                                {
                                                    return true;
                                                }
                                                if (typeCode2 == TypeCode.Double)
                                                {
                                                    return true;
                                                }
                                                return false;
                                            }
                                            else
                                            {
                                                if (typeCode != TypeCode.Single)
                                                {
                                                    return false;
                                                }
                                                if (typeCode2 == TypeCode.Double)
                                                {
                                                    int result;
                                                    int num = result = 1;
                                                    if (num != 0)
                                                    {
                                                        return num != 0;
                                                    }
                                                    return result != 0;
                                                }
                                                else
                                                {
                                                    int result2;
                                                    int num2 = result2 = 0;
                                                    if (num2 == 0)
                                                    {
                                                        return num2 != 0;
                                                    }
                                                    return result2 != 0;
                                                }
                                            }
                                        }
                                    }
                                }
                                switch (typeCode2)
                                {
                                    case TypeCode.Int32:
                                        return true;
                                    case TypeCode.UInt32:
                                    case TypeCode.UInt64:
                                        return false;
                                    case TypeCode.Int64:
                                        return true;
                                    case TypeCode.Single:
                                        return true;
                                    case TypeCode.Double:
                                        break;
                                    default:
                                        return false;
                                }
                                return true;
                            }
                            switch (typeCode2)
                            {
                                case TypeCode.Int32:
                                    return true;
                                case TypeCode.UInt32:
                                    return true;
                                case TypeCode.Int64:
                                    return true;
                                case TypeCode.UInt64:
                                    return true;
                                case TypeCode.Single:
                                    return true;
                                case TypeCode.Double:
                                    return true;
                            }
                            return false;
                        }
                        switch (typeCode2)
                        {
                            case TypeCode.UInt16:
                                return true;
                            case TypeCode.Int32:
                                return true;
                            case TypeCode.UInt32:
                                return true;
                            case TypeCode.Int64:
                                return true;
                            case TypeCode.UInt64:
                                return true;
                            case TypeCode.Single:
                                break;
                            case TypeCode.Double:
                                return true;
                            default:
                                return false;
                        }
                        return true;
                    }
                    return false;
                }

                public static MethodInfo GetMethod(Type type, string name, Type[] types, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                {
                    var nameMethods = type.GetMethods(bindingAttr).Where(mi =>
                    {
                        if (mi.Name == name)
                        {
                            var pars = mi.GetParameters();
                            return types.All((temp) =>
                            {
                                return pars.Any((pm) => pm.ParameterType == temp);
                            });
                        }
                        return false;
                    });
                    return nameMethods.FirstOrDefault();
                }

                private class BinderState
                {
                    public object[] args;
                }
            }
        }

    }

}
