
using System;

namespace Presentation.Base
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}