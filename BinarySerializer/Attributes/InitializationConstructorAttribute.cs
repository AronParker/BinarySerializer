using System;

namespace BinarySerializer.Attributes
{

    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class InitializationConstructorAttribute : Attribute
    {
    }
}
