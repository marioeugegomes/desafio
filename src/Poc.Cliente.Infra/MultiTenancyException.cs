using System;

[Serializable]
public class MultiTenancyException : Exception
{
    public MultiTenancyException() : base() { }
    public MultiTenancyException(string message) : base(message) { }
    public MultiTenancyException(string message, Exception inner) : base(message, inner) { }

    protected MultiTenancyException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}