namespace Holorama.Logic.Abstract.General
{
    /// <summary>
    /// Instance implementing IAware is awared obout something in given object
    /// </summary>
    public interface IAware
    {
        /// <summary>
        /// Returns awared object in given object
        /// </summary>
        /// <typeparam name="TRequested"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        TRequested GetAwared<TRequested>(object @object);
    }

    /// <summary>
    /// Specialized implementation of IAware. Returns instances of <see cref="TAwared"/>.
    /// </summary>
    /// <typeparam name="TAwared"></typeparam>
    public interface IAware<out TAwared>
    {
        TAwared GetAwared();
    }
    
    /// <summary>
    /// Specialized implementation of IAware. Knows about awared only in instances of TSource.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TAwared"></typeparam>
    public interface IAware<in TSource, out TAwared>
    {
        TAwared GetAwared(TSource source);
    }
}