namespace BT03_adminPage.Services
{
    public interface IDashboardService
    {
        (int users, int products, int orders) GetCounts();
    }

    public class DashboardService : IDashboardService
    {
        public (int users, int products, int orders) GetCounts()
        {
            // Placeholder counts; replace with real data access as needed
            return (users: 123, products: 456, orders: 789);
        }
    }
}


