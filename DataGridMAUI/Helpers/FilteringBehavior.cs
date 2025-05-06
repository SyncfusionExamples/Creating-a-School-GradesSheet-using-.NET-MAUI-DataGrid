using Syncfusion.Maui.DataGrid;

namespace DataGridMAUI
{
    public class FilteringBehavior : Behavior<SfDataGrid>
    {
        SfDataGrid dataGrid;
        GradeSheetViewModel gradeSheetViewModel;
        protected override void OnAttachedTo(SfDataGrid bindable)
        {
            base.OnAttachedTo(bindable);
            dataGrid = bindable;
            dataGrid.Loaded += DataGrid_Loaded;
        }
        private void DataGrid_Loaded(object? sender, EventArgs e)
        {
            gradeSheetViewModel = dataGrid.BindingContext as GradeSheetViewModel;
            gradeSheetViewModel.Filtertextchanged += this.OnFilterChanged;
        }
        public void OnFilterChanged()
        {
            if (this.dataGrid!.View != null)
            {
                this.dataGrid.View.Filter = this.gradeSheetViewModel!.FilerRecords;
                this.dataGrid.View.RefreshFilter();
            }
        }
        public void OnFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null)
            {
                this.gradeSheetViewModel!.FilterText = string.Empty;
            }
            else
            {
                this.gradeSheetViewModel!.FilterText = e.NewTextValue;
            }
        }

        protected override void OnDetachingFrom(SfDataGrid bindable)
        {
            base.OnDetachingFrom(bindable);
            dataGrid.Loaded -= DataGrid_Loaded;
            gradeSheetViewModel.Filtertextchanged -= this.OnFilterChanged;
            gradeSheetViewModel = null;
        }
    }
}
