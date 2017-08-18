namespace CreateAR.Commons.Unity.DataStructures
{
    /// <summary>
    /// Provides static methods of creating tuples that are compatible with
    /// future .NET versions.
    /// </summary>
    public static class Tuple
    {
        /// <summary>
        /// Creates a new Tuple.
        /// </summary>
        /// <typeparam name="T1">Type parameter of item1.</typeparam>
        /// <typeparam name="T2">Type parameter of item2.</typeparam>
        /// <param name="item1">item1</param>
        /// <param name="item2">item2</param>
        /// <returns></returns>
        public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new Tuple<T1, T2>(item1, item2);
        }

        /// <summary>
        /// Creates a new Tuple.
        /// </summary>
        /// <typeparam name="T1">Type parameter of item1.</typeparam>
        /// <typeparam name="T2">Type parameter of item2.</typeparam>
        /// <typeparam name="T3">Type parameter of item3.</typeparam>
        /// <param name="item1">item1</param>
        /// <param name="item2">item2</param>
        /// <param name="item3">item3</param>
        /// <returns></returns>
        public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
        {
            return new Tuple<T1, T2, T3>(item1, item2, item3);
        }

        /// <summary>
        /// Creates a new Tuple.
        /// </summary>
        /// <typeparam name="T1">Type parameter of item1.</typeparam>
        /// <typeparam name="T2">Type parameter of item2.</typeparam>
        /// <typeparam name="T3">Type parameter of item3.</typeparam>
        /// <typeparam name="T4">Type parameter of item4.</typeparam>
        /// <param name="item1">item1</param>
        /// <param name="item2">item2</param>
        /// <param name="item3">item3</param>
        /// <param name="item4">item4</param>
        /// <returns></returns>
        public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
        }
    }

    /// <summary>
    /// Tuple API compatible with container from future .NET versions.
    /// </summary>
    /// <typeparam name="T1">Type of first argument.</typeparam>
    /// <typeparam name="T2">Type of second argument.</typeparam>
    public class Tuple<T1, T2>
    {
        /// <summary>
        /// Retrieves the first item.
        /// </summary>
        public T1 Item1 { get; private set; }

        /// <summary>
        /// Retrieves the second item.
        /// </summary>
        public T2 Item2 { get; private set; }

        /// <summary>
        /// Creates a new tuple
        /// </summary>
        /// <param name="item1">First item.</param>
        /// <param name="item2">Second item.</param>
        internal Tuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }
    
    /// <summary>
    /// Tuple API compatible with container from future .NET versions.
    /// </summary>
    /// <typeparam name="T1">Type of first argument.</typeparam>
    /// <typeparam name="T2">Type of second argument.</typeparam>
    /// <typeparam name="T3">Type of third argument.</typeparam>
    public class Tuple<T1, T2, T3>
    {
        /// <summary>
        /// Retrieves the first item.
        /// </summary>
        public T1 Item1 { get; private set; }

        /// <summary>
        /// Retrieves the second item.
        /// </summary>
        public T2 Item2 { get; private set; }

        /// <summary>
        /// Retrieves the third item.
        /// </summary>
        public T3 Item3 { get; private set; }
        
        /// <summary>
        /// Creates a new tuple
        /// </summary>
        /// <param name="item1">First item.</param>
        /// <param name="item2">Second item.</param>
        /// <param name="item3">Third item.</param>
        internal Tuple(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }
    }

    /// <summary>
    /// Tuple API compatible with container from future .NET versions.
    /// </summary>
    /// <typeparam name="T1">Type of first argument.</typeparam>
    /// <typeparam name="T2">Type of second argument.</typeparam>
    /// <typeparam name="T3">Type of third argument.</typeparam>
    /// <typeparam name="T4">Type of fourth argument.</typeparam>
    public class Tuple<T1, T2, T3, T4>
    {
        /// <summary>
        /// Retrieves the first item.
        /// </summary>
        public T1 Item1 { get; private set; }

        /// <summary>
        /// Retrieves the second item.
        /// </summary>
        public T2 Item2 { get; private set; }

        /// <summary>
        /// Retrieves the third item.
        /// </summary>
        public T3 Item3 { get; private set; }

        /// <summary>
        /// Retrieves the fourth item.
        /// </summary>
        public T4 Item4 { get; private set; }

        /// <summary>
        /// Creates a new tuple
        /// </summary>
        /// <param name="item1">First item.</param>
        /// <param name="item2">Second item.</param>
        /// <param name="item3">Third item.</param>
        /// <param name="item4">Fourth item.</param>
        internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }
    }
}