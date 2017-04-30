using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp.Portable;

namespace HSAManager.Helpers.BizzaroHelpers
{
    public class Paginator<T>
    {
        private readonly int PageTake;
        private readonly IRestRequest Request;
        private readonly int SkipBy;
        private readonly AbstractBizzaroActions BizzaroAction;
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

        public async Task<IEnumerable<T>> Next()
        {
            if (ReachedEnd)
                return new List<T>();

            Request.AddOrUpdateQueryParameter("skip", NextSkip);
            IEnumerable<T> taskWithData;

            try
            {
                taskWithData = await BizzaroAction.CallBizzaro<IEnumerable<T>>(Request);
            }
            catch (Exception ex)
            {
                taskWithData = new List<T>();
            }
            var data = taskWithData.ToList();

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