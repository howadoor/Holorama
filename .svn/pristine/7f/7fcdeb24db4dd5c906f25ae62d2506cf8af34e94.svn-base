using Holorama.Logic.Abstract.General;
using Holorama.Logic.Image_Synthesis.Abstract;

namespace Holorama.Logic.Concrete.General
{
    /// <summary>
    /// Instance of <see cref="IFactory{ISynthesisItem, RectangleF}"/> using factory pool to create one of randomly choosen instance of <see cref="ISynthesisItem"/>.
    /// </summary>
    public class ProxyFactory<TResult, TRequest> : IFactory<TResult, TRequest>
    {
        private readonly IAware<IFactory<TResult, TRequest>> factoryAware;

        public ProxyFactory(IAware<IFactory<TResult, TRequest>> factoryAware)
        {
            this.factoryAware = factoryAware;
        }

        public TResult Create(TRequest request)
        {
            return factoryAware.GetAwared().Create(request);
        }
    }

    /// <summary>
    /// Instance of <see cref="IFactory{ISynthesisItem, RectangleF}"/> using factory pool to create one of randomly choosen instance of <see cref="ISynthesisItem"/>.
    /// </summary>
    public class ProxyFactory<TResult, TTransientRequest, TPermanentRequest> : IFactory<TResult, TTransientRequest, TPermanentRequest>
    {
        private readonly IAware<IFactory<TResult, TTransientRequest, TPermanentRequest>> factoryAware;

        public ProxyFactory(IAware<IFactory<TResult, TTransientRequest, TPermanentRequest>> factoryAware)
        {
            this.factoryAware = factoryAware;
        }

        public TResult Create(TTransientRequest transient, TPermanentRequest permanent)
        {
            return factoryAware.GetAwared().Create(transient, permanent);
        }
    }
}