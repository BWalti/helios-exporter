using System;

namespace Helios_PoC
{
    public class VariableDeclaration
    {
        protected object min;
        protected object max;

        public string Code { get; }

        public ushort RegisterCount { get; }

        public string Description { get; }

        public AccessMode Access { get; }

        public Type ValueType { get; }

        public VariableDeclaration(string code, ushort registerCount, string description, AccessMode access,
            Type valueType, string min, string max)
        {
            Code = code;
            RegisterCount = registerCount;
            Description = description;
            Access = access;
            ValueType = valueType;

            try
            {
                var minValue = Convert.ChangeType(min, valueType);
                var maxValue = Convert.ChangeType(max, valueType);

                this.min = minValue;
                this.max = maxValue;
            }
            catch (Exception)
            {
                // no valid boundaries?
            }
        }
    }

    public class VariableDeclaration<T> : VariableDeclaration
    {
        public VariableDeclaration(string code, ushort registerCount, string description, AccessMode access, string min, string max) 
            : base(code, registerCount, description, access, typeof(T), min, max)
        {
        }

        public T Min => (T) this.min;

        public T Max => (T) this.max;
    }
}