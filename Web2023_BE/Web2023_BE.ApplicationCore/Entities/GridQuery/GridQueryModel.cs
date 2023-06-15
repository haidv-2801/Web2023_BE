namespace Web2023_BE.ApplicationCore.Entities
{
    public class GridQueryModel
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public string SortBy { get; set; }
        public string Search { get; set; }
        public string Direction { get; set; }
    }
}
