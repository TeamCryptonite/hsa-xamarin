using System.Collections.Generic;
using System.Linq;
using RestSharp.Portable;

namespace HSAManager
{
    public class Paginator<T>
    {
        private AbstractBizzaroActions BizzaroAction;
        private readonly int PageTake;
        private readonly IRestRequest Request;
        private readonly int SkipBy;
        private int NextSkip;
        private bool ReachedEnd;

        public Paginator(AbstractBizzaroActions bizzaroAction, IRestRequest request, int skipBy = 10, int pageTake = 10)
        {
            BizzaroAction = bizzaroAction;
            Request = request;
            NextSkip = 0;
            SkipBy = skipBy;
            PageTake = pageTake;
        }

        public void Reset()
        {
            // Reset CurrentSkip
            NextSkip = 0;
            ReachedEnd = false;

            // Update Parameters
            Request.AddOrUpdateQueryParameter("skip", NextSkip);
            Request.AddOrUpdateQueryParameter("take", PageTake);
        }

        public IEnumerable<T> Next()
        {
            if (ReachedEnd)
                return new List<T>();

            Request.AddOrUpdateQueryParameter("skip", NextSkip);

            var data = BizzaroAction.CallBizzaro<IEnumerable<T>>(Request).ToList();

            if (!data.Any())
                ReachedEnd = true;

            NextSkip += SkipBy;

            return data;
        }

        public bool HasReachedEnd()
        {
            return ReachedEnd;
        }
    }
}