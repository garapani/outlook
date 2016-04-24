using System;

namespace DotNetApp.Toolkit.Requests
{
    public interface IRequest<T> where T : RequestArgs
    {
        #region Properties

        Action<T> Callback { get; set; }
        
        #endregion

        #region Methods

        void Send();

        #endregion
    }
}