using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Holorama.Logic.Abstract;
using Holorama.Logic.Concrete.Generators;
using Holorama.Logic.Image_Synthesis;
using Holorama.Logic.Image_Synthesis.Abstract;
using Holorama.Logic.Setup;
using Ninject;
using Ninject.Modules;


namespace Holorama.Application.Setup
{
    static class NinjectKernelSetup
    {
        internal static IKernel CreateKernel()
        {
            IKernel kernel = new StandardKernel();
            SetupKernel(kernel);
            return kernel;
        }

        private static void SetupKernel(IKernel kernel)
        {
            kernel.Load<ImageSynthesisNinjectModule>();
            kernel.Load<HoloramaNinjectModule>();
            kernel.Load<ApplicationNinjectModule>();
        }
    }

    internal class ApplicationNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IGenerator>().To<BasicExperimentalGenerator>().WhenInjectedInto<MainWindow>();
        }
    }
}
