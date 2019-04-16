using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    using static Characters;

    // TODO: TBD: see notes concerning Variant, since the scaffold is so similar, I do not think it makes sense to pursue any sort of code generation here...
    // TODO: TBD: but rather "collapse" the supported types under the banner of "variant", potentially
    // TODO: TBD: notwithstanding potential for usage ambiguities... i.e. "constant" has a specific meaning, whereas "variant" just occurs as a natural alternative
    /// <inheritdoc cref="IConstant" />
    public abstract class Constant : IConstant
    {
        /// <summary>
        /// Returns a new <see cref="Constant{T}"/> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Constant<T> Create<T>() => new Constant<T>();

        /// <summary>
        /// Returns a new <see cref="Constant{T}"/> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Constant<T> Create<T>(T value) => new Constant<T>(value);

        // TODO: TBD: consider valid Protocol Buffer C# type mapping...
        /// <inheritdoc />
        public virtual Type Type { get; protected set; }

        /// <inheritdoc />
        public object Value { get; protected set; }

        /// <summary>
        /// Gets the protected <see cref="Value"/>.
        /// </summary>
        protected object ProtectedValue
        {
            get => Value;
            set
            {
                Value = value;
                // TODO: TBD: make sure this is Null-safe.
                // TODO: TBD: this may also extend through the other Type specific bits...
                Type = value?.GetType();
            }
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="value"></param>
        protected Constant(object value)
        {
            ProtectedValue = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected delegate bool EqualsCallback(object a, object b);

        private static bool BooleanEquals(bool a, bool b) => a == b;
        private static bool LongEquals(long a, long b) => a == b;
        private static bool UnsignedLongEquals(ulong a, ulong b) => a == b;

        private static bool DoubleEquals(double a, double b)
        {
            if (double.IsNaN(a)
                || double.IsNaN(b)
                || double.IsInfinity(a)
                || double.IsInfinity(b))
            {
                return false;
            }

            return (a - b).IsZero();
        }

        private static bool FloatEquals(float a, float b) => (a - b).IsZero();
        private static bool StringEquals(string a, string b) => a == b;

        private static bool IdentifierPathEquals(IIdentifierPath a, IIdentifierPath b)
            => IdentifierPath.Equals((IdentifierPath) a, (IdentifierPath) b);

        private static bool BytesEquals(IEnumerable<byte> a, IEnumerable<byte> b)
            // ReSharper disable once PossibleMultipleEnumeration
            => a.Count()
               // ReSharper disable once PossibleMultipleEnumeration
               == b.Count()
               // ReSharper disable once PossibleMultipleEnumeration
               && a.Zip(
                   // ReSharper disable once PossibleMultipleEnumeration
                   b, (x, y) => x == y).All(x => x);

        /// <summary>
        /// 
        /// </summary>
        protected static IDictionary<Type, EqualsCallback> EqualsCallbacks
            = new Dictionary<Type, EqualsCallback>
            {
                {typeof(bool), (a, b) => BooleanEquals((bool) a, (bool) b)},
                {typeof(long), (a, b) => LongEquals((long) a, (long) b)},
                {typeof(ulong), (a, b) => UnsignedLongEquals((ulong) a, (ulong) b)},
                {typeof(float), (a, b) => FloatEquals((float) a, (float) b)},
                {typeof(double), (a, b) => DoubleEquals((double) a, (double) b)},
                {typeof(string), (a, b) => StringEquals((string) a, (string) b)},
                {typeof(byte[]), (a, b) => BytesEquals((byte[]) a, (byte[]) b)},
                {
                    typeof(IIdentifierPath),
                    (a, b) => IdentifierPathEquals((IIdentifierPath) a, (IIdentifierPath) b)
                },
                {
                    typeof(IdentifierPath),
                    (a, b) => IdentifierPathEquals((IIdentifierPath) a, (IIdentifierPath) b)
                }
            };

        /// <summary>
        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>. Here we bypass
        /// <see cref="Type"/> for the time being and inspect the direct <see cref="object"/>
        /// type. We perform the comparison in the best possible combination of Allowable and
        /// Desired types as possible.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool Equals(IConstant a, IConstant b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // We bypass Type for the time being.
            var x = a.Value;
            var y = b.Value;

            bool Equals<TAllowable, TDesired>()
            {
                return (x is TAllowable || x is TDesired)
                       && (y is TAllowable || y is TDesired)
                       && EqualsCallbacks[typeof(TDesired)].Invoke(x, y);
            }

            return Equals<int, long>()
                   || Equals<uint, ulong>()
                   || Equals<bool, bool>()
                   || Equals<float, float>()
                   || Equals<double, double>()
                   || Equals<string, string>()
                   || Equals<byte[], byte[]>()
                   || Equals<IIdentifierPath, IIdentifierPath>()
                   || Equals<IdentifierPath, IIdentifierPath>()
                ;
        }

        /// <inheritdoc />
        public bool Equals(IConstant other) => Equals(this, other);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected delegate string RenderCallback(object x, IStringRenderingOptions options);

        private static string RenderBoolean(bool x, IStringRenderingOptions _) => $"{x}".ToLower();
        // ReSharper disable once SuggestBaseTypeForParameter
        private static string RenderLong(long x, IStringRenderingOptions o) => x.RenderLong(o.IntegerRendering);
        private static string RenderUnsignedLong(ulong x, IStringRenderingOptions o) => RenderLong((long) x, o);

        // ReSharper disable once SuggestBaseTypeForParameter
        private static string RenderDouble(double x, IStringRenderingOptions options)
        {
            if (double.IsPositiveInfinity(x))
            {
                return "inf";
            }

            if (double.IsNegativeInfinity(x))
            {
                return "-inf";
            }

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (double.IsNaN(x))
            {
                return "nan";
            }

            return x.RenderDouble(options.FloatingPointRendering);
        }

        // TODO: TBD: escape the string...
        private static string RenderString(string s, IStringRenderingOptions _) => $"{OpenQuote}{s}{CloseQuote}";
        // TODO: TBD: circle around on this one... how ought this one to be rendered?
        private static string RenderBytes(IEnumerable<byte> _, IStringRenderingOptions __)
            => throw new NotImplementedException();
        // ReSharper disable once SuggestBaseTypeForParameter
        // TODO: TBD: we see this in terms of the interface today, but ostensibly we should also support the concrete class...
        private static string RenderIdentifierPath(IIdentifierPath x, IStringRenderingOptions o)
            => x.ToDescriptorString(o);

        // TODO: TBD: convey the rendering options...
        // TODO: TBD: I'm not sure there is any way to tell that the Constant value is signed or unsigned
        // TODO: TBD: additionally, the grammar only specified "integer", which we are taking as "long" in this case (i.e. 64-bit integer).
        // TODO: TBD: in other words, not to confuse the Constant literal values with the Protocol Buffer "types" ...
        /// <summary>
        /// 
        /// </summary>
        protected IDictionary<Type, RenderCallback> RenderCallbacks { get; }
            = new Dictionary<Type, RenderCallback>
            {
                {typeof(bool), (x, o) => RenderBoolean((bool) x, o)},
                {typeof(int), (x, o) => RenderLong((long) x, o)},
                {typeof(long), (x, o) => RenderLong((long) x, o)},
                {typeof(uint), (x, o) => RenderUnsignedLong((ulong) x, o)},
                {typeof(ulong), (x, o) => RenderUnsignedLong((ulong) x, o)},
                {typeof(double), (x, o) => RenderDouble((double) x, o)},
                {typeof(float), (x, o) => RenderDouble((double) x, o)},
                {typeof(string), (s, o) => RenderString((string) s, o)},
                {typeof(byte[]), (x, o) => RenderBytes((byte[]) x, o)},
                {
                    typeof(IdentifierPath),
                    (x, o) => RenderIdentifierPath((IIdentifierPath) x, o)
                },
                {
                    typeof(IIdentifierPath),
                    (x, o) => RenderIdentifierPath((IIdentifierPath) x, o)
                },
            };

        /// <inheritdoc />
        public string ToDescriptorString() => ToDescriptorString(new StringRenderingOptions { });

        /// <inheritdoc />
        public string ToDescriptorString(IStringRenderingOptions options) => RenderCallbacks[Type].Invoke(Value, options);
    }

    /// <inheritdoc cref="IConstant{T}" />
    public class Constant<T> : Constant, IConstant<T>
    {
        /// <summary>
        /// <see cref="Type"/> backing field.
        /// </summary>
        private readonly Type _type = typeof(T);

        /// <inheritdoc />
        /// <typeparamref name="T"/>
        public override Type Type
        {
            get => _type;
            protected set
            {
                // There is nothing to set in this case since we already know the Type.
            }
        }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        /// <inheritdoc />
        public new T Value
        {
            get => (T) ProtectedValue;
            set
            {
                ProtectedValue = value;
                base.Type = typeof(T);
            }
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <inheritdoc />
        public Constant()
            : this(default(T))
        {
        }

        /// <summary>
        /// Public Constructor.
        /// </summary>
        /// <param name="value"></param>
        /// <inheritdoc />
        public Constant(T value)
            : base(value)
        {
            // Just make sure the Type squares.
            base.Type = _type;
        }
    }
}
