using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptJunkie.Common;

namespace ScriptJunkie.Services
{
    public class ServiceManager
    {
        public static ServiceManager Services = new Lazy<ServiceManager>(() => new ServiceManager()).Value;

        /// <summary>
        /// The Argument Service.
        /// </summary>
        public ArgumentService ArgumentService
        {
            get
            {
                return this.GetService<ArgumentService>();
            }
        }

        /// <summary>
        /// The logging service.
        /// </summary>
        public LogService LogService
        {
            get
            {
                return this.GetService<LogService>();
            }
        }

        private ServiceManager()
        {

        }

        private List<object> _services = new List<object>();

        public T GetService<T>()
        {
            Type type = typeof(T);
            T service = (T)_services.SingleOrDefault(i => i.GetType() == type);

            if(service == null || service.Equals(default(T)))
            {
                throw Utilities.Throw<InvalidOperationException>("Service Manager does not contain \"{0}\".", service.GetType());
            }

            return service;
        }

        public void Add(BaseService service)
        {
            // If service manager already contains one with this service.
            if(_services.Any(i => i.GetType() == service.GetType()))
            {
                return;
            }

            _services.Add(service);
        }
    }
}
