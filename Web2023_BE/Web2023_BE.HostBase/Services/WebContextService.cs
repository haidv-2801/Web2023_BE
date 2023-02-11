
using Web2023_BE.ApplicationCore;

namespace Web2023_BE.Cache
{
    public class WebContextService : IContextService
    {
        private ContextData _context = null;

        public WebContextService()
        {
        }

        public ContextData GetContext()
        {
            //TODO fix dev
            if (_context == null)
            {
                _context = new ContextData
                {
                    //TenantId = 1,
                    DatabaseId = 1,
                    UserId = 1,
                };
            }

            return _context;
        }

        public void SetContext(ContextData context)
        {
            _context = context;
        }
    }
}
