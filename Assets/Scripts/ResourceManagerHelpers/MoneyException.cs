using System;
using System.Runtime.Serialization;

[Serializable]
public class MoneyException : Exception
{
    public MoneyException()
    {
    }

    public MoneyException(string message) : base(message)
    {
    }

    public MoneyException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected MoneyException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}