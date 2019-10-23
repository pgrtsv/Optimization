namespace Optimization.Interfaces
{
    /// <summary>
    /// Товар.
    /// </summary>
    public interface IGood
    {
        /// <summary>
        /// Наименование товара.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Объём, который занимает товар, м3.
        /// </summary>
        double Volume { get; }

        /// <summary>
        /// Цена товара.
        /// </summary>
        decimal Price { get; }
    }
}