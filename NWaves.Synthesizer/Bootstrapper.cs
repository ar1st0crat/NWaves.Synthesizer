using Caliburn.Micro;
using NWaves.Synthesizer.Interfaces;
using NWaves.Synthesizer.Services;
using NWaves.Synthesizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace NWaves.Synthesizer
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container.Instance(_container);

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();

            _container
                .PerRequest<IAudioService, AudioService>()
                .PerRequest<IKeyNoteMapperService, KeyNoteMapperService>()
                .PerRequest<ISynthesizerService, SynthesizerService>();

            _container
               .PerRequest<MainWindowViewModel>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainWindowViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
