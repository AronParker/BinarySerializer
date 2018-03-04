using System;

namespace BinarySerializer.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class OneOfAttribute : Attribute
    {
        public OneOfAttribute(params Type[] types)
        {
            Types = types;
        }

        public Type[] Types { get; }
    }
}
