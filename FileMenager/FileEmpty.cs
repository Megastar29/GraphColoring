using System;
using System.Collections.Generic;
using System.Text;

namespace FileEmpty;

public class FileEmptyException : Exception
{
    public FileEmptyException(string  message) 
        : base(message)
    { }
}
