using System;
using System.Collections.Generic;
using System.Text;

namespace FileEmpty;

/// <summary>
/// Represents errors that occur when a file is expected to contain data but is found to be empty.
/// </summary>
public class FileEmptyException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileEmptyException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public FileEmptyException(string message) 
        : base(message)
    { }
}
