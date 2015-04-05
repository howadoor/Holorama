using System.Drawing;
using Holorama.Logic.Abstract;
using Holorama.Logic.Abstract.General;
using Holorama.Logic.Concrete.General;
using Holorama.Logic.Concrete.Generators;
using Holorama.Logic.Concrete.Space_Divisors;
using Ninject.Modules;
using Ninject.Extensions.Conventions;

namespace Holorama.Logic.Setup
{
    public class HoloramaNinjectModule : NinjectModule
    {
        public override void Load()
        {
            // Space divisors
            Kernel.Bind(x => x.FromThisAssembly().SelectAllClasses().InheritedFrom<IFactory<ISpaceDivision, RectangleF, SpaceDivisionOptions>>().BindSingleInterface().Configure(b => b.InSingletonScope())); 

            // Generators
            Kernel.Bind(x => x.FromThisAssembly().SelectAllClasses().InheritedFrom<IGenerator>().BindSingleInterface().Configure(b => b.InSingletonScope()));
            Bind<IAware<IFactory<ISpaceDivision, RectangleF, SpaceDivisionOptions>>>().To<RandomAwareSelector<IFactory<ISpaceDivision, RectangleF, SpaceDivisionOptions>>>();
            Bind<IFactory<ISpaceDivision, RectangleF, SpaceDivisionOptions>>().To<ProxyFactory<ISpaceDivision, RectangleF, SpaceDivisionOptions>>().WhenInjectedInto<BasicExperimentalGenerator>();
        }
    }
}