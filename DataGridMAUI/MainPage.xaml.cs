namespace DataGridMAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            dataGrid.QueryUnboundColumnValue += DataGrid_QueryUnboundColumnValue;
        }

        private void DataGrid_QueryUnboundColumnValue(object? sender, Syncfusion.Maui.DataGrid.DataGridUnboundColumnEventArgs e)
        {
            var viewModel = this.BindingContext as GradeSheetViewModel;
            if (e.Column.MappingName == "Grade")
                e.Value = viewModel.Grades.FirstOrDefault(o => o.ID == (e.Record as Grade).ID).CalculateFinalGrade();
            else if (e.Column.MappingName == "StudentID")
            {
                var grade = viewModel.Grades.FirstOrDefault(o => o.ID == (e.Record as Grade).ID);
                e.Value = grade.StudentName + " - " + grade.ID;
            }
        }
    }

}
