using System.Windows;
using Microsoft.Phone.Controls;
using DnD35.SRD.Navigator8.ViewModel;
using DnDNavigator.Runtime.Model.DnDEntry;
using System.Windows.Controls;
using System.Windows.Navigation;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DnD35.SRD.Navigator8
{
    public partial class BrowseListPage : PhoneApplicationPage
    {
        private EntryType.Types entryType;

        public BrowseListPage()
        {
            InitializeComponent();            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            entryType = (EntryType.Types)(Enum.Parse(typeof(EntryType.Types), NavigationContext.QueryString["entryType"]));

            if(this.DataContext == null)
            {
                SetViewModel();
            }
        }

        private void SetViewModel()
        {
            switch (entryType)
            {
                case EntryType.Types.Monster:
                    {
                        this.DataContext = new MonsterBrowseListViewModel();
                        var vm = (MonsterBrowseListViewModel)DataContext;
                        vm.GetEntriesCommand.Execute(null);
                        break;
                    }
                case EntryType.Types.Spell:
                    {
                        this.DataContext = new SpellBrowseListViewModel();
                        var vm = (SpellBrowseListViewModel)DataContext;
                        vm.GetEntriesCommand.Execute(null);
                        break;
                    }
                case EntryType.Types.Class:
                    {
                        this.DataContext = new ClassBrowseListViewModel();
                        var vm = (ClassBrowseListViewModel)DataContext;
                        vm.GetEntriesCommand.Execute(null);
                        break;
                    }
                case EntryType.Types.Domain:
                    {
                        this.DataContext = new DomainBrowseListViewModel();
                        var vm = (DomainBrowseListViewModel)DataContext;
                        vm.GetEntriesCommand.Execute(null);
                        break;
                    }
                case EntryType.Types.Equipment:
                    {
                        this.DataContext = new EquipmentBrowseListViewModel();
                        var vm = (EquipmentBrowseListViewModel)DataContext;
                        vm.GetEntriesCommand.Execute(null);
                        break;
                    }
                case EntryType.Types.Item:
                    {
                        this.DataContext = new ItemBrowseListViewModel();
                        var vm = (ItemBrowseListViewModel)DataContext;
                        vm.GetEntriesCommand.Execute(null);
                        break;
                    }
                case EntryType.Types.Power:
                    {
                        this.DataContext = new PowerBrowseListViewModel();
                        var vm = (PowerBrowseListViewModel)DataContext;
                        vm.GetEntriesCommand.Execute(null);
                        break;
                    }
                case EntryType.Types.Skill:
                    {
                        this.DataContext = new SkillBrowseListViewModel();
                        var vm = (SkillBrowseListViewModel)DataContext;
                        vm.GetEntriesCommand.Execute(null);
                        break;
                    }
                case EntryType.Types.Feat:
                    {
                        this.DataContext = new FeatBrowseListViewModel();
                        var vm = (FeatBrowseListViewModel)DataContext;
                        vm.GetEntriesCommand.Execute(null);
                        break;
                    }
                default:
                    {
                        //TODO - do nothing for now
                        break;
                    }
            }
        }

        private void SortTypeListPickerSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortTypeListPicker.SelectedIndex != -1)
            {
                if (SortTypeListPicker.SelectedItem != null)
                {
                    if (DataContext is MonsterBrowseListViewModel)
                    {
                        var vm = (MonsterBrowseListViewModel)DataContext;
                        vm.ReorderEntriesCommand.Execute(SortTypeListPicker.SelectedItem);
                    }
                    else if (DataContext is SpellBrowseListViewModel)
                    {
                        var vm = (SpellBrowseListViewModel)DataContext;
                        vm.ReorderEntriesCommand.Execute(SortTypeListPicker.SelectedItem);
                    }
                    else if (DataContext is ClassBrowseListViewModel)
                    {
                        var vm = (ClassBrowseListViewModel)DataContext;
                        vm.ReorderEntriesCommand.Execute(SortTypeListPicker.SelectedItem);
                    }
                    else if (DataContext is DomainBrowseListViewModel)
                    {
                        var vm = (DomainBrowseListViewModel)DataContext;
                        vm.ReorderEntriesCommand.Execute(SortTypeListPicker.SelectedItem);
                    }
                    else if (DataContext is EquipmentBrowseListViewModel)
                    {
                        var vm = (EquipmentBrowseListViewModel)DataContext;
                        vm.ReorderEntriesCommand.Execute(SortTypeListPicker.SelectedItem);
                    }
                    else if (DataContext is FeatBrowseListViewModel)
                    {
                        var vm = (FeatBrowseListViewModel)DataContext;
                        vm.ReorderEntriesCommand.Execute(SortTypeListPicker.SelectedItem);
                    }
                    else if (DataContext is ItemBrowseListViewModel)
                    {
                        var vm = (ItemBrowseListViewModel)DataContext;
                        vm.ReorderEntriesCommand.Execute(SortTypeListPicker.SelectedItem);
                    }
                    else if (DataContext is PowerBrowseListViewModel)
                    {
                        var vm = (PowerBrowseListViewModel)DataContext;
                        vm.ReorderEntriesCommand.Execute(SortTypeListPicker.SelectedItem);
                    }
                    else if (DataContext is SkillBrowseListViewModel)
                    {
                        var vm = (SkillBrowseListViewModel)DataContext;
                        vm.ReorderEntriesCommand.Execute(SortTypeListPicker.SelectedItem);
                    }
                }
            }
        }

        private void FilterListPickerSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortTypeListPicker.SelectedIndex != -1)
            {
                if (SortTypeListPicker.SelectedItem != null)
                {
                    if (DataContext is SpellBrowseListViewModel)
                    {
                        var vm = (SpellBrowseListViewModel)DataContext;
                        vm.FilterEntriesCommand.Execute(FilterListPicker.SelectedItem);
                    }
                }
            }
        }
    }
}