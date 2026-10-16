using System;

namespace Vehicles.Core;

public class LogMessageEventArgs : EventArgs
{
    public string Message { get; }

    public LogMessageEventArgs(string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        Message = message;
    }
}