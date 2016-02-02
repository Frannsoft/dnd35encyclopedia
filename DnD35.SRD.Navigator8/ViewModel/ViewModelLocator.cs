/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:DnD35.SRD.Navigator8"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using DnD35.SRD.Navigator8.Navigation;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.Model.Browse;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Model.Menu;
using DnDNavigator.Runtime.Model.Search;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace DnD35.SRD.Navigator8.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IMenuOptionsService, MenuOptionsService>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<ISearchService, SearchService>();
            SimpleIoc.Default.Register<IEntryService, EntryService>();
            SimpleIoc.Default.Register<IBrowseListService<Monster>, MonsterBrowseListService>();
            SimpleIoc.Default.Register<IBrowseListService<Spell>, SpellBrowseListService>();
            SimpleIoc.Default.Register<IBrowseListService<Class>, ClassBrowseListService>();
            SimpleIoc.Default.Register<IBrowseListService<Domain>, DomainBrowseListService>();
            SimpleIoc.Default.Register<IBrowseListService<Skill>, SkillBrowseListService>();
            SimpleIoc.Default.Register<IBrowseListService<Equipment>, EquipmentBrowseListService>();
            SimpleIoc.Default.Register<IBrowseListService<Item>, ItemBrowseListService>();
            SimpleIoc.Default.Register<IBrowseListService<Power>, PowerBrowseListService>();
            SimpleIoc.Default.Register<IBrowseListService<Feat>, FeatBrowseListService>();
            SimpleIoc.Default.Register<IHistoryService, HistoryService>();
            SimpleIoc.Default.Register<IFavoriteService, FavoriteService>();
            SimpleIoc.Default.Register<IPlaylistsVMService, PlaylistsVMService>();

            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<MonsterBrowseListViewModel>();
            SimpleIoc.Default.Register<SpellBrowseListViewModel>();
            SimpleIoc.Default.Register<SearchViewModel>();
            SimpleIoc.Default.Register<EntryViewModel>();
            SimpleIoc.Default.Register<ClassBrowseListViewModel>();
            SimpleIoc.Default.Register<DomainBrowseListViewModel>();
            SimpleIoc.Default.Register<SkillBrowseListViewModel>();
            SimpleIoc.Default.Register<EquipmentBrowseListViewModel>();
            SimpleIoc.Default.Register<ItemBrowseListViewModel>();
            SimpleIoc.Default.Register<PowerBrowseListViewModel>();
            SimpleIoc.Default.Register<FeatBrowseListViewModel>();
            SimpleIoc.Default.Register<ProximityViewModel>();
            SimpleIoc.Default.Register<HistoryViewModel>();
            SimpleIoc.Default.Register<FavoriteViewModel>();
            SimpleIoc.Default.Register<PlaylistsViewModel>();
        }

        public HomeViewModel Home
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomeViewModel>();
            }
        }

        public MonsterBrowseListViewModel MonsterViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MonsterBrowseListViewModel>();
            }
        }

        public SpellBrowseListViewModel SpellBrowseListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SpellBrowseListViewModel>();
            }
        }

        public SearchViewModel SearchViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SearchViewModel>();
            }
        }

        public EntryViewModel EntryViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EntryViewModel>();
            }
        }

        public MonsterBrowseListViewModel MonsterBrowseListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MonsterBrowseListViewModel>();
            }
        }

        public ClassBrowseListViewModel ClassBrowseListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ClassBrowseListViewModel>();
            }
        }

        public DomainBrowseListViewModel DomainBrowseListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DomainBrowseListViewModel>();
            }
        }

        public SkillBrowseListViewModel SkillBrowseListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SkillBrowseListViewModel>();
            }
        }

        public EquipmentBrowseListViewModel EquipmentBrowseListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EquipmentBrowseListViewModel>();
            }
        }

        public ItemBrowseListViewModel ItemBrowseListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ItemBrowseListViewModel>();
            }
        }

        public PowerBrowseListViewModel PowerBrowseListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PowerBrowseListViewModel>();
            }
        }

        public FeatBrowseListViewModel FeatBrowseListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FeatBrowseListViewModel>();
            }
        }

        public ProximityViewModel ProximityViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProximityViewModel>();
            }
        }

        public HistoryViewModel HistoryViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HistoryViewModel>();
            }
        }

        public FavoriteViewModel FavoriteViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<FavoriteViewModel>();
            }
        }

        public PlaylistsViewModel PlaylistsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlaylistsViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}