# Creating-a-School-GradesSheet-using-.NET-MAUI-DataGrid

This repository demonstrates how to build a simple, practical School Grades Sheet application using the Syncfusion SfDataGrid control in a .NET MAUI app. The sample app shows how to present student grades, compute derived values (such as final grades), and provide common data grid features such as column definitions, unbound columns, filtering behaviors, grouping, sorting, and column visibility toggles.

The project is intentionally small and focused. It contains a MAUI page (`MainPage.xaml`) and its code-behind (`MainPage.xaml.cs`) that wire up the `SfDataGrid` and a `GradeSheetViewModel` which supplies the `Grades` collection and metadata used by the grid. The `MainPage.xaml` demonstrates how to declare the `SfDataGrid` in XAML, attach behaviors, and bind commonly used UI affordances (chips, popups, combo boxes) to view model commands and properties. The `MainPage.xaml.cs` shows how to handle unbound column events to calculate values that are not directly stored on the model (for example, computing the final grade or composing a StudentID display string).

Key highlights
- Uses Syncfusion's `SfDataGrid` for a responsive, feature-rich data table experience across Android, iOS, MacCatalyst and Windows.
- Demonstrates `AutoGenerateColumnsMode="None"` and explicit `Columns` binding so you control the grid schema.
- Shows how to handle `QueryUnboundColumnValue` in code-behind (`MainPage.xaml.cs`) to populate unbound columns like `Grade` or a composed `StudentID` entry.
- Uses `FilteringBehavior` (see `Helpers/FilteringBehavior.cs`) to provide in-grid filtering UI.
- Includes UI components for hide/show columns, filtering, grouping and sorting using Syncfusion popups and chips.

## xaml
The following excerpt from `MainPage.xaml` shows the `SfDataGrid` declaration and some of the UI used to control filtering, grouping and column visibility. The sample binds the grid's `ItemsSource` to `Grades`, sets `AutoGenerateColumnsMode` to `None`, and binds a `Columns` collection so columns can be shown/hidden at runtime.
```
<syncfusion:SfDataGrid x:Name="dataGrid"
                        Grid.Row="1"                               
                        ColumnWidthMode="{OnPlatform Default=Fill, Android=None, iOS=None}"
                        SortColumnDescriptions="{Binding SortColumnDescriptions}"
                        GroupColumnDescriptions="{Binding GroupColumnDescriptions}"
                        AutoGenerateColumnsMode="None"
                        Columns="{Binding Columns}"
                        FrozenColumnCount="1"
                        ItemsSource="{Binding Grades}">
    <syncfusion:SfDataGrid.Behaviors>
        <local:FilteringBehavior/>
    </syncfusion:SfDataGrid.Behaviors>
</syncfusion:SfDataGrid>
```

## C#
This snippet is taken from `MainPage.xaml.cs` and demonstrates subscribing to `QueryUnboundColumnValue` to provide values for unbound columns. The handler retrieves the `GradeSheetViewModel` from the `BindingContext` and computes values such as a final grade or a composite `StudentID` string.
```
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
```

##### Conclusion
 
I hope you enjoyed learning about how to create a School GradesSheet using .NET MAUI DataGrid (SfDataGrid).
 
You can refer to our [.NET MAUI DataGrid’s feature tour](https://www.syncfusion.com/maui-controls/maui-datagrid) page to learn about its other groundbreaking feature representations. You can also explore our [.NET MAUI DataGrid Documentation](https://help.syncfusion.com/maui/datagrid/getting-started) to understand how to present and manipulate data. 
For current customers, you can check out our .NET MAUI components on the [License and Downloads](https://www.syncfusion.com/sales/teamlicense) page. If you are new to Syncfusion, you can try our 30-day [free trial](https://www.syncfusion.com/downloads/maui) to explore our .NET MAUI DataGrid and other .NET MAUI components.
 
If you have any queries or require clarifications, please let us know in the comments below. You can also contact us through our [support forums](https://www.syncfusion.com/forums), [Direct-Trac](https://support.syncfusion.com/create) or [feedback portal](https://www.syncfusion.com/feedback/maui?control=sfdatagrid), or the feedback portal. We are always happy to assist you!
