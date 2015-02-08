using Ninject;
using PillBox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Services.DI
{
    public static class NinjectBootstrapper
    {
        public static IKernel Kernel { get; private set; }

        public static void Initialize()
        {
            Kernel = new StandardKernel(new DICoreModule(Constants.DB_NAME));
        }
    }
}
