using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhythm.SchemaOrg
{
    public abstract class BaseJunction
    {
        protected BaseJunction(object value)
        {
            Value = value;
        }

        protected abstract Type[] _allowedTypes { get; }

        protected bool AllowsNull => _allowedTypes.Any(t => t.IsClass || t.IsInterface);

        protected bool AllowsType(Type type) {
            return _allowedTypes.Any(t => t.IsAssignableFrom(type));
        }

        protected object _value;
        public object Value
        {
            get { return _value; }
            set {
                if (value == null)
                {
                    if (!AllowsNull)
                        throw new Exception($"The junction type {this.GetType().Name} cannot be assigned a null value.");
                }
                else
                {
                    var valueType = value.GetType();
                    if (!AllowsType(valueType))
                        throw new Exception($"The junction type {this.GetType().Name} cannot be assigned a value of type {valueType.Name}.");
                }
                _value = value;
            }
        }

        public Type ValueType => _value == null ? null : _value.GetType();
    }

    public class Junction<A, B> : BaseJunction
    {
        protected Junction(object value) : base(value) { }

        public static readonly Type[] AllowedTypes = new Type[] { typeof(A), typeof(B) };
        protected override Type[] _allowedTypes => AllowedTypes;

        public static explicit operator A(Junction<A, B> obj) { return obj._value is A ? (A)obj._value : default(A); }
        public static implicit operator Junction<A, B>(A obj) { return new Junction<A, B>(obj); }

        public static explicit operator B(Junction<A, B> obj) { return obj._value is B ? (B)obj._value : default(B); }
        public static implicit operator Junction<A, B>(B obj) { return new Junction<A, B>(obj); }
    }

    public class Junction<A, B, C> : BaseJunction
    {
        protected Junction(object value) : base(value) { }

        public static readonly Type[] AllowedTypes = new Type[] { typeof(A), typeof(B), typeof(C) };
        protected override Type[] _allowedTypes => _allowedTypes;

        public static explicit operator A(Junction<A, B, C> obj) { return obj._value is A ? (A)obj._value : default(A); }
        public static implicit operator Junction<A, B, C>(A obj) { return new Junction<A, B, C>(obj); }

        public static explicit operator B(Junction<A, B, C> obj) { return obj._value is B ? (B)obj._value : default(B); }
        public static implicit operator Junction<A, B, C>(B obj) { return new Junction<A, B, C>(obj); }

        public static explicit operator C(Junction<A, B, C> obj) { return obj._value is C ? (C)obj._value : default(C); }
        public static implicit operator Junction<A, B, C>(C obj) { return new Junction<A, B, C>(obj); }
    }

    public class Junction<A, B, C, D> : BaseJunction
    {
        protected Junction(object value) : base(value) { }

        public static readonly Type[] AllowedTypes = new Type[] { typeof(A), typeof(B), typeof(C), typeof(D) };
        protected override Type[] _allowedTypes => _allowedTypes;

        public static explicit operator A(Junction<A, B, C, D> obj) { return obj._value is A ? (A)obj._value : default(A); }
        public static implicit operator Junction<A, B, C, D>(A obj) { return new Junction<A, B, C, D>(obj); }

        public static explicit operator B(Junction<A, B, C, D> obj) { return obj._value is B ? (B)obj._value : default(B); }
        public static implicit operator Junction<A, B, C, D>(B obj) { return new Junction<A, B, C, D>(obj); }

        public static explicit operator C(Junction<A, B, C, D> obj) { return obj._value is C ? (C)obj._value : default(C); }
        public static implicit operator Junction<A, B, C, D>(C obj) { return new Junction<A, B, C, D>(obj); }

        public static explicit operator D(Junction<A, B, C, D> obj) { return obj._value is D ? (D)obj._value : default(D); }
        public static implicit operator Junction<A, B, C, D>(D obj) { return new Junction<A, B, C, D>(obj); }
    }

    public class Junction<A, B, C, D, E> : BaseJunction
    {
        protected Junction(object value) : base(value) { }

        public static readonly Type[] AllowedTypes = new Type[] { typeof(A), typeof(B), typeof(C), typeof(D), typeof(E) };
        protected override Type[] _allowedTypes => _allowedTypes;

        public static explicit operator A(Junction<A, B, C, D, E> obj) { return obj._value is A ? (A)obj._value : default(A); }
        public static implicit operator Junction<A, B, C, D, E>(A obj) { return new Junction<A, B, C, D, E>(obj); }

        public static explicit operator B(Junction<A, B, C, D, E> obj) { return obj._value is B ? (B)obj._value : default(B); }
        public static implicit operator Junction<A, B, C, D, E>(B obj) { return new Junction<A, B, C, D, E>(obj); }

        public static explicit operator C(Junction<A, B, C, D, E> obj) { return obj._value is C ? (C)obj._value : default(C); }
        public static implicit operator Junction<A, B, C, D, E>(C obj) { return new Junction<A, B, C, D, E>(obj); }

        public static explicit operator D(Junction<A, B, C, D, E> obj) { return obj._value is D ? (D)obj._value : default(D); }
        public static implicit operator Junction<A, B, C, D, E>(D obj) { return new Junction<A, B, C, D, E>(obj); }

        public static explicit operator E(Junction<A, B, C, D, E> obj) { return obj._value is E ? (E)obj._value : default(E); }
        public static implicit operator Junction<A, B, C, D, E>(E obj) { return new Junction<A, B, C, D, E>(obj); }
    }
}
