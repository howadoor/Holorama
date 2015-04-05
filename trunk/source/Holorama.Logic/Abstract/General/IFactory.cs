namespace Holorama.Logic.Abstract.General
{
    /// <summary>
    /// Represents abstract factory object. It creates new instances of <see cref="TResult"/> based on request of type <see cref="TRequest"/>.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IFactory<out TResult, in TRequest>
    {
        /// <summary>
        /// Creates new instance of <see cref="TResult"/> type based on request in <see cref="TRequest"/>.
        /// </summary>
        /// <param name="request">Request for the creation.</param>
        /// <returns>New instance of <see cref="TResult"/>.</returns>
        TResult Create(TRequest request);
    }

    /// <summary>
    /// Represents abstract factory object. It creates new instances of <see cref="TResult"/> based on request of type <see cref="TRequest"/>.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TTransientRequest"></typeparam>
    /// <typeparam name="TPermanentRequest"></typeparam>
    public interface IFactory<out TResult, in TTransientRequest, in TPermanentRequest>
    {
        /// <summary>
        /// Creates new instance of <see cref="TResult"/> type based on request in <see cref="TRequest"/>.
        /// </summary>
        /// <param name="transient"></param>
        /// <param name="permanent"></param>
        /// <returns>New instance of <see cref="TResult"/>.</returns>
        TResult Create(TTransientRequest transient, TPermanentRequest permanent);
    }
}