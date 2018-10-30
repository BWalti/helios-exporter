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
            Type valueType, string min=null, string max=null)
        {
            Code = code;
            RegisterCount = registerCount;
            Description = description;
            Access = access;
            ValueType = valueType;

            SetMinMaxValues(valueType, min, max);
        }

        private void SetMinMaxValues(Type valueType, string min, string max)
        {
            try
            {
                if (min != null)
                {
                    var minValue = Convert.ChangeType(min, valueType);
                    this.min = minValue;
                }
            }
            catch (Exception)
            {
                // no valid boundary?
            }

            try
            {
                if (max != null)
                {
                    var maxValue = Convert.ChangeType(max, valueType);
                    this.max = maxValue;
                }
            }
            catch (Exception)
            {
                // no valid boundary?
            }
        }
    }

    public class VariableDeclaration<T> : VariableDeclaration
    {
        public VariableDeclaration(string code, ushort registerCount, string description, AccessMode access, T min=default, T max=default) 
            : base(code, registerCount, description, access, typeof(T))
        {
            this.min = min;
            this.max = max;
        }

        public T Min => (T) this.min;

        public T Max => (T) this.max;
    }
}