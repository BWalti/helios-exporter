using System;

namespace Helios_PoC
{
    public class VariableDeclaration
    {
        public string Code { get; }

        public int RegisterCount { get; }

        public string Description { get; }

        public object Min { get; }

        public object Max { get; }

        public AccessMode Access { get; }

        public Type ValueType { get; }

        public VariableDeclaration(string code, int registerCount, string description, AccessMode access,
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

                Min = minValue;
                Max = maxValue;
            }
            catch (Exception)
            {
                // no valid boundaries?
            }
        }
    }
}