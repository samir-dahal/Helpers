private DateTime LastFlyoutHiddenUtcDateTime { get; set; }
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(FlyoutIsPresented))
            {
                if (!FlyoutIsPresented)
                {
                    LastFlyoutHiddenUtcDateTime = DateTime.UtcNow;
                }
            }
        }
        private bool WasNavigationCancelledToCloseFlyoutAndReRunAfterADelayToAvoidJitteryFlyoutCloseTransitionBug = false;

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            if (!WasNavigationCancelledToCloseFlyoutAndReRunAfterADelayToAvoidJitteryFlyoutCloseTransitionBug)
            {
                // if the above value is true, then this is the re-run navigation from the GoToAsync(args.Target) call below - skip this block this second pass through, as the flyout is now closed
                if ((DateTime.UtcNow - LastFlyoutHiddenUtcDateTime).TotalMilliseconds < 1000)
                {
                    args.Cancel();

                    FlyoutIsPresented = false;

                    OnPropertyChanged(nameof(FlyoutIsPresented));

                    await Task.Delay(300);

                    WasNavigationCancelledToCloseFlyoutAndReRunAfterADelayToAvoidJitteryFlyoutCloseTransitionBug = true;

                    // re-run the originally requested navigation
                    await GoToAsync(args.Target);

                    return;
                }
            }

            WasNavigationCancelledToCloseFlyoutAndReRunAfterADelayToAvoidJitteryFlyoutCloseTransitionBug = false;

            base.OnNavigating(args);
        }