using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WikiSpeak
{
    public partial class AddArticle : PhoneApplicationPage
    {
        public AddArticle()
        {
            InitializeComponent();

            this.DataContext = App.AddArticleViewModel;
        }

        private void Language_Click(object sender, EventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DoSearchTermSearch();
        }

        /// <summary>
        /// Call do do a search based on what's in the SearchTerm box
        /// </summary>
        private void DoSearchTermSearch()
        {
            SearchForArticlesAsync(SearchTerm.Text, "en");
        }

        private async Task SearchForArticlesAsync(string searchTerm, string language)
        {
            App.AddArticleViewModel.Articles.Clear();
            WikipediaHelper.ArticleTitleAndUrl[] articles = await WikipediaHelper.SearchForArticlesAsync(searchTerm, language, 10);
            foreach (WikipediaHelper.ArticleTitleAndUrl article in articles)
            {
                App.AddArticleViewModel.Articles.Add(
                    new ViewModels.ArticleViewModel()
                    {
                        Article = new Article(article)
                    });
            }
        }

        private void ArticleUserControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ArticleUserControl newSelection = sender as ArticleUserControl;

            if (newSelection != null)
            {
                // Get the list of items in the LongListSelector
                List<UserControl> listItems = new List<UserControl>();
                MainPage.GetItemsRecursive<UserControl>(ArticleList, ref listItems);

                // Iterate through the list items. Select / unselect as appropriate
                foreach (UserControl listItem in listItems)
                {
                    bool result = VisualStateManager.GoToState(listItem, listItem == newSelection ? "Selected" : "Normal", true);
                }
            }
        }

        /// <summary>
        /// Handler for when the + button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArticleUserControl_Add(object sender, EventArgs e)
        {
            ArticleUserControl articleUserControl = sender as ArticleUserControl;
            if (articleUserControl != null)
            {
                ViewModels.ArticleViewModel articleViewModel = articleUserControl.DataContext as ViewModels.ArticleViewModel;
                if (articleViewModel != null)
                {
                    App.MainViewModel.Articles.Add(articleViewModel);
                    articleViewModel.Article.DownloadAsync();
                    NavigationService.GoBack();
                }
            }
            
        }

        /// <summary>
        /// Ensures the search box getting focus for the first time results in the (useless) text being selected for easy deletion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTerm_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.Equals(SearchTerm.Text, "search for...", StringComparison.OrdinalIgnoreCase))
            {
                SearchTerm.SelectAll();
            }
        }

        /// <summary>
        /// Handler for when the page is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            SearchTerm.Focus();
        }

        /// <summary>
        /// Handler for when a key is pressed on the keyboard while SearchTerm has focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTerm_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoSearchTermSearch();
            }
        }

    }
}