using Xamarin.Forms;

namespace iChave.Control
{
    public class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage(Page root) : base(root)
        {
            Title = root.Title;
            IconImageSource = root.IconImageSource;
        }
        public CustomNavigationPage()
        {
        }
    }
}