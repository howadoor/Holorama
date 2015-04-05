using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Holorama.Logic.Abstract.General;
using Holorama.Logic.Concrete.General;
using Holorama.Logic.Image_Synthesis;
using Holorama.Logic.Image_Synthesis.Abstract;
using Ninject.Modules;

namespace Holorama.Logic.Setup
{
    /// <summary>
    /// Performs a Ninject setup for image synthesisi related classes and tasks
    /// </summary>
    public class ImageSynthesisNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<SynthesisFactory>().To<SynthesisFactory>().InSingletonScope();
            Bind<IFactory<ISynthesisItem, RectangleF>>().To<CircularItemFactory>().InSingletonScope();
            Bind<IFactory<ISynthesisItem, RectangleF>>().To<PolygonalItemFactory>().InSingletonScope();
            
            Bind<IAware<IFactory<ISynthesisItem, RectangleF>>>().To<RandomAwareSelector<IFactory<ISynthesisItem, RectangleF>>>();
            Bind<IFactory<ISynthesisItem, RectangleF>>().To<ProxyFactory<ISynthesisItem, RectangleF>>().WhenInjectedInto<SynthesisFactory>().InSingletonScope();
        }
    }
}
