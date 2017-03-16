using System.Collections.Generic;
using System.Linq;
using RestSharp.Portable;

namespace HSAManager
{
    public class Paginator<T> : AbstractBizzaroActions
    {
        private readonly int PageTake;
        private readonly IRestRequest Request;
        private readonly int SkipBy;
        private int CurrentSkip;
        public IEnumerable<T> Data;
        private bool ReachedEnd;

        public Paginator(string authToken, string baseUrl, IRestRequest request, int skipBy = 10, int pageTake = 10)
            : base(authToken, baseUrl)
        {
            Request = request;
            CurrentSkip = 0;
            SkipBy = skipBy;
            PageTake = pageTake;

            ResetAndRun();
        }

        public void ResetAndRun()
        {
            // Reset CurrentSkip
            CurrentSkip = 0;

            // Update Parameters
            Request.AddOrUpdateQueryParameter("skip", CurrentSkip);
            Request.AddOrUpdateQueryParameter("take", PageTake);

            Data = CallBizzaro<IEnumerable<T>>(Request);
        }

        public void Next()
        {
            if (ReachedEnd)
                return;

            CurrentSkip += SkipBy;

            Request.AddOrUpdateQueryParameter("skip", CurrentSkip);

            Data = CallBizzaro<IEnumerable<T>>(Request);

            if (!Data.Any())
                ReachedEnd = true;
        }

        public bool HasReachedEnd()
        {
            return ReachedEnd;
        }
    }
}