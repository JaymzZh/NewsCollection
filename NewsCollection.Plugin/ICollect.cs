namespace NewsCollection.Plugin
{
    public interface ICollect
    {
        /// <summary>
        /// Collect Things Of Today
        /// </summary>
        void CollectToday();

        /// <summary>
        /// Collect Things Of History
        /// </summary>
        void CollectHistory();
    }
}
