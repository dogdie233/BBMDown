using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBMDown.Requests
{
    public abstract class BaseRequest<TPayload>
    {
        public readonly HttpMethod method;
        public readonly Uri uri;
        public readonly TPayload payload;
    }


}
