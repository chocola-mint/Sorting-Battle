namespace ChocoUtil.Algorithms
{
    /// <summary>
    /// A struct that associates a float weight with a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct WeightedValue<T>
    {
        /// <summary>
        /// The weight of the value.
        /// </summary>
        public readonly float weight;
        /// <summary>
        /// The value associated with the weight.
        /// </summary>
        public readonly T value;
        /// <summary>
        /// Constructs a <see cref="WeightedValue{T}"/>.
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="value"></param>
        public WeightedValue(float weight, T value) { 
            this.weight = weight; 
            this.value = value; 
        }    
    }
}
