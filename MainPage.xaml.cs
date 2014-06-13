using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WikiSpeak.Resources;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WikiSpeak
{
	public partial class MainPage : PhoneApplicationPage
	{
		// Constructor
		public MainPage()
		{
			InitializeComponent();

			// Set the data context of the listbox control to the sample data
			DataContext = App.MainViewModel;

            // Refresh the app bar
            RefreshAppBar();

			// Sample code to localize the ApplicationBar
			//BuildLocalizedApplicationBar();
		}

		// Load data for the ViewModel Items
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (!App.MainViewModel.IsDataLoaded)
			{
				App.MainViewModel.LoadData();
			}
		}

        /// <summary> 
        /// Recursive get the item. 
        /// </summary> 
        /// <typeparam name="T">The item to get.</typeparam> 
        /// <param name="parents">Parent container.</param> 
        /// <param name="objectList">Item list</param> 
        public static void GetItemsRecursive<T>(DependencyObject parents, ref List<T> objectList) where T : DependencyObject
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(parents);


            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parents, i);


                if (child is T)
                {
                    objectList.Add(child as T);
                }


                GetItemsRecursive<T>(child, ref objectList);
            }


            return;
        } 

        /// <summary>
        /// Because the app bar isn't a real SilverLight control, we need to resort to this instead of clean bindings :(
        /// </summary>
        private void RefreshAppBar()
        {
            ApplicationBarIconButton articleViewListToggle = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            if (articleViewListToggle != null)
            {
                articleViewListToggle.IconUri = App.MainViewModel.ArticleListViewToggleIconUri;
            }
        }

        private void ArticleUserControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ArticleUserControl newSelection = sender as ArticleUserControl;

            if (newSelection != null)
            {
                // Get the list of items in the LongListSelector
                List<UserControl> listItems = new List<UserControl>();
                GetItemsRecursive<UserControl>(ArticleList, ref listItems);

                // Iterate through the list items. Select / unselect as appropriate
                foreach (UserControl listItem in listItems)
                {
                    bool result = VisualStateManager.GoToState(listItem, listItem == newSelection ? "Selected" : "Normal", true);
                }
            }
        }

        /// <summary>
        /// Handles the event where a user article's play button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArticleUserControl_Play(object sender, EventArgs e)
        {
            ArticleUserControl article = sender as ArticleUserControl;
            if (article != null)
            {
                App.MainViewModel.CurrentArticle = (article.DataContext as ViewModels.ArticleViewModel);
                //Browser.NavigateToString(App.MainViewModel.CurrentArticleHTMLRepresentation);
                App.MainViewModel.IsArticleViewVisible = true;
            }
        }

        /// <summary>
        /// Handler for the Add Article app bar button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddArticle_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddArticle.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Handler for the app bar button that toggles between the article lsit / view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArticleViewListToggle_Click(object sender, EventArgs e)
        {
            App.MainViewModel.IsArticleViewVisible = !App.MainViewModel.IsArticleViewVisible;
            RefreshAppBar();
        }

        /// <summary>
        /// Handler for Back key. If we're in the article view, we want to stop reading and go back to the list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (App.MainViewModel.IsArticleViewVisible)
            {
                App.MainViewModel.IsArticleViewVisible = false;
                e.Cancel = true;
            }
        }

		// Sample code for building a localized ApplicationBar
		//private void BuildLocalizedApplicationBar()
		//{
		//    // Set the page's ApplicationBar to a new instance of ApplicationBar.
		//    ApplicationBar = new ApplicationBar();

		//    // Create a new button and set the text value to the localized string from AppResources.
		//    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
		//    appBarButton.Text = AppResources.AppBarButtonText;
		//    ApplicationBar.Buttons.Add(appBarButton);

		//    // Create a new menu item with the localized string from AppResources.
		//    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
		//    ApplicationBar.MenuItems.Add(appBarMenuItem);
		//}
	}
}