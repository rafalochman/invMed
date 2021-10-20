using System;

namespace invMed.Services
{
    public class RefreshService
    {
        public static event Action RefreshRequested;
        public static void CallRequestRefresh()
        {
            RefreshRequested?.Invoke();
        }
    }
}
