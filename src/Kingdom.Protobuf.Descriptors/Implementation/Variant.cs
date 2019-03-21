using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    // TODO: TBD: will run with this for now, however, would it make better sense to model the concerns by collapsing "constant" into "variant" and/or vice versa?
    // TODO: TBD: from a modeling perspective it would leverage the same scaffold, notwithstanding the usage patterns...
    /// <summary>
    /// Ditto <see cref="IVariant"/>, along similar lines, this borrows much of the same scaffold
    /// pattern as <see cref="Constant"/>.
    /// </summary>
    /// <inheritdoc cref="IVariant" />
    public abstract class Variant : IVariant
    {
        /// <summary>
        /// Returns a new <see cref="Variant{T}"/> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Variant<T> Create<T>() => new Variant<T>();

        /// <summary>
        /// Returns a new <see cref="Variant{T}"/> instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Variant<T> Create<T>(T value) => new Variant<T>(value);

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
        protected Variant(object value)
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

        private static bool ProtoTypeEquals(ProtoType a, ProtoType b) => a == b;

        private static bool ElementTypeIdentifierPathEquals(IElementTypeIdentifierPath a, IElementTypeIdentifierPath b)
            => ElementTypeIdentifierPath.Equals((ElementTypeIdentifierPath) a, (ElementTypeIdentifierPath) b);

        /// <summary>
        /// 
        /// </summary>
        protected static IDictionary<Type, EqualsCallback> EqualsCallbacks
            = new Dictionary<Type, EqualsCallback>
            {
                {typeof(ProtoType), (a, b) => ProtoTypeEquals((ProtoType) a, (ProtoType) b)},
                {
                    typeof(IElementTypeIdentifierPath), (a, b) => ElementTypeIdentifierPathEquals(
                        (ElementTypeIdentifierPath) a, (ElementTypeIdentifierPath) b)
                },
                {
                    typeof(ElementTypeIdentifierPath), (a, b) => ElementTypeIdentifierPathEquals(
                        (ElementTypeIdentifierPath) a, (ElementTypeIdentifierPath) b)
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
        private static bool Equals(IVariant a, IVariant b)
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

            return Equals<ProtoType, ProtoType>()
                   || Equals<IElementTypeIdentifierPath, ElementTypeIdentifierPath>()
                   || Equals<ElementTypeIdentifierPath, ElementTypeIdentifierPath>()
                ;
        }

        /// <inheritdoc />
        public bool Equals(IVariant other) => Equals(this, other);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected delegate string RenderCallback(object x, IStringRenderingOptions options);

        private static string RenderProtoType(ProtoType x, IStringRenderingOptions o) => x.ToDescriptorString(o);

        // ReSharper disable once SuggestBaseTypeForParameter
        private static string RenderElementTypeIdentifierPath(ElementTypeIdentifierPath x, IStringRenderingOptions o) => x.ToDescriptorString(o);

        /// <summary>
        /// 
        /// </summary>
        protected IDictionary<Type, RenderCallback> RenderCallbacks { get; }
            = new Dictionary<Type, RenderCallback>
            {
                {typeof(ProtoType), (x, o) => RenderProtoType((ProtoType) x, o)},
                {
                    typeof(ElementTypeIdentifierPath), (x, o) => RenderElementTypeIdentifierPath(
                        (ElementTypeIdentifierPath) x, o)
                },
                {
                    typeof(IElementTypeIdentifierPath), (x, o) => RenderElementTypeIdentifierPath(
                        (ElementTypeIdentifierPath) x, o)
                }
            };

        // ReSharper disable once RedundantEmptyObjectOrCollectionInitializer
        /// <inheritdoc />
        public string ToDescriptorString() => ToDescriptorString(new StringRenderingOptions { });

        /// <inheritdoc />
        public string ToDescriptorString(IStringRenderingOptions options) => RenderCallbacks[Type].Invoke(Value, options);
    }

    /// <inheritdoc cref="IVariant{T}" />
    public class Variant<T> : Variant, IVariant<T>
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
        public Variant()
            : this(default(T))
        {
        }

        /// <summary>
        /// Public Constructor.
        /// </summary>
        /// <param name="value"></param>
        /// <inheritdoc />
        public Variant(T value)
            : base(value)
        {
            // Just make sure the Type squares.
            base.Type = _type;
        }
    }
}
