﻿namespace DocumentPacker.EventHandling;

using System.Collections.ObjectModel;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Handles the events of applications: Provides requested views and view models.
/// </summary>
/// <seealso cref="IEventHandlerCenter" />
internal class EventHandlerCenter : IEventHandlerCenter
{
    private IApplicationWindow? applicationWindow;
    private IServiceProvider? serviceProvider;
    private bool suppressClosed;
    public event EventHandler? Closed;

    public void Initialize(IServiceProvider provider)
    {
        this.serviceProvider = provider;

        this.applicationWindow =
            this.serviceProvider.GetRequiredKeyedService<IApplicationWindow>(ApplicationElementPart.Window);
        this.Inject(this.applicationWindow);
        this.applicationWindow.Closed += this.HandleClosed;
        this.applicationWindow.Show();
    }

    private event EventHandler<BackLinkEventArgs>? BackLinkCreated;

    private void HandleClosed(object? sender, EventArgs e)
    {
        if (!this.suppressClosed)
        {
            this.applicationWindow?.Dispose();
            this.applicationWindow = null;

            this.Closed?.Invoke(
                this,
                EventArgs.Empty);
        }

        this.suppressClosed = false;
    }

    private void HandleShowViewRequested(object? sender, ShowViewRequestedEventArgs showViewRequestedEventArgs)
    {
        ArgumentNullException.ThrowIfNull(this.serviceProvider);

        if (showViewRequestedEventArgs.View is null)
        {
            var view = this.serviceProvider.GetRequiredKeyedService<IApplicationView>(showViewRequestedEventArgs.Part);
            this.Inject(view);
            showViewRequestedEventArgs.View = view;
        }

        this.ShowViewRequested?.Invoke(
            sender,
            showViewRequestedEventArgs);
    }

    private void Inject(IApplicationView view)
    {
        var views = new Queue<IApplicationView>();
        views.Enqueue(view);
        var viewModels = new Queue<IApplicationViewModel>();

        while (views.Count > 0 || viewModels.Count > 0)
        {
            while (views.Count > 0)
            {
                viewModels.Enqueue(this.InjectViewModel(views.Dequeue()));
            }

            while (viewModels.Count > 0)
            {
                views.Enqueue(this.InjectView(viewModels.Dequeue()));
            }
        }
    }

    private IEnumerable<IApplicationView> InjectView(IApplicationViewModel viewModel)
    {
        ArgumentNullException.ThrowIfNull(this.serviceProvider);

        foreach (var propertyInfo in viewModel.GetType().GetProperties())
        {
            foreach (var applicationPartAttribute in propertyInfo.GetCustomAttributes<ApplicationPartAttribute>(false))
            {
                var view = this.serviceProvider.GetRequiredKeyedService<IApplicationView>(
                    applicationPartAttribute.Part);
                var value = propertyInfo.GetValue(viewModel);
                if (value is ObservableCollection<object> observableCollection)
                {
                    observableCollection.Add(view);
                }
                else
                {
                    propertyInfo.SetValue(
                        viewModel,
                        view);
                }

                yield return view;
            }
        }
    }

    private IEnumerable<IApplicationViewModel> InjectViewModel(IApplicationView view)
    {
        ArgumentNullException.ThrowIfNull(this.serviceProvider);

        var attribute = view.GetType()
        .GetCustomAttribute(
            typeof(DataContextAttribute),
            false);
        if (attribute is not DataContextAttribute dataContextAttribute)
        {
            yield break;
        }

        var viewModel = this.serviceProvider.GetRequiredKeyedService<IApplicationViewModel>(dataContextAttribute.Part);
        this.ShowViewRequested += viewModel.HandleShowViewRequested;
        viewModel.ShowViewRequested += this.HandleShowViewRequested;
        viewModel.BackLinkCreated += (sender, eventArgs) => this.BackLinkCreated?.Invoke(
            sender,
            eventArgs);
        if (viewModel is IHandleBackLink handleBackLink)
        {
            this.BackLinkCreated += handleBackLink.HandleBackLink;
        }

        view.DataContext = viewModel;
        yield return viewModel;
    }

    private event EventHandler<ShowViewRequestedEventArgs>? ShowViewRequested;
}
