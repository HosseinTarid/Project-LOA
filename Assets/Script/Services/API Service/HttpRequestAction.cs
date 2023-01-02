using System;
using BestHTTP;

namespace Assets.Script.Services.API_Service
{
    public class HttpRequestAction<T>
    {
        public Action<HTTPRequest,T> OnFinishedSuccess;
        public Action<HTTPRequest> OnFinishedFail;
        public Action<HTTPRequest> OnError;
        public Action<HTTPRequest> OnAborted;
        public Action<HTTPRequest> OnConnectionTimedOut;
        public Action<HTTPRequest> OnTimedOut;

    }
}