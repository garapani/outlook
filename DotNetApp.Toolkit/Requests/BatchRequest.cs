using System;
using System.Linq;
using System.Threading;

namespace DotNetApp.Toolkit.Requests
{
    public abstract class BatchRequest<T> : IRequest<T> where T : RequestArgs
    {
        #region Fields

        private readonly object _synchronizationObject = new object();
        private DateTime _startTime;
        private readonly Action<T> _callback;
        private RequestArgs[] _allRequestArgs;
        private bool _hasFailed;

        #endregion

        #region Constructors

        protected BatchRequest(Action<T> callback)
        {
            _callback = callback;
        }

        #endregion

        #region Properties

        public Action<T> Callback { get; set; }
        protected IRequest<T>[] Requests { get; set; }
        protected uint ResponseTimeInMs { get; private set; }

        #endregion

        #region Methods

        public void Send()
        {
            _allRequestArgs = new RequestArgs[Requests.Length];
            _startTime = DateTime.Now;

            for (int requestIndex = 0; requestIndex < Requests.Length; requestIndex++)
            {
                ThreadPool.QueueUserWorkItem(SendRequest, requestIndex);
            }
        }

        #endregion

        #region Event Handlers

        protected abstract void OnRequestsCompleted(RequestArgs[] allRequestArgs, bool isSuccess);

        #endregion

        #region Proterted Methods

        protected void InvokeCallback(T requestArgs)
        {
            if (_callback != null)
            {
                _callback.Invoke(requestArgs);
            }
        }

        #endregion

        #region Private Methods

        private void SendRequest(object state)
        {
            int requestIndex = (int) state;

            Requests[requestIndex].Callback += delegate(T requestResultArgs)
                                                   {
                                                       if (requestResultArgs != null && requestResultArgs.IsSuccess)
                                                       {
                                                           _allRequestArgs[requestIndex] = requestResultArgs;

                                                           ReportIfAllRequestsCompletedSuccessfully();
                                                       }
                                                       else
                                                       {
                                                           ReportError();
                                                       }
                                                   };

            Requests[requestIndex].Send();
        }

        private void ReportIfAllRequestsCompletedSuccessfully()
        {
            lock (_synchronizationObject)
            {
                if (_allRequestArgs.All(requestArgs => requestArgs != null))
                {
                    ResponseTimeInMs = (uint)((DateTime.Now - _startTime).TotalMilliseconds);

                    OnRequestsCompleted(_allRequestArgs, true);
                }
            }
        }

        private void ReportError()
        {
            lock (_synchronizationObject)
            {
                if (!_hasFailed)
                {
                    _hasFailed = true;

                    OnRequestsCompleted(_allRequestArgs, false);
                }
            }
        }

        #endregion
    }
}