using Syncfusion.Maui.Data;
using Syncfusion.Maui.DataGrid;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace DataGridMAUI
{    
    public class GradeSheetViewModel : INotifyPropertyChanged
    {
        // IsOpen property for open/close popup
        private bool isOpenForHideColumns;
        private bool isOpenForFilterColumns;
        private bool isOpenForGroupColumns;
        private bool isOpenForSortColumns;
        private bool isOpenForSearch;
        private bool isOnState;       

        // Columns and Data manipulation property definition
        private ColumnCollection columns;
        private GroupColumnDescriptionCollection groupColumns;
        private SortColumnDescriptionCollection sortColumns;
        private string? filtertext = string.Empty;
        private GridColumn? selectedcolumn;
        private GridColumn? selectedgroupColumn;
        private GridColumn? selectedsortcolumn;
        private string? selectedcondition = "Contains";
        internal FilterChanged? Filtertextchanged;
        private ObservableCollection<Grade> grades;
        public ObservableCollection<Grade> Grades 
        {
            get
            {
                return grades;
            }            
            set
            {
                grades = value;
                OnPropertyChanged(nameof(Grades));
            }
        }            
        public List<GridColumn> GridColumns {  get; set; }
        public List<GridColumn> GridColumnsForGroup { get; set; }
        public List<GridColumn> GridColumnsForFilter { get; set; }
        public List<GridColumn> GridColumnsForSort { get; set; }

        // Commands for open popupto handle columns and data manipulation.
        public ICommand HideColumns { get;set; }
        public ICommand HideAllColumns { get; set; }
        public ICommand ShowAllColumns { get; set; }

        public ICommand FilterColumns { get; set; }
        public ICommand ClearFilter { get; set; }

        public ICommand GroupColumns { get; set; }
        public ICommand ClearGroup { get; set; }

        public ICommand SortColumns { get; set; }
        public ICommand ClearSort { get; set; }        

        public List<string> SearchConditions { get; set; }

        public string? SelectedCondition
        {
            get
            {
                return selectedcondition;
            }
            set
            {
                selectedcondition = value;
                OnPropertyChanged(nameof(SelectedCondition));
            }
        }

        public GridColumn? SelectedColumn 
        {
            get
            {
                return selectedcolumn;
            }
            set
            {
                selectedcolumn = value;
                OnPropertyChanged(nameof(SelectedColumn));
            }
        }

        public GridColumn? SelectedGroupColumn
        {
            get
            {
                return selectedgroupColumn;
            }
            set
            {
                selectedgroupColumn = value;
                this.ExecuteAddGrouping();
                OnPropertyChanged(nameof(SelectedGroupColumn));
            }
        }

        public GridColumn? SelectedSortColumn
        {
            get
            {
                return selectedsortcolumn;
            }
            set
            {
                selectedsortcolumn = value;
                ExecuteAddSorting(); 
                OnPropertyChanged(nameof(SelectedSortColumn));
            }
        }

        /// <summary>
        /// Used to send a Notification while Filter Changed
        /// </summary>
        internal delegate void FilterChanged();

        /// <summary>
        /// Gets or sets the value of FilterText and notifies user when value gets changed 
        /// </summary>
        public string? FilterText
        {
            get
            {
                return this.filtertext;
            }

            set
            {
                this.filtertext = value;
                this.OnFilterTextChanged();
                this.OnPropertyChanged("FilterText");
            }
        }
        public bool IsOpenForHideColumns
        {
            get
            {
                return isOpenForHideColumns;
            }
            set
            {
                isOpenForHideColumns = value;
                OnPropertyChanged(nameof(IsOpenForHideColumns));
            }
        }

        public bool IsOpenForFilterColumns
        {
            get
            {
                return isOpenForFilterColumns;
            }
            set
            {
                isOpenForFilterColumns = value;
                OnPropertyChanged(nameof(IsOpenForFilterColumns));
            }
        }

        public bool IsOpenForGroupColumns
        {
            get
            {
                return isOpenForGroupColumns;
            }
            set
            {
                isOpenForGroupColumns = value;
                OnPropertyChanged(nameof(IsOpenForGroupColumns));
            }

        }

        public bool IsOpenForSortColumns
        {
            get
            {
                return isOpenForSortColumns;
            }
            set
            {
                isOpenForSortColumns = value;
                OnPropertyChanged(nameof(IsOpenForSortColumns));
            }
        }

        public bool IsOpenForSearch
        {
            get
            {
                return isOpenForSearch;
            }
            set
            {
                isOpenForSearch = value;
                OnPropertyChanged(nameof(IsOpenForSearch));
            }
        }

        public bool IsOnState
        {
            get
            {
                return isOnState;
            }
            set
            {
                isOnState = value;
                ExecuteAddSorting();
                OnPropertyChanged(nameof(IsOnState));
            }
        }
        public ColumnCollection Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
                OnPropertyChanged(nameof(Columns));
            }
        }

        public GroupColumnDescriptionCollection GroupColumnDescriptions
        {
            get
            {
                return groupColumns;
            }
            set
            {
                groupColumns = value;
                OnPropertyChanged(nameof(GroupColumnDescription));
            }
        }

        public SortColumnDescriptionCollection SortColumnDescriptions
        {
            get
            {
                return sortColumns;
            }
            set
            {
                sortColumns = value;
                OnPropertyChanged(nameof(SortColumnDescriptions));
            }
        }
        public GradeSheetViewModel()
        {            
            PopulateCollection();
            InitializeColumns();            
            InitializeCommands();  
            PopulateSearchCriteria();
            InitializeSortGroupColumnDescriptions();
            PopuplateColumnNames();            
        }

            string[] studentNames = { "Alice Johnson", "Bob Smith", "Charlie Brown", "Diana Prince", "Evan Davis",
                                  "Faith Wilson", "George Harris", "Helen Moore", "Ian Clark", "Jenny Lewis",
                                  "Kyle Robinson", "Laura Scott", "Martin King", "Nina Adams", "Oscar Perez",
                                  "Paula Turner", "Quincy Bell", "Rachel Cox", "Steven Wright", "Tracy Mills" };

            int[] studentIDs = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };

            string[] subjectNames = { "Math", "Science", "History", "English", "Art" };

            // Arrays for grade properties (length should match the number of students * subjects for expanded data)
            int[] assignmentScores = { 90, 78, 85, 92, 88, 95, 80, 86, 81, 82, 88, 79, 84, 92, 80, 90, 83, 87, 91, 85 };
            int[] quizScores = { 85, 82, 80, 88, 80, 90, 75, 87, 84, 79, 91, 82, 88, 90, 81, 92, 86, 89, 87, 88 };
            int[] examScores = { 88, 80, 87, 91, 85, 93, 79, 89, 86, 81, 89, 83, 87, 95, 83, 88, 85, 86, 88, 84 };
            int[] projectScores = { 92, 85, 90, 95, 87, 97, 83, 88, 90, 84, 92, 85, 89, 94, 86, 91, 88, 90, 93, 89 };        
            string[] comments = {
            "Excellent performance", "Needs improvement", "Good effort", "Outstanding", "Satisfactory",
            "Great participation", "Study more", "Consistent work", "Keep up the good work", "Focus more",
            "Excellent progress", "Shows interest", "Needs more practice", "Impressive work", "Catch up on assignments",
            "Solid understanding", "Achieved beyond expectations", "Room for improvement", "Good participation", "Well done"
        };
        private void PopulateCollection()
        {
            // Populate Grades collection
            Grades = new ObservableCollection<Grade>();
            for (int i = 0; i < studentNames.Length; i++)
            {
                Grades.Add(new Grade
                {
                    ID = studentIDs[i],
                    StudentName = studentNames[i],
                    SubjectName = subjectNames[i % subjectNames.Length],
                    AssignmentScore = assignmentScores[i],
                    QuizScore = quizScores[i],
                    ExamScore = examScores[i],
                    ProjectScore = projectScores[i],
                    Comments = comments[i],
                });
            }
        }
        private void InitializeSortGroupColumnDescriptions()
        {
            SortColumnDescriptions = new SortColumnDescriptionCollection();
            GroupColumnDescriptions = new GroupColumnDescriptionCollection();
        }
        private void PopuplateColumnNames()
        {
            var type = typeof(Grade);
            var properties = type.GetProperties();
            GridColumns = new List<GridColumn>();
            GridColumnsForFilter = new List<GridColumn>();
            GridColumnsForGroup = new List<GridColumn>();
            GridColumnsForSort = new List<GridColumn>();
            GridColumnsForFilter.Add(new GridColumn() { Name = "All Columns", DisplayName = "All Columns"});
                columns.ForEach(o =>
            {             
                    var column = new GridColumn() { Name = o.MappingName, DisplayName=o.HeaderText };
                    GridColumns.Add(column);
                    column.PropertyChanged += OnIsCheckedChanged;                
            });
            GridColumnsForFilter.AddRange(GridColumns);
            GridColumnsForGroup.AddRange(GridColumns);
            GridColumnsForSort.AddRange(GridColumns);
            GridColumns.RemoveAt(0);
            GridColumnsForFilter.Remove(GridColumnsForFilter.FirstOrDefault(o => o.Name == "StudentID"));
            SelectedColumn = GridColumnsForFilter.FirstOrDefault();
            SelectedGroupColumn = GridColumnsForGroup.FirstOrDefault(); 
            SelectedSortColumn = GridColumnsForSort.FirstOrDefault();
        }

        private void PopulateSearchCriteria()
        {
            SearchConditions = new List<string>
            {
                "Contains",
                "Equals",
                "Does Not Equal",
            };
        }

        private void OnIsCheckedChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "IsChecked")
            {
                var column = sender as GridColumn;
                columns.First(o => o.MappingName == column.Name).Visible = !column.IsChecked;
            }
        }
        
        private void InitializeColumns()
        {
            columns = new ColumnCollection();
            var studentNameIdUnboundColumn = new DataGridUnboundColumn
            {
                MappingName = "StudentID",
                HeaderText = "Student ID"
            };
            columns.Add(studentNameIdUnboundColumn);
            var studentIdColumn = new DataGridTextColumn
            {
                MappingName = "ID",
                HeaderText = "ID"
            };
            columns.Add(studentIdColumn);
            var studentNameColumn = new DataGridTextColumn
            {
                MappingName = "StudentName",
                HeaderText = "Student Name"
            };
            columns.Add(studentNameColumn);
            var courseNameColumn = new DataGridTextColumn
            {
                MappingName = "SubjectName",
                HeaderText = "Course Name"
            };
            columns.Add(courseNameColumn);
            var assignmentScoreColumn = new DataGridNumericColumn
            {
                MappingName = "AssignmentScore",
                HeaderText = "Assignment Score"
            };
            columns.Add(assignmentScoreColumn);
            var quizScoreColumn = new DataGridNumericColumn
            {
                MappingName = "QuizScore",
                HeaderText = "Quiz Score"
            };
            columns.Add(quizScoreColumn);
            var examScoreColumn = new DataGridNumericColumn
            {
                MappingName = "ExamScore",
                HeaderText = "Exam Score"
            };
            columns.Add(examScoreColumn);
            var projectScoreColumn = new DataGridNumericColumn
            {
                MappingName = "ProjectScore",
                HeaderText = "Project Score"
            };
            columns.Add(projectScoreColumn);
            var gradeColumn = new DataGridUnboundColumn
            {
                MappingName = "Grade",
                HeaderText = "Grade"
            };
            columns.Add(gradeColumn);
            var commentsColumn = new DataGridTextColumn
            {
                MappingName = "Comments",
                HeaderText = "Comments"
            };
            columns.Add(commentsColumn);
        }

        private void InitializeCommands()
        {
            HideColumns = new Command(ExecuteHideColumns);
            ShowAllColumns = new Command(ExecuteShowAllColumns);
            HideAllColumns = new Command(ExecuteHideAllColumns);

            FilterColumns = new Command(ExecuteFilterColumns);
            ClearFilter = new Command(ExecuteClearFilter);
            GroupColumns = new Command(ExecuteGroupColumns);
            ClearGroup = new Command(ExecuteClearGroups);
            SortColumns = new Command(ExecuteSortColumns);
            ClearSort = new Command(ExecuteClearSorts);            
        }

        // sorting
        private void ExecuteSortColumns()
        {
            IsOpenForSortColumns = true;
        }
        private void ExecuteClearSorts()
        {
            if (SortColumnDescriptions == null)
                return;
            SortColumnDescriptions.Clear();

            SelectedSortColumn = null;

            Application.Current?.Dispatcher.Dispatch(() =>
            {
                IsOpenForSortColumns = false;
            });
        }

        private void ExecuteAddSorting()
        {
            if (SelectedSortColumn != null)
            {
                SortColumnDescriptions.Clear();
                var sortColumnDescription = new SortColumnDescription() { ColumnName = this.SelectedSortColumn.Name, SortDirection = IsOnState ? ListSortDirection.Ascending : ListSortDirection.Descending };
                SortColumnDescriptions.Add(sortColumnDescription);
            }
        }

        // Grouping
        private void ExecuteGroupColumns()
        {
            IsOpenForGroupColumns = true;
        }
        private void ExecuteClearGroups()
        {
            if (GroupColumnDescriptions == null)
                return;
            GroupColumnDescriptions.Clear();

            SelectedGroupColumn = null;

            Application.Current?.Dispatcher.Dispatch(() =>
            {
                IsOpenForGroupColumns = false;
            });
        }

        private void ExecuteAddGrouping()
        {
            if (SelectedGroupColumn != null)
            {
                GroupColumnDescriptions.Clear();
                var groupColumnDescription = new GroupColumnDescription() { ColumnName = this.SelectedGroupColumn.Name };
                GroupColumnDescriptions.Add(groupColumnDescription);
            }
        }

        // Show/Hide columns
        private void ExecuteHideColumns()
        {
            IsOpenForHideColumns = true;
        }

        private void ExecuteHideAllColumns()
        {
            foreach (var item in columns)
            {
                if (item.MappingName == "StudentID")
                    continue;
                item.Visible = false;                
            }
            foreach (var item in GridColumns)
            {
                item.IsChecked = true;
                item.OnPropertyChanged("IsChecked");
            }
        }

        private void ExecuteShowAllColumns()
        {
            foreach (var item in columns)
            {
                if (item.MappingName == "StudentID")
                    continue;
                item.Visible = true;                
            }
            foreach (var item in GridColumns)
            {
                item.IsChecked = false;
                item.OnPropertyChanged("IsChecked");
            }
        }

        // Filtering
        private void ExecuteClearFilter()
        {
            SelectedCondition = string.Empty;
            SelectedColumn = null;
            FilterText = string.Empty;
            Application.Current?.Dispatcher.Dispatch(() =>
            {
                IsOpenForFilterColumns = false;
            });
        }

        private void ExecuteFilterColumns()
        {
            IsOpenForFilterColumns = true;
        }

        /// <summary>
        /// used to decide generate records or not
        /// </summary>
        /// <param name="o">object type parameter</param>
        /// <returns>true or false value</returns>
        public bool FilerRecords(object o)
        {
            if (SelectedColumn == null && string.IsNullOrEmpty(this.SelectedCondition))
                return true;

            double res;
            bool checkNumeric = double.TryParse(this.FilterText, out res);
            var item = o as Grade;
            if (item != null && this.FilterText!.Equals(string.Empty) && !string.IsNullOrEmpty(this.FilterText))
            {
                return true;
            }
            else
            {
                if (item != null && SelectedColumn != null)
                {
                    if (checkNumeric && !this.SelectedColumn.Name!.Equals("All Columns") && !this.SelectedCondition!.Equals("Contains"))
                    {
                        bool result = this.MakeNumericFilter(item, this.SelectedColumn.Name, this.SelectedCondition);
                        return result;
                    }
                    else if (this.SelectedColumn.Name!.Equals("All Columns"))
                    {
                        if (item.ID!.ToString().ToLower().Contains(this.FilterText!.ToLower()) ||
                            item.StudentName!.ToString().ToLower().Contains(this.FilterText.ToLower()) ||
                            item.SubjectName!.ToString().ToLower().Contains(this.FilterText.ToLower()) ||
                            item.AssignmentScore!.ToString().ToLower().Contains(this.FilterText.ToLower()) ||
                            item.QuizScore!.ToString().ToLower().Contains(this.FilterText.ToLower())||
                            item.ExamScore!.ToString().ToLower().Contains(this.FilterText.ToLower())||
                            item.ProjectScore!.ToString().ToLower().Contains(this.FilterText.ToLower())||
                            item.Comments!.ToString().ToLower().Contains(this.FilterText.ToLower()))
                            {
                                return true;
                            }
                        return false;
                    }
                    else
                    {
                        bool result = this.MakeStringFilter(item, this.SelectedColumn.Name, this.SelectedCondition!);
                        return result;
                    }
                }
            }
            return false;
        }

        private void OnFilterTextChanged()
        {
            if (this.Filtertextchanged != null)
            {
                this.Filtertextchanged();
            }
        }

        private bool MakeStringFilter(Grade o, string option, string condition)
        {
            var value = o.GetType().GetProperty(option);
            var exactValue = value!.GetValue(o, null);
            exactValue = exactValue!.ToString()!.ToLower();
            string text = this.FilterText!.ToLower();
            var methods = typeof(string).GetMethods();

            if (methods.Count() != 0)
            {
                if (condition == "Contains")
                {
                    var methodInfo = methods.FirstOrDefault(method => method.Name == condition);
                    bool result1 = (bool)methodInfo!.Invoke(exactValue!, new object[] { text })!;
                    return result1;
                }
                else if (exactValue.ToString() == text.ToString())
                {
                    bool result1 = string.Equals(exactValue.ToString(), text.ToString());
                    if (condition == "Equals")
                    {
                        return result1;
                    }
                    else if (condition == "NotEquals")
                    {
                        return false;
                    }
                }
                else if (condition == "NotEquals")
                {
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Used decide to make the numeric filter
        /// </summary>
        /// <param name="o">o</param>
        /// <param name="option">option</param>
        /// <param name="condition">condition</param>
        /// <returns>true or false value</returns>
        private bool MakeNumericFilter(Grade o, string option, string condition)
        {
            var value = o.GetType().GetProperty(option);
            var exactValue = value!.GetValue(o, null);
            double res;
            bool checkNumeric = double.TryParse(exactValue!.ToString(), out res);
            if (checkNumeric)
            {
                switch (condition)
                {
                    case "Equals":
                        try
                        {
                            if (exactValue.ToString() == this.FilterText)
                            {
                                if (Convert.ToDouble(exactValue) == Convert.ToDouble(this.FilterText))
                                {
                                    return true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }

                        break;
                    case "NotEquals":
                        try
                        {
                            if (Convert.ToDouble(this.FilterText) != Convert.ToDouble(exactValue))
                            {
                                return true;
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                            return true;
                        }

                        break;
                }
            }

            return false;
        }

        public event PropertyChangedEventHandler? PropertyChanged; 

        private void OnPropertyChanged(string propertyName)
        {
            if(this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
